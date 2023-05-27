using MediatR;
using MongoDB.Driver;
using ProfilerIntegration.Entities;

namespace ProfilerCQRS.Commands;

public class SeekAndDestroyUpdateEventsCommand : IRequest<List<ProfileUpdatedEvent>>
{
}