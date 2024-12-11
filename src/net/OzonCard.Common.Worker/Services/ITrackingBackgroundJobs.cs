
using OzonCard.Common.Core;
using OzonCard.Common.Worker.Data;
using OzonCard.Common.Worker.Domain.Jobs;

namespace OzonCard.Common.Worker.Services;

public interface ITrackingBackgroundJobs
{
    void Observe<TResult>(ICommand<TResult> task, string taskId, Guid track, Guid? user);
    Task<Job?> GetJobAsync(Guid track, CancellationToken ct);
    Task<IEnumerable<Job>> GetJobsAsync(IEnumerable<string> ids, CancellationToken ct);
    void ReportProgress<TProgress>(Guid? track, TProgress progress, object? result = null) where TProgress : NamedProgress, new();
}