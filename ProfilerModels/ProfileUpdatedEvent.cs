using MongoDB.Bson;

namespace ProfilerModels;
public class ProfileUpdatedEvent
{
    public ObjectId Id { get; set; }
    public Profile PreProfile { get; set; }
    public Profile PostProfile { get; set; }
    public long TimeStamp { get; set; }
}
