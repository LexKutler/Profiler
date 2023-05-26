using System.ComponentModel.DataAnnotations;

namespace ProfilerWebAPI.ProfileIO;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class FileSize : ValidationAttribute
{
    private readonly int _maxFileSize;

    public FileSize(int maxSize)
    {
        _maxFileSize = maxSize;
    }

    public override bool IsValid(object value)
    {
        if (value is not IFormFile file) return false;
        return file.Length > _maxFileSize;
    }
}