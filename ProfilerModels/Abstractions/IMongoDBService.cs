using MongoDB.Driver;

namespace ProfilerModels.Abstractions;
public interface IMongoDBService
{
    MongoClient MongoClient { get; }
    IMongoCollection<Profile> Profiles { get; }
    IMongoCollection<ProfileUpdatedEvent> ProfileUpdatedEvents { get; }
}