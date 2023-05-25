using MongoDB.Driver;
using ProfilerIntegration.Entities;

namespace ProfilerModels.Abstractions;
public interface IMongoDBService
{
    MongoClient MongoClient { get; }
    IMongoCollection<UserProfile> Profiles { get; }
    IMongoCollection<ProfileUpdatedEvent> ProfileUpdatedEvents { get; }
}