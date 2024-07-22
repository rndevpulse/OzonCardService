
using OzonCard.Common.Worker.Data;

namespace OzonCard.Common.Worker.Services;

internal interface ITrackingBackgroundJobs
{
    Task<TStatus> CreateAsync<TStatus>(string taskId, Guid reference, CancellationToken ct) where TStatus: class, new();
    object? GetJobProgress(IJobProgress job);
    Task<IJobProgress?> GetJobAsync(Guid reference, CancellationToken ct);
    Task<IEnumerable<IJobProgress>> GetJobsAsync(IEnumerable<string> ids, CancellationToken ct);
    void ReportProgress<TProgress>(IJobProgress job, TProgress progress);
}