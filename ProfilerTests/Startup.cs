using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using ProfilerWebAPI.MappingProfiles;

namespace TestsProfiler;

public static class Startup
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ProfilesMappingRule).Assembly);
    }
}