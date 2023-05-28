using MediatR;
using MongoDB.Driver;
using ProfilerCQRS.Commands;
using ProfilerIntegrations.Abstractions;
using ProfilerIntegrations.Entities;

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
                .Set(profile => profile.FirstName, request.UserProfile.FirstName)
                .Set(profile => profile.LastName, request.UserProfile.LastName)
                .Set(profile => profile.Email, request.UserProfile.Email)
                .Set(profile => profile.UserName, request.UserProfile.UserName)
                .Set(profile => profile.Address, request.UserProfile.Address)
                .Set(profile => profile.City, request.UserProfile.City)
                .Set(profile => profile.State, request.UserProfile.State)
                .Set(profile => profile.Zip, request.UserProfile.Zip)
                .Set(profile => profile.CountryCode, request.UserProfile.CountryCode)
                .Set(profile => profile.PicturePath,
                    request.UserProfile.PicturePath ?? existingProfile.PicturePath)
                .Set(profile => profile.TimeStamp, DateTime.UtcNow.Ticks);

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
                    EventHappened = DateTime.UtcNow.Ticks
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