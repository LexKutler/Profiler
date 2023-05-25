using ProfilerModels;
using ProfilerModels.Abstractions;

namespace ProfilerBusiness;
public class MessageBroker: IMessageBroker
{
    public Task PublishProfileUpdatedEvent(ProfileUpdatedEvent profileUpdatedEvent)
    {
        // Send & log the event
        return Task.CompletedTask;
    }
}
