using System.ComponentModel.DataAnnotations;

namespace ProfilerWebAPI.ProfileIO;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class AllowedExtensions : ValidationAttribute
{
    private readonly string[] _extensions;

    public AllowedExtensions(string[] extensions)
    {
        _extensions = extensions;
    }

    public override bool IsValid(object value)
    {
        if (value is not IFormFile file) return false;
        var extension = Path.GetExtension(file.FileName);
        return _extensions.Contains(extension.ToLowerInvariant());
    }
}
