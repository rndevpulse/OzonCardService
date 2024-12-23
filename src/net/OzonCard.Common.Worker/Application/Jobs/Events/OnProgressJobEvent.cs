using System.Text.Json;
using OzonCard.Common.Core;
using OzonCard.Common.Worker.Data;
using OzonCard.Common.Worker.Domain.Jobs;

namespace OzonCard.Common.Worker.Application.Jobs.Events;

public record OnProgressJobEvent(
    Guid Aggregate,
    NamedProgress Progress,
    object? Result
) : IEvent
{
    public class Handler(IStoreContext store) : IEventHandler<OnProgressJobEvent>
    {
        public async Task Handle(OnProgressJobEvent notification, CancellationToken cancellationToken)
        {
            var job = await store.GetItemAsync<Job>(notification.Aggregate, cancellationToken);
            if (job == null)
                return;
            job.Progress = JsonSerializer.Serialize(notification.Progress, notification.Progress.GetType());
            if (notification.Result != null)
                job.Result = JsonSerializer.Serialize(notification.Result, notification.Result.GetType());
            store.Update(job);

        }
    }
    
}