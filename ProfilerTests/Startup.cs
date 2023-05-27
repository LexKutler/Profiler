using Microsoft.Extensions.DependencyInjection;
using ProfilerBusiness;
using ProfilerCQRS.Commands;
using ProfilerIntegrations.Abstractions;
using ProfilerWebAPI.MappingProfiles;
using ProfilerWebAPI.Mongo;
using System.Reflection;

namespace TestsProfiler;

public static class Startup
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ProfilesMappingRule).Assembly);
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssemblies(
                Assembly.GetExecutingAssembly(),
                typeof(CreateProfileCommand).Assembly));
        services.AddSingleton<IMongoDBService, MongoDBService>();
        services.AddScoped<IMessageBroker, MessageBroker>();
    }
}