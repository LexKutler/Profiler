using System.ComponentModel.DataAnnotations;

namespace ProfilerWebAPI.ProfileIO;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class FileSizeAttribute : ValidationAttribute
{
    private readonly int _maxFileSize;

    public FileSizeAttribute(int maxSize)
    {
        _maxFileSize = maxSize;
    }

    protected override ValidationResult? IsValid(object? value,
        ValidationContext validationContext)
    {
        if (value is not IFormFile file)
        {
            return new ValidationResult("File is required");
        }

        return file.Length < _maxFileSize ?
            ValidationResult.Success :
            new ValidationResult("File is too big");
    }
}