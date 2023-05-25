using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProfilerModels;

public class Profile
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
}