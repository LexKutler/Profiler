using MediatR;
using MongoDB.Driver;
using ProfilerCQRS.Queries;
using ProfilerIntegrations.Abstractions;
using ProfilerIntegrations.Entities;

namespace ProfilerCQRS.QueryHandlers;

public class ProfileByIdQueryHandler : IRequestHandler<ProfileQuery, UserProfile>
{
    private readonly IMongoCollection<UserProfile> _profiles;

    public ProfileByIdQueryHandler(IMongoDBService mongoDbService)
    {
        _profiles = mongoDbService.Profiles;
    }

    public async Task<UserProfile> Handle(ProfileQuery request, CancellationToken cancellationToken)
    {
        return await _profiles.Find(profile => profile.Id == request.Id).FirstOrDefaultAsync(cancellationToken);
    }
}