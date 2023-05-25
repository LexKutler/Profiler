using MediatR;
using MongoDB.Driver;
using ProfilerCQRS.Commands;
using ProfilerModels;
using ProfilerModels.Abstractions;

namespace ProfilerCQRS.CommandHandlers;
public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, long>
{
    private readonly IMongoCollection<Profile> _profiles;
    private readonly IMongoCollection<ProfileUpdatedEvent> _profileUpdatedEvents;
    private readonly MongoClient _mongoClient;
    public UpdateProfileCommandHandler(IMongoDBService mongoDbService)
    {
        _profiles = mongoDbService.Profiles;
        _mongoClient = mongoDbService.MongoClient;
        _profileUpdatedEvents = mongoDbService.ProfileUpdatedEvents;
    }
    public async Task<long> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        long modifiedCount;

        using var session = await _mongoClient.StartSessionAsync(cancellationToken: cancellationToken);
        session.StartTransaction();

        try
        {
            // Get the existing profile timestamp
            var existingProfile = await _profiles.Find(x => x.Id == request.Profile.Id)
                .FirstOrDefaultAsync(cancellationToken);

            // Update configuration for the profile
            var updateConfig = Builders<Profile>.Update
                .Set(profile => profile.TimeStamp, DateTime.UtcNow.Ticks);

            foreach (var property in typeof(Profile).GetProperties())
            {
                var value = property.GetValue(request.Profile);
                updateConfig = updateConfig.Set(property.Name, value);
            }

            // Update profile with filtering by timestamp i.e. implement optimistic locking
            // If timestamp is not matched, then it means that profile was updated by another request
            var updateResult = await _profiles.UpdateOneAsync(
                session,
                profile => profile.Id == request.Profile.Id &&
                           profile.TimeStamp == existingProfile.TimeStamp,
                updateConfig,
                cancellationToken: cancellationToken);

            modifiedCount = updateResult.ModifiedCount;

            if (updateResult.ModifiedCount == 1)
            {
                var profileUpdatedEvent = new ProfileUpdatedEvent
                {
                    Id = existingProfile.Id,
                    PreProfile = existingProfile,
                    PostProfile = request.Profile,
                    TimeStamp = DateTime.UtcNow.Ticks
                };

                await _profileUpdatedEvents.InsertOneAsync(
                    session,
                    profileUpdatedEvent,
                    cancellationToken: cancellationToken);

                // At this point, profile is updated and event is saved
                // So it's safe to commit the transaction
                await session.CommitTransactionAsync(cancellationToken: cancellationToken);
            }
            else
            {
                throw new OperationCanceledException("Profile was updated by another request");
            }
        }
        catch (Exception e)
        {
            await session.AbortTransactionAsync(cancellationToken);
            throw;
        }
        return modifiedCount;
    }
}
