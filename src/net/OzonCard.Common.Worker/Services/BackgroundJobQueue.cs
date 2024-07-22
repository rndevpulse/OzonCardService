using System.Linq.Expressions;
using Hangfire;

namespace OzonCard.Common.Worker.Services;

internal class BackgroundJobQueue(
    IBackgroundJobClient client,
    IRecurringJobManager recurring
) : IBackgroundJobQueue
{
    public void AppendSchedule<T>(string jobId, Expression<Func<T, Task>> job, string schedule) =>
        recurring.AddOrUpdate(jobId, job, schedule);

    public string Enqueue<T>(Expression<Func<T, Task>> job) =>
        client.Enqueue(job);
}