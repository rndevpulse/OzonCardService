using OzonCard.Common.Worker.Data;

namespace OzonCard.Common.Worker.JobsProgress;

public interface IJobProgressRepository
{
    IJobProgress Add(string taskId, Guid track, string path);
    Task<IJobProgress?> GetItemAsync(string taskId, CancellationToken ct);
    Task<IJobProgress?> GetItemAsync(Guid track, CancellationToken ct);
    Task<IEnumerable<IJobProgress>> GetItemsAsync(IEnumerable<string> taskId, CancellationToken ct);
}