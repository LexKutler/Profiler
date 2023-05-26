using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProfilerModels.Abstractions;
using Serilog;

namespace ProfilerBusiness;
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
            await messageBroker.PublishProfileUpdatedEvent();

            Log.Information("Outbox checked");

            await Task.Delay(10000, stoppingToken);
        }
    }
    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        Log.Information("Outbox checker stopped");

        await base.StopAsync(stoppingToken);
    }
}
