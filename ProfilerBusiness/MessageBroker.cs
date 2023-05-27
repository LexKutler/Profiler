using MediatR;
using ProfilerCQRS.Commands;
using ProfilerIntegrations.Abstractions;
using ProfilerIntegrations.Entities;

namespace ProfilerBusiness;

/// <summary>
/// Implementation of <see cref="IMessageBroker"/>
/// </summary>
public class MessageBroker : IMessageBroker
{
    private readonly IMediator _mediator;

    public MessageBroker(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<List<ProfileUpdatedEvent>> PublishProfileUpdatedEvent()
    {
        // First MQTT broker connection check would be advised
        var events = await _mediator.Send(new SeekAndDestroyUpdateEventsCommand());
        // Send events to MQTT here
        return events;
    }
}