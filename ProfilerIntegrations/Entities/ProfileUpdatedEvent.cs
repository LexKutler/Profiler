using MongoDB.Bson;

namespace ProfilerIntegrations.Entities;

public class ProfileUpdatedEvent
{
    public ObjectId Id { get; set; }
    public UserProfile UserProfileBefore { get; set; }
    public UserProfile UserProfileAfter { get; set; }
    public long EventHappened { get; set; }
}