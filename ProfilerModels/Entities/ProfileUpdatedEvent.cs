using MongoDB.Bson;
using ProfilerModels;

namespace ProfilerIntegration.Entities;
public class ProfileUpdatedEvent
{
    public ObjectId Id { get; set; }
    public Profile PreProfile { get; set; }
    public Profile PostProfile { get; set; }
    public DateTime ExpireAt { get; set; }
}
