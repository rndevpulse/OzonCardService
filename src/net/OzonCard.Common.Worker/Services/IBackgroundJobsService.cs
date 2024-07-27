using OzonCard.Common.Core;

namespace OzonCard.Common.Worker.Services;

public interface IBackgroundJobsService
{
    IBackgroundTask AppendSchedule<TResult>(string taskId, ICommand<TResult> task, string schedule = "*/10 * * * *", string queue = "default");
    IBackgroundTask Enqueue<TResult>(ICommand<TResult> task, Guid? track = null);
    void Dequeue(string taskId);
    IEnumerable<IBackgroundTask> GetTasks(params string[] tasksId);
    IBackgroundTask? Cancel(string taskId);
}