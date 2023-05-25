using MongoDB.Bson;

namespace ProfilerWebAPI.ProfileIO;
public class ProfileImageRequest
{
    public ObjectId ProfileId { get; set; }

    [FileSize(1024 * 1024 * 10)]
    [AllowedExtensions(new[] { ".jpg", ".png", ".jpeg" })]
    public IFormFile Image { get; set; }
}
