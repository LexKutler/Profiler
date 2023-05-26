using ProfilerIntegration.Entities;

namespace ProfilerModels.Abstractions;

public interface IMessageBroker
{
    /// <summary>
    /// This method would seek and destroy events.
    /// It is possible to add MQTT or other message brokers later.
    /// </summary>
    /// <returns></returns>
    Task PublishProfileUpdatedEvent();
}