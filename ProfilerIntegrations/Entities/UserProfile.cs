using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProfilerIntegrations.Entities;

/// <summary>
/// Class that encapsulates document structure as it is stored in database
/// It is meant to never be exposed to end-user due to its <see cref="TimeStamp">TimeStamp</see>
/// &amp; <see cref="PicturePath">PicturePath</see> properties
/// </summary>
public class UserProfile
{
    [BsonId]
    public ObjectId Id { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }

    /// <summary>
    /// Unique field by DB configuration
    /// </summary>
    public string UserName { get; set; }

    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public int Zip { get; set; }
    public int CountryCode { get; set; }

    /// <summary>
    /// Used for optimistic locking. Meant to be <see langword="long"/> representation of <see langword="UtcNow"/>
    /// </summary>
    public long? TimeStamp { get; set; }

    /// <summary>
    /// Updates only from endpoint. Can bare information about server infrastructure and file organization
    /// </summary>
    public string? PicturePath { get; set; }
}