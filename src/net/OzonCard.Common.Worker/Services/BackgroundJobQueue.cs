using System.Linq.Expressions;
using Hangfire;

namespace OzonCard.Common.Worker.Services;

internal class BackgroundJobQueue(
    IBackgroundJobClient client,
    IRecurringJobManager recurring
) : IBackgroundJobQueue
{

    public void AppendSchedule<T>(string jobId, Expression<Func<T, Task>> job, string schedule, string queue = "default")=>
        recurring.AddOrUpdate(jobId, queue, job, schedule);

    
    public string Enqueue<T>(Expression<Func<T, Task>> job) =>
        client.Enqueue(job);

    public void Dequeue(string jobId) =>
        recurring.RemoveIfExists(jobId);

    public void Cancel(string taskId)
    {
        client.Delete(taskId);
    }

    public string ContinueWith<T>(string taskId, Expression<Func<T, Task>> job) =>
        client.ContinueJobWith(taskId, job);
}