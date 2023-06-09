using ProfilerBusiness;
using ProfilerCQRS.Commands;
using ProfilerIntegrations.Abstractions;
using ProfilerWebAPI.Middleware;
using ProfilerWebAPI.Mongo;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((_, lc) =>
    lc.WriteTo.Console(LogEventLevel.Information, theme: AnsiConsoleTheme.Code));

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("ProfilerDB"));

builder.Services.AddHostedService<BackgroundEventManager>();
builder.Services.AddTransient<GlobalErrorHandlerMiddleware>();
builder.Services.AddSingleton<IProfileService, ProfileService>();
builder.Services.AddSingleton<IMongoDBService, MongoDBService>();
builder.Services.AddSingleton<IMessageBroker, MessageBroker>();

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblies(
        Assembly.GetExecutingAssembly(),
        typeof(CreateProfileCommand).Assembly));

var app = builder.Build();

app.UseMiddleware<GlobalErrorHandlerMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();