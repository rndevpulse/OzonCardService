using OzonCard.Common.Core;

namespace OzonCard.Customer.Api.Services.BackgroundTasks;

public interface IBackgroundQueue
{
    BackgroundTask<TResult> Enqueue<TResult>(ICommand<TResult> command, Guid? reference);
    BackgroundTask<TResult> Enqueue<TResult, TStatus>(ICommand<TResult> command, Guid? reference);
    IEnumerable<BackgroundTask> GetTasks(params Guid[] tasks);
    internal Task<BackgroundTask?> DequeueAsync();
    internal void Complete(BackgroundTask task);
    void Cancel(Guid task);
    void UpdateProgress<TStatus>(Guid task, TStatus status);
    TStatus? GetProgress<TStatus>(Guid task);
}