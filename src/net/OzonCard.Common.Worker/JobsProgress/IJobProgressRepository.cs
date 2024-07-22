using OzonCard.Common.Worker.Data;

namespace OzonCard.Common.Worker.JobsProgress;

public interface IJobProgressRepository
{
    Task<IJobProgress> AddAsync(string taskId, Guid reference, string path, CancellationToken ct);
    Task<IJobProgress?> FindJobAsync(Guid reference, CancellationToken ct);
    Task<IEnumerable<IJobProgress>> GetJobsAsync(IEnumerable<string> taskId, CancellationToken ct);
}