using OzonCard.Common.Core;
using OzonCard.Common.Worker.Domain.Jobs;
using OzonCard.Common.Worker.Stores;

namespace OzonCard.Common.Worker.Jobs.Events;

public record OnCreatedJobEvent<TResult>(
    Guid Aggregate,
    string Number,
    Guid? User,
    ICommand<TResult> Job
) : IEvent
{
    
    public class Handler(
        IStoreContext store
    ) : IEventHandler<OnCreatedJobEvent<TResult>>
    {
        public Task Handle(OnCreatedJobEvent<TResult> notification, CancellationToken cancellationToken)
        {
            var job = new Job(
                notification.Aggregate,
                notification.Number,
                notification.User ?? Guid.Empty
            );
            store.Append<Job>(job);
            return Task.CompletedTask;
        }
    }
}