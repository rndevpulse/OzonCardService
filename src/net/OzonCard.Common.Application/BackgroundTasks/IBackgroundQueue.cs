using OzonCard.Common.Core;

namespace OzonCard.Common.Application.BackgroundTasks;

public interface IBackgroundQueue
{
    BackgroundTask<TResult> Enqueue<TResult>(ICommand<TResult> command, Guid? reference);
    BackgroundTask<TResult> Enqueue<TResult, TStatus>(ICommand<TResult> command, Guid? reference);
    IEnumerable<BackgroundTasks.BackgroundTask> GetTasks(params Guid[] tasks);
    Task<BackgroundTasks.BackgroundTask?> DequeueAsync();
    void Complete(BackgroundTasks.BackgroundTask task);
    void Cancel(Guid task);
    void UpdateProgress<TStatus>(Guid task, TStatus status);
    // TStatus? GetProgress<TStatus>(Guid task);
}