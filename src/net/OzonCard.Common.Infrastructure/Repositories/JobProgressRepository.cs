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
    public async Task<IJobProgress> AddAsync(string taskId, Guid reference, string path, CancellationToken ct)
    {
        var job = new JobProgress
        {
            TaskId = taskId,
            Reference = reference,
            Path = path
        };
        await context.Set<JobProgress>().AddAsync(job, ct);
        return job;
    }

    public async Task<IJobProgress?> FindJobAsync(Guid reference, CancellationToken ct)
    {
        return await context.Set<JobProgress>().FirstOrDefaultAsync(x=>x.Reference == reference, ct);
    }

    public async Task<IEnumerable<IJobProgress>> GetJobsAsync(IEnumerable<string> taskId, CancellationToken ct)
    {
        return await context.Set<JobProgress>().Where(x=>taskId.Contains(x.TaskId)).ToListAsync(ct);

    }
}