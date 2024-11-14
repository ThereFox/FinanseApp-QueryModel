using Application.Events.Abstractions;
using Application.Events.Realisation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.StateUpdator.EventReader.Service;

public class EventReaderService : IHostedService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public EventReaderService(
        IServiceScopeFactory factory
        )
    {
        _scopeFactory = factory;
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.Run(async () => await readLoop(cancellationToken));
    }

    private async Task readLoop(CancellationToken cancellationToken)
    {
        while (cancellationToken.IsCancellationRequested == false)
        {
            await readWhileCan<ClientCreatedEvent>();
            await readWhileCan<BillCreatedEvent>();
            await readWhileCan<BillAmountChangeEvent>();
        }
        
    }

    private async Task readWhileCan<T>() where T : IDBStateChangeEvent
    {
        var scope = _scopeFactory.CreateAsyncScope();

        var updator = scope.ServiceProvider.GetService<StateUpdator<T>>();
        
        while (true)
        {
            var handleResult = await updator.HandleEvent();

            if (handleResult.IsFailure)
            {
                break;
            }
        }
        
        await scope.DisposeAsync();
    }
    
    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}