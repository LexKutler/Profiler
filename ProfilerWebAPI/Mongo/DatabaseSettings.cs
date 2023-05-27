namespace ProfilerWebAPI.Mongo;

/// <summary>
/// Class repeats appsettings database section
/// </summary>
public class DatabaseSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;
}