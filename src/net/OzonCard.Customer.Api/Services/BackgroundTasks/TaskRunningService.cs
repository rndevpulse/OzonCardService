using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace OzonCard.Customer.Api.Services.BackgroundTasks;

public class TaskRunningService : BackgroundService
{
    private readonly IBackgroundQueue _queue;
    private readonly IServiceProvider _provider;

    public TaskRunningService(IBackgroundQueue queue, IServiceProvider provider)
    {
        _queue = queue;
        _provider = provider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var task = await _queue.DequeueAsync();
            if (task != null)
               new Thread(async () => await ExecuteAsync(task))
                   .Start();
        }
    }

    private async Task ExecuteAsync(BackgroundTask task)
    {
        await using var scope = _provider.CreateAsyncScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        await task.ExecuteAsync(mediator);
        _queue.Complete(task);
    }
   
}
