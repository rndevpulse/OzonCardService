using System.Collections.Concurrent;
using Microsoft.Extensions.Caching.Memory;
using OzonCard.Common.Application.BackgroundTasks;
using OzonCard.Common.Core;

namespace OzonCard.Common.Infrastructure.BackgroundTasks;

public class BackgroundQueue(IMemoryCache cache) : IBackgroundQueue
{
    private readonly MemoryCacheEntryOptions _cacheOptions = new MemoryCacheEntryOptions()
        .SetSlidingExpiration(TimeSpan.FromMinutes(5));
    private readonly ConcurrentQueue<BackgroundTask> _queue = new ();
    private readonly SemaphoreSlim _semaphore = new (0);
    
    
    private string GetKey(Guid id) => $"task_{id}";
    
    

    public BackgroundTask<TResult> Enqueue<TResult>(ICommand<TResult> command, Guid? reference)
    {
        var task = new BackgroundTask<TResult>(command, reference);
        _queue.Enqueue(task);
        _semaphore.Release();
        return task;
    }

    public BackgroundTask<TResult> Enqueue<TResult, TStatus>(ICommand<TResult> command, Guid? reference)
    {
        var task = new BackgroundTask<TResult, TStatus>(command, reference);
        _queue.Enqueue(task);
        _semaphore.Release();
        return task;
    }

    public BackgroundTask? Cancel(Guid id)
    {
        if (cache.Get<BackgroundTask>(GetKey(id)) is not { } task) 
            return null;
        task.Cancel();
        return task;
    }

    public void UpdateProgress<TStatus>(Guid id, TStatus status)
    {
        if (cache.Get<BackgroundTask>(GetKey(id)) is not IProgress<TStatus> task) 
            return;
        task.Report(status);
        cache.Set(GetKey(id), task, _cacheOptions);

    }

    // public TStatus? GetProgress<TStatus>(Guid id)
    // {
    //     if (cache.Get<BackgroundTask>(GetKey(id)) is IProgress<TStatus> task)
    //         return task.GetProgress();
    //     return default;
    // }

    async Task<BackgroundTask?> IBackgroundQueue.DequeueAsync()
    {
        await _semaphore.WaitAsync();
        return _queue.TryDequeue(out var task) 
            ? cache.Set(GetKey(task.Reference ?? task.Id), task, _cacheOptions)
            : null;
    }

    void IBackgroundQueue.Complete(BackgroundTask task) =>
        cache.Set(GetKey(task.Reference ?? task.Id), task, _cacheOptions);

    
    
    public IEnumerable<BackgroundTask> GetTasks(params Guid[] tasks)
    {
        var processed = new List<BackgroundTask>();
        foreach (var id in tasks)
            if (cache.Get<BackgroundTask>(GetKey(id)) is {} task)
                processed.Add(task);
        processed.AddRange(_queue.Where(x => tasks.Contains(x.Id)));
        return processed;
    }
}