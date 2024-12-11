using MediatR;
using Microsoft.Extensions.Logging;

namespace OzonCard.Common.Infrastructure.Pipelines;

public class EventTransactionPipeline(
    IEventTransactionManager transactions,
    ILogger<EventTransactionPipeline> logger
) : INotificationPublisher
{
    public async Task Publish(IEnumerable<NotificationHandlerExecutor> handlerExecutors, INotification notification,
        CancellationToken cancellationToken)
    {
        try
        {
            var trx = transactions.StartTransaction();
            var tasks = handlerExecutors
                .Select(handler => handler.HandlerCallback(
                    notification,
                    cancellationToken))
                .ToArray();
            await Task.WhenAll(tasks).ContinueWith(async _ =>
            {
                if (trx)
                    await transactions.CommitAsync(cancellationToken);
            }, cancellationToken);

        }
        catch (Exception e)
        {
            logger.LogWarning(e, "Error while executing transaction events scope");
            throw;
        }
    }
}
