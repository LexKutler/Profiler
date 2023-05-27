using ProfilerIntegration.Entities;

namespace ProfilerModels.Abstractions;

/// <summary>
/// Service is sending events to MQTT in fire-and-forget fashion
/// </summary>
public interface IMessageBroker
{
    /// <summary>
    /// This method would seek, destroy and publish events.
    /// </summary>
    Task<List<ProfileUpdatedEvent>> PublishProfileUpdatedEvent();
}