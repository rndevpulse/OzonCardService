using Microsoft.EntityFrameworkCore;
using OzonCard.Common.Domain.JobProgresses;
using OzonCard.Common.Infrastructure.Database;
using OzonCard.Common.Worker.Data;
using OzonCard.Common.Worker.JobsProgress;

namespace OzonCard.Common.Infrastructure.Repositories;

public class JobProgressRepository(
    InfrastructureContext context
) : IJobProgressRepository
{
    public IJobProgress Add(string taskId, Guid track, string path)
    {
        var job = new JobProgress
        {
            TaskId = taskId,
            Track = track,
            Path = path,
            CreatedAt = DateTimeOffset.Now
        };
        context.Set<JobProgress>().Add(job);
        context.SaveChanges();
        return job;
        
    }

    public async Task<IJobProgress?> GetItemAsync(string taskId, CancellationToken ct)
    {
        return await context.Set<JobProgress>().FirstOrDefaultAsync(x=>x.TaskId == taskId, ct);
    }
    public async Task<IJobProgress?> GetItemAsync(Guid track, CancellationToken ct)
    {
        return await context.Set<JobProgress>().FirstOrDefaultAsync(x=>x.Track == track, ct);
    }

    public async Task<IEnumerable<IJobProgress>> GetItemsAsync(IEnumerable<string> taskId, CancellationToken ct)
    {
        return await context.Set<JobProgress>().Where(x=>taskId.Contains(x.TaskId)).ToListAsync(ct);

    }
}