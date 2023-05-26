using MongoDB.Bson;

namespace ProfilerIntegration.Entities;

public class ProfileUpdatedEvent
{
    public ObjectId Id { get; set; }
    public UserProfile UserProfileBefore { get; set; }
    public UserProfile UserProfileAfter { get; set; }
    public DateTime ExpireAt { get; set; }
}