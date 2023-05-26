using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using ProfilerCQRS.Commands;
using ProfilerWebAPI.MappingProfiles;
using ProfilerModels.Abstractions;
using ProfilerWebAPI.Mongo;
using ProfilerBusiness;

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