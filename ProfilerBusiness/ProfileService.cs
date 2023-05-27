using AutoMapper;
using MediatR;
using MongoDB.Bson;
using ProfilerCQRS.Commands;
using ProfilerCQRS.Queries;
using ProfilerIntegrations.Abstractions;
using ProfilerIntegrations.Entities;
using ProfilerIntegrations.Models;
using ProfilerIntegrations.System;

namespace ProfilerBusiness;

/// <summary>
/// Implementation of <see cref="IProfileService"/>
/// </summary>
public class ProfileService : IProfileService
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public ProfileService(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task<UserProfile?> GetProfileByIdAsync(ObjectId profileId)
    {
        if (profileId == ObjectId.Empty)
        {
            throw new ArgumentException("Id is empty");
        }

        return await _mediator.Send(new ProfileQuery { Id = profileId });
    }

    public async Task<UserProfile> CreateProfileAsync(UserProfile userProfile)
    {
        if (userProfile.Id == ObjectId.Empty)
        {
            userProfile.Id = ObjectId.GenerateNewId();
        }

        if (userProfile.TimeStamp is null or 0)
        {
            userProfile.TimeStamp = DateTime.UtcNow.Ticks;
        }

        // Mongo will throw DuplicateKey exception if profile with same id already exists
        // Which is completely fine, cause we have global exception handler
        await _mediator.Send(new CreateProfileCommand { UserProfile = userProfile });

        return await _mediator.Send(new ProfileQuery { Id = userProfile.Id });
    }

    public async Task<ProfileUpdateResult> UpdateProfileAsync(UserProfile userProfile)
    {
        if (userProfile.Id == ObjectId.Empty)
        {
            throw new ArgumentException("Id is invalid");
        }

        // Update returns UpdateResult, which means it won't throw exception if profile doesn't exist
        // So we need to check if profile exists before updating
        var profileBefore = await _mediator.Send(new ProfileQuery { Id = userProfile.Id });
        if (profileBefore == null)
        {
            throw new KeyNotFoundException("Profile not found");
        }

        var profileBeforeRecord = _mapper.Map<UserProfileRecord>(profileBefore);
        var profileAfterRecord = _mapper.Map<UserProfileRecord>(userProfile);

        if (profileBeforeRecord.Equals(profileAfterRecord))
        {
            throw new InvalidOperationException("Profile update is invalid");
        }

        var updateResult = await _mediator.Send(new UpdateProfileCommand { UserProfile = userProfile });

        if (updateResult.ModifiedCount == 0)
        {
            throw new InvalidOperationException("Profile not modified");
        }

        var profileAfter = await _mediator.Send(new ProfileQuery { Id = userProfile.Id });

        return new ProfileUpdateResult
        {
            ProfileBefore = _mapper.Map<ProfileResponse>(profileBefore),
            ProfileAfter = _mapper.Map<ProfileResponse>(profileAfter)
        };
    }
}