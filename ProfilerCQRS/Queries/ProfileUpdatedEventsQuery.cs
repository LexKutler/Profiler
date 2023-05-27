using MediatR;
using ProfilerIntegrations.Entities;

namespace ProfilerCQRS.Queries;

public class ProfileUpdatedEventsQuery : IRequest<List<ProfileUpdatedEvent>>
{
}