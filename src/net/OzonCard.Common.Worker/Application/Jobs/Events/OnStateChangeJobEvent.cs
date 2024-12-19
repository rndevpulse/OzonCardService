using Microsoft.Extensions.Logging;
using OzonCard.Common.Core;
using OzonCard.Common.Worker.Domain.Jobs;
using OzonCard.Common.Worker.Stores;

namespace OzonCard.Common.Worker.Application.Jobs.Events;

public record OnStateChangeJobEvent(
    Guid Aggregate,
    string Number,
    string State,
    string Reason,
    bool IsFinal
) : IEvent
{
    
    public class Handler(IStoreContext store, ILogger<OnStateChangeJobEvent> logger) : IEventHandler<OnStateChangeJobEvent>
    {
        public async Task Handle(OnStateChangeJobEvent notification, CancellationToken cancellationToken)
        {
            logger.LogInformation($"jobId '{notification.Number}' state '{notification.State}'");
            var job = await store.GetItemAsync<Job>(x=>x.Number == notification.Number, cancellationToken);
            
            if (job == null)
                return;
            // Processing
            // Awaiting
            
            // Scheduled
            // Succeeded
            // Enqueued
            // Deleted
            // Failed
            logger.LogInformation($"update job with number '{notification.Number}'");
            var newState = CastSate(notification.State);
            if (job.Status != "Processing" && newState == "Processing")
                job.ProcessedAt = DateTimeOffset.UtcNow;
            
            job.Status = newState;
            
            job.Reason = CastReason(notification.Reason);
            if (notification.IsFinal)
                job.Closed = DateTime.UtcNow;
            store.Dispose();
        }


        private string CastSate(string state) =>
            state switch
            {
                "Succeeded" => state,
                "Deleted" => state,
                "Failed" => state,
                "Scheduled" => state,
                _ => "Processing"
                
            };
        
        private string? CastReason(string reason)
        {
            if (string.IsNullOrEmpty(reason)) 
                return null;
            if (reason.Contains("Retry attempt"))
                return reason.Replace("Retry attempt", "Попытка").Replace(" of ", "/");
            return "Не удалось выполнить задачу, попробуйте позже";
        }
    }
}