using MediatR;
using MongoDB.Bson;
using ProfilerIntegration.Entities;

namespace ProfilerCQRS.Queries;

public class ProfileUpdatedEventsQuery : IRequest<List<ProfileUpdatedEvent>>
{
}