using MediatR;
using ProfilerIntegrations.Entities;

namespace ProfilerCQRS.Commands;

public class SeekAndDestroyUpdateEventsCommand : IRequest<List<ProfileUpdatedEvent>>
{
}