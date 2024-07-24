
using OzonCard.Common.Worker.Data;

namespace OzonCard.Common.Worker.Services;

public interface ITrackingBackgroundJobs
{
    void Observe(string taskId, Guid track);
    object? GetJobProgress(IJobProgress job);
    Task<IJobProgress?> GetJobAsync(string taskId, CancellationToken ct);
    Task<IJobProgress?> GetJobAsync(Guid track, CancellationToken ct);
    Task<IEnumerable<IJobProgress>> GetJobsAsync(IEnumerable<string> ids, CancellationToken ct);
    void ReportProgress<TProgress>(IJobProgress? job, TProgress progress) where TProgress : NamedProgress<TProgress>, new();
}