using System.Text.Json;
using OzonCard.Common.Core;
using OzonCard.Common.Worker.Data;
using OzonCard.Common.Worker.Domain.Jobs;
using OzonCard.Common.Worker.Stores;

namespace OzonCard.Common.Worker.Application.Jobs.Events;

public record OnProgressJobEvent(
    Guid Aggregate,
    NamedProgress Progress,
    object? Result
) : IEvent
{
    public class Handler(IStoreContext context) : IEventHandler<OnProgressJobEvent>
    {
        public async Task Handle(OnProgressJobEvent notification, CancellationToken cancellationToken)
        {
            var job = await context.GetItemAsync<Job>(notification.Aggregate, cancellationToken);
            if (job == null)
                return;
            job.Progress = JsonSerializer.Serialize(notification.Progress);
            job.Result = JsonSerializer.Serialize(notification.Result);
        }
    }
    
}