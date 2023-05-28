using MongoDB.Bson;

namespace ProfilerWebAPI.ProfileIO;

/// <summary>
/// This class is exclusively for file validation via <see cref="FileSizeAttribute"/> and <see cref="AllowedExtensionsAttribute"/> attributes
/// </summary>
public class ProfileImageRequest
{
    [FileSize(1024 * 1024 * 10)]
    [AllowedExtensions(new[] { ".jpg", ".png", ".jpeg" })]
    public IFormFile Image { get; set; }
}