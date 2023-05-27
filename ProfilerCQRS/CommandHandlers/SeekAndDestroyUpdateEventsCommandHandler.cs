using MediatR;
using MongoDB.Driver;
using ProfilerCQRS.Commands;
using ProfilerIntegrations.Abstractions;
using ProfilerIntegrations.Entities;

namespace ProfilerCQRS.CommandHandlers;

public class SeekAndDestroyUpdateEventsCommandHandler :
    IRequestHandler<SeekAndDestroyUpdateEventsCommand, List<ProfileUpdatedEvent>>
{
    private readonly IMongoCollection<ProfileUpdatedEvent> _profileUpdatedEvents;
    private readonly MongoClient _mongoClient;

    public SeekAndDestroyUpdateEventsCommandHandler(IMongoDBService mongoDbService)
    {
        _mongoClient = mongoDbService.MongoClient;
        _profileUpdatedEvents = mongoDbService.ProfileUpdatedEvents;
    }

    public async Task<List<ProfileUpdatedEvent>> Handle(SeekAndDestroyUpdateEventsCommand request,
        CancellationToken cancellationToken)
    {
        // Avoid any transaction if there are no events
        var eventsPresentCount = await _profileUpdatedEvents.CountDocumentsAsync(
            Builders<ProfileUpdatedEvent>.Filter.Empty,
            cancellationToken: cancellationToken);

        if (eventsPresentCount == 0)
        {
            return new List<ProfileUpdatedEvent>();
        }

        using var session = await _mongoClient.StartSessionAsync(cancellationToken: cancellationToken);
        session.StartTransaction();
        try
        {
            // Get all event at this point in time
            var existingEvents = await _profileUpdatedEvents
                .Find(Builders<ProfileUpdatedEvent>.Filter.Empty)
                .ToListAsync(cancellationToken: cancellationToken);

            // This move has a potential to save some time under higher loads
            var existingEventsHashSet = new HashSet<ProfileUpdatedEvent>(existingEvents);

            // Delete each one of them
            await _profileUpdatedEvents.DeleteManyAsync(
                profileUpdatedEvent => existingEventsHashSet.Contains(profileUpdatedEvent),
                cancellationToken: cancellationToken);

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