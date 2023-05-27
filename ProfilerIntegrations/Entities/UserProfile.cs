using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProfilerIntegrations.Entities;

public class UserProfile
{
    [BsonId]
    public ObjectId Id { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public int Zip { get; set; }
    public int CountryCode { get; set; }
    public long? TimeStamp { get; set; }
    public string? PicturePath { get; set; }
}