using OzonCard.Common.Core;
using OzonCard.Common.Worker.Application.Jobs.Events;
using OzonCard.Common.Worker.Data;

namespace OzonCard.Common.Application.Common;

public abstract class BaseCommandHandlerProgress(IEventBus events)
{
    protected void ReportProgress<TProgress>(
        Guid? job,
        TProgress progress,
        object? result = null) where TProgress : NamedProgress
    {
        if (job == null || job == Guid.Empty)
            return;
        events.Publish(new OnProgressJobEvent((Guid)job, progress, result));
    }
}