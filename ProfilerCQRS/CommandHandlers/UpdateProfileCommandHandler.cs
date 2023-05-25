using MediatR;
using MongoDB.Driver;
using ProfilerCQRS.Commands;
using ProfilerIntegration.Entities;
using ProfilerModels.Abstractions;

namespace ProfilerCQRS.CommandHandlers;
public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, UpdateResult>
{
    private readonly IMongoCollection<UserProfile> _profiles;
    private readonly IMongoCollection<ProfileUpdatedEvent> _profileUpdatedEvents;
    private readonly MongoClient _mongoClient;
    public UpdateProfileCommandHandler(IMongoDBService mongoDbService)
    {
        _profiles = mongoDbService.Profiles;
        _mongoClient = mongoDbService.MongoClient;
        _profileUpdatedEvents = mongoDbService.ProfileUpdatedEvents;
    }
    public async Task<UpdateResult> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        UpdateResult updateResult;
        using var session = await _mongoClient.StartSessionAsync(cancellationToken: cancellationToken);
        session.StartTransaction();

        try
        {
            // Get the existing profile timestamp
            var existingProfile = await _profiles.Find(x => x.Id == request.UserProfile.Id)
                .FirstOrDefaultAsync(cancellationToken);

            // Update configuration for the profile
            var updateConfig = Builders<UserProfile>.Update
                .Set(profile => profile.TimeStamp, DateTime.UtcNow.Ticks);

            foreach (var property in typeof(UserProfile).GetProperties())
            {
                var value = property.GetValue(request.UserProfile);
                updateConfig = updateConfig.Set(property.Name, value);
            }

            // Update profile with filtering by timestamp i.e. implement optimistic locking
            // If timestamp is not matched, then it means that profile was updated by another request
            updateResult = await _profiles.UpdateOneAsync(
                session,
                profile => profile.Id == request.UserProfile.Id &&
                           profile.TimeStamp == existingProfile.TimeStamp,
                updateConfig,
                cancellationToken: cancellationToken);

            if (updateResult.ModifiedCount == 1)
            {
                var profileUpdatedEvent = new ProfileUpdatedEvent
                {
                    Id = existingProfile.Id,
                    UserProfileBefore = existingProfile,
                    UserProfileAfter = request.UserProfile,
                    ExpireAt = DateTime.UtcNow.AddHours(1)
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
        return updateResult;
    }
}
