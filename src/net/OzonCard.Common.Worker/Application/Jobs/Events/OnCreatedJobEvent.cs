using OzonCard.Common.Core;
using OzonCard.Common.Worker.Domain.Jobs;
using OzonCard.Common.Worker.Stores;

namespace OzonCard.Common.Worker.Application.Jobs.Events;

public record OnCreatedJobEvent(
    Guid Aggregate,
    string Number,
    Guid? User,
    string Status,
    string? Arguments,
    string? Title = null
) : IEvent
{
    
    public class Handler(IStoreContext store) : IEventHandler<OnCreatedJobEvent>
    {
        public Task Handle(OnCreatedJobEvent notification, CancellationToken cancellationToken)
        {
            var job = new Job(
                notification.Aggregate,
                notification.Number,
                notification.User ?? Guid.Empty,
                notification.Status,
                notification.Arguments ?? string.Empty
            )
            {
                Title = notification.Title
            };
            store.Append<Job>(job);
            store.Dispose();
            return Task.CompletedTask;
        }
    }
}