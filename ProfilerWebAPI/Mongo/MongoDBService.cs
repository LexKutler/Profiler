using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ProfilerIntegrations.Abstractions;
using ProfilerIntegrations.Entities;
using Serilog;

namespace ProfilerWebAPI.Mongo;

/// <summary>
/// Implementation of <see cref="IMongoDBService"/>
/// </summary>
public class MongoDBService : IMongoDBService
{
    public MongoClient MongoClient { get; }
    public IMongoCollection<UserProfile> Profiles { get; }
    public IMongoCollection<ProfileUpdatedEvent> ProfileUpdatedEvents { get; }
    private readonly IMessageBroker _broker;

    public MongoDBService(IOptions<DatabaseSettings> databaseSettings, IMessageBroker broker)
    {
        _broker = broker;

        var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
        var database = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);

        try
        {
            database.CreateCollection("profiles");
            database.CreateCollection("profileUpdatedEvents");
        }
        catch (MongoCommandException)
        {
            Log.Information("Collections present");
        }

        var profiles = database.GetCollection<UserProfile>("profiles");
        var profileUpdatedEvents = database.GetCollection<ProfileUpdatedEvent>("profileUpdatedEvents");

        // This would ensure that the UserName field is unique
        profiles.Indexes.CreateOne(
            new CreateIndexModel<UserProfile>(
                Builders<UserProfile>.IndexKeys.Ascending(x => x.UserName),
                new CreateIndexOptions { Unique = true }));

        MongoClient = mongoClient;
        Profiles = profiles;
        ProfileUpdatedEvents = profileUpdatedEvents;
    }
}