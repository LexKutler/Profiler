using System.ComponentModel.DataAnnotations;

namespace ProfilerWebAPI.ProfileIO;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class AllowedExtensionsAttribute : ValidationAttribute
{
    private readonly string[] _extensions;

    public AllowedExtensionsAttribute(string[] extensions)
    {
        _extensions = extensions;
    }

    protected override ValidationResult? IsValid(object? value,
        ValidationContext validationContext)
    {
        if (value is not IFormFile file)
        {
            return new ValidationResult("File is required");
        }

        var extension = Path.GetExtension(file.FileName);

        return _extensions.Contains(extension.ToLowerInvariant()) ?
            ValidationResult.Success :
            new ValidationResult("File is not supported");
    }
}