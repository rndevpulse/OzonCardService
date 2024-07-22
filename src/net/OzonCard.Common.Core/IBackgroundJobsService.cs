namespace OzonCard.Common.Core;

public interface IBackgroundJobsService
{
    void AppendSchedule<TResult>(string taskId, ICommand<TResult> task, string schedule = "*/10 * * * *");
    IBackgroundTask Enqueue<TResult>(ICommand<TResult> task);
    Task<IBackgroundTask> Enqueue<TResult, TStatus>(ICommand<TResult> task, Guid reference) where TStatus : class, new();
    IEnumerable<IBackgroundTask> GetTasks(params string[] tasksId);
}