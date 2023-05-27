using MediatR;
using MongoDB.Driver;
using ProfilerCQRS.Queries;
using ProfilerIntegrations.Abstractions;
using ProfilerIntegrations.Entities;

namespace ProfilerCQRS.QueryHandlers;

public class ProfileUpdatedEventsQueryHandler : IRequestHandler<ProfileUpdatedEventsQuery, List<ProfileUpdatedEvent>>
{
    private readonly IMongoCollection<ProfileUpdatedEvent> _events;

    public ProfileUpdatedEventsQueryHandler(IMongoDBService mongoDbService)
    {
        _events = mongoDbService.ProfileUpdatedEvents;
    }

    public async Task<List<ProfileUpdatedEvent>> Handle(ProfileUpdatedEventsQuery request, CancellationToken cancellationToken)
    {
        return await _events.Find(Builders<ProfileUpdatedEvent>.Filter.Empty)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}