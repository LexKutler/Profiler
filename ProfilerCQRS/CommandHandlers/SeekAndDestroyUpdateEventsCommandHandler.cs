using MediatR;
using MongoDB.Driver;
using ProfilerCQRS.Commands;
using ProfilerIntegration.Entities;
using ProfilerModels.Abstractions;

namespace ProfilerCQRS.CommandHandlers;

public class SeekAndDestroyUpdateEventsCommandHandler : IRequestHandler<SeekAndDestroyUpdateEventsCommand, List<ProfileUpdatedEvent>?>
{
    private readonly IMongoCollection<ProfileUpdatedEvent> _profileUpdatedEvents;
    private readonly MongoClient _mongoClient;

    public SeekAndDestroyUpdateEventsCommandHandler(IMongoDBService mongoDbService)
    {
        _mongoClient = mongoDbService.MongoClient;
        _profileUpdatedEvents = mongoDbService.ProfileUpdatedEvents;
    }

    public async Task<List<ProfileUpdatedEvent>?> Handle(SeekAndDestroyUpdateEventsCommand request,
        CancellationToken cancellationToken)
    {
        // Avoid any transaction if there are no events
        var eventsPresentCount = await _profileUpdatedEvents.CountDocumentsAsync(
            Builders<ProfileUpdatedEvent>.Filter.Empty,
            cancellationToken: cancellationToken);

        if (eventsPresentCount == 0)
        {
            return null;
        }

        using var session = await _mongoClient.StartSessionAsync(cancellationToken: cancellationToken);
        session.StartTransaction();
        try
        {
            // Get all event at this point in time
            var existingEvents = await _profileUpdatedEvents
                .Find(Builders<ProfileUpdatedEvent>.Filter.Empty)
                .ToListAsync(cancellationToken: cancellationToken);

            // Delete each one of them
            foreach(var profileUpdatedEvent in existingEvents)
            {
                await _profileUpdatedEvents.DeleteOneAsync(
                    profile => profile.Id == profileUpdatedEvent.Id,
                    cancellationToken: cancellationToken);
            }

            // Commit & return
            await session.CommitTransactionAsync(cancellationToken: cancellationToken);
            return existingEvents;
        }
        catch (Exception e)
        {
            // If anything goes wrong, abort the transaction
            await session.AbortTransactionAsync(cancellationToken);
            throw;
        }
    }
}