using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ProfilerBusiness;
using ProfilerModels;
using ProfilerModels.Abstractions;

namespace ProfilerWebAPI.Mongo;
public class MongoDBService: IMongoDBService
{
    public MongoClient MongoClient { get; }
    public IMongoCollection<Profile> Profiles { get; }
    public IMongoCollection<ProfileUpdatedEvent> ProfileUpdatedEvents { get; }
    private readonly IMessageBroker _broker;

    public MongoDBService(IOptions<DatabaseSettings> databaseSettings, IMessageBroker broker)
    {
        _broker = broker;

        var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
        var database = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);

        database.CreateCollection("profiles");
        database.CreateCollection("profileUpdatedEvents");

        var profiles = database.GetCollection<Profile>("profiles");
        var profileUpdatedEvents = database.GetCollection<ProfileUpdatedEvent>("profileUpdatedEvents");

        WatchProfileUpdate(profileUpdatedEvents);

        // This would ensure that the UserName field is unique
        profiles.Indexes.CreateOne(
            new CreateIndexModel<Profile>(Builders<Profile>.IndexKeys.Ascending(x => x.UserName),
                new CreateIndexOptions { Unique = true }));

        MongoClient = mongoClient;
        Profiles = profiles;
        ProfileUpdatedEvents = profileUpdatedEvents;
    }

    private void WatchProfileUpdate(IMongoCollection<ProfileUpdatedEvent> collection)
    {
        var cursor = collection.Watch();

        while (cursor.MoveNext())
        {
            foreach (var change in cursor.Current)
            {
                if (change.OperationType == ChangeStreamOperationType.Insert)
                {
                    _broker.PublishProfileUpdatedEvent(change.FullDocument);
                }
            }
        }
    }
}