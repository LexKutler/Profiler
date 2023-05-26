using MediatR;
using Microsoft.Extensions.Hosting;
using ProfilerCQRS.Commands;
using ProfilerIntegration.Entities;
using ProfilerModels.Abstractions;
using Serilog;

namespace ProfilerBusiness;

public class MessageBroker : IMessageBroker
{
    private readonly IMediator _mediator;
    public MessageBroker(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task PublishProfileUpdatedEvent()
    {
        var events = await _mediator.Send(new SeekAndDestroyUpdateEventsCommand());

        // Send events to MQTT here if needed

        await Task.CompletedTask;
    }
}