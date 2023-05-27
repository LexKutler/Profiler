using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProfilerModels.Abstractions;
using Serilog;

namespace ProfilerBusiness;
/// <summary>
/// Class is derived from <see cref="BackgroundService"/>.
/// It is used as a recurring manager to <see cref="IMessageBroker"/>.
/// </summary>
public class BackgroundEventManager : BackgroundService
{
    private readonly IServiceProvider _services;
    public BackgroundEventManager(IServiceProvider services)
    {
        _services = services;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _services.CreateScope();

        var messageBroker = scope.ServiceProvider
                .GetRequiredService<IMessageBroker>();

        while (!stoppingToken.IsCancellationRequested)
        {
            var listOfEvents = await messageBroker.PublishProfileUpdatedEvent();

            if (listOfEvents.Any())
            {
                foreach (var profileUpdatedEvent in listOfEvents)
                {
                    Log.Information(
                        $"Update event for {profileUpdatedEvent.UserProfileAfter.Id} happened");
                }
                Log.Information($"Outbox checked for {listOfEvents.Count} events");
            }
            else
            {
                Log.Information("Outbox checked. No events found so far");
            }

            await Task.Delay(10000, stoppingToken);
        }
    }
    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        Log.Information("Outbox checker stopped");

        await base.StopAsync(stoppingToken);
    }
}
