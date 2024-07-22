using System.Linq.Expressions;

namespace OzonCard.Common.Worker.Services;

internal interface IBackgroundJobQueue
{
    void AppendSchedule<T>(string jobId, Expression<Func<T, Task>> job, string schedule);
    string Enqueue<T>(Expression<Func<T, Task>> job);
}