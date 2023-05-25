using MediatR;
using MongoDB.Bson;
using ProfilerCQRS.Commands;
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

    public async Task CreateProfile(Profile profile)
    {
        if (profile.Id == ObjectId.Empty)
        {
            profile.Id = ObjectId.GenerateNewId();
        }

        if (profile.TimeStamp is null or 0)
        {
            profile.TimeStamp = DateTime.UtcNow.Ticks;
        }

        await _mediator.Send(new CreateProfileCommand { Profile = profile });
    }

    public Task UpdateProfile(Profile profile)
    {
        throw new NotImplementedException();
    }
}
