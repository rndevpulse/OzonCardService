using System.Linq.Expressions;
using OzonCard.Common.Core;

namespace OzonCard.Common.Worker.Services;

internal interface IBackgroundJobQueue
{
    void AppendSchedule<T>(string jobId, Expression<Func<T, Task>> job, string schedule, string queue = "default");
    string Enqueue<T>(Expression<Func<T, Task>> job);

    void Dequeue(string jobId);
    void Cancel(string taskId);
}