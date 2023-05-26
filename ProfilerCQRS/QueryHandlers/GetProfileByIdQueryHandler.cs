using MediatR;
using MongoDB.Driver;
using ProfilerCQRS.Queries;
using ProfilerIntegration.Entities;
using ProfilerModels.Abstractions;

namespace ProfilerCQRS.QueryHandlers;

public class GetProfileByIdQueryHandler : IRequestHandler<GetProfileQuery, UserProfile>
{
    private readonly IMongoCollection<UserProfile> _profiles;

    public GetProfileByIdQueryHandler(IMongoDBService mongoDbService)
    {
        _profiles = mongoDbService.Profiles;
    }

    public async Task<UserProfile> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        return await _profiles.Find(profile => profile.Id == request.Id).FirstOrDefaultAsync(cancellationToken);
    }
}