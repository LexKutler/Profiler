using MediatR;
using MongoDB.Bson;
using ProfilerCQRS.Commands;
using ProfilerCQRS.Queries;
using ProfilerIntegration.Entities;
using ProfilerIntegration.System;
using ProfilerModels;
using ProfilerModels.Abstractions;

namespace ProfilerBusiness;
public class ProfileService: IProfileService
{
    private readonly IMediator _mediator;
    public ProfileService(IMediator mediator)
    {
        _mediator = mediator;
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

        await _mediator.Send(new CreateProfileCommand { UserProfile = userProfile });

        return await _mediator.Send(new GetProfileQuery { Id = userProfile.Id });
    }

    public async Task<ProfileUpdateResult> UpdateProfileAsync(UserProfile userProfile)
    {
        if (userProfile.Id == ObjectId.Empty)
        {
            throw new ArgumentException("Id is invalid");
        }

        if (userProfile.TimeStamp is null or 0)
        {
            throw new ArgumentException("Profile is corrupted");
        }

        var profileBefore = await _mediator.Send(new GetProfileQuery { Id = userProfile.Id }) 
                            ?? throw new KeyNotFoundException("Profile not found");

        var updateResult = await _mediator.Send(new UpdateProfileCommand { UserProfile = userProfile });

        if (updateResult.ModifiedCount == 0)
        {
            throw new InvalidOperationException("Profile not modified");
        }

        var profileAfter = await _mediator.Send(new GetProfileQuery { Id = userProfile.Id });

        return new ProfileUpdateResult
        {
            ProfileBefore = profileBefore,
            ProfileAfter = profileAfter
        };
    }
}
