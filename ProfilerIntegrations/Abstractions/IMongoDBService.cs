using MongoDB.Driver;
using ProfilerIntegrations.Entities;

namespace ProfilerIntegrations.Abstractions;

/// <summary>
/// Service provide access for MongoDB client in use.
/// See <a href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/recommended-tags">Microsoft documentation example</a>
/// </summary>
public interface IMongoDBService
{
    MongoClient MongoClient { get; }
    IMongoCollection<UserProfile> Profiles { get; }
    IMongoCollection<ProfileUpdatedEvent> ProfileUpdatedEvents { get; }
}