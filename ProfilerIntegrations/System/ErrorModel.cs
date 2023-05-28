using System.Text.Json;

namespace ProfilerIntegrations.System;

/// <summary>
/// Error model to output from error handler
/// </summary>
public class ErrorModel
{
    public int StatusCode { get; set; }
    public string Message { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}