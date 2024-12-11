using OzonCard.Common.Core;

namespace OzonCard.Common.Worker.Services;

public interface IBackgroundJobsService
{
    IBackgroundTask AppendSchedule<TResult>(string taskId, ICommand<TResult> task, string schedule = "*/10 * * * *", string queue = "default");
    IBackgroundTask Schedule<TResult>(ICommand<TResult> task, DateTimeOffset enqueueAt, Guid? track = null, Guid? user = null);
    IBackgroundTask Enqueue<TResult>(ICommand<TResult> task, Guid? track = null, Guid? user = null);
    void Dequeue(string taskId); 
    Task<IEnumerable<IBackgroundTask>> GetTasksAsync(Guid? user = null, params string[] tasksId);
    IBackgroundTask? Cancel(string taskId);
}