using System.Text.Json;
using Hangfire;
using OzonCard.Common.Core;
using OzonCard.Common.Worker.Application.Jobs.Events;
using OzonCard.Common.Worker.Data;
using OzonCard.Common.Worker.Domain.Jobs;
using OzonCard.Common.Worker.Stores;

namespace OzonCard.Common.Worker.Services;

internal class BackgroundJobService(
    IBackgroundJobQueue jobQueue,
    IEventBus events,
    IStoreContext store
) : IBackgroundJobsService
{
   
   
    
    
    public IBackgroundTask AppendSchedule<TResult>(
        string taskId, 
        ICommand<TResult> task, 
        string schedule = "*/10 * * * *", 
        string queue = "default")
    {
        jobQueue.AppendSchedule<ICommandBus>(
            taskId, 
            commands => commands.Send(task, CancellationToken.None), 
            schedule,
            queue);
        var jobData = JobStorage.Current.GetConnection().GetJobData(taskId);
        return new BackgroundTask<TResult>(taskId, 
            jobData?.CreatedAt ?? DateTime.Now, 
            "Enqueued");
    }

    public IBackgroundTask Schedule<TResult>(ICommand<TResult> task, DateTimeOffset enqueueAt, Guid? track = null, Guid? user = null)
    {
        var taskId = jobQueue.Schedule<ICommandBus>(
            commands => commands.Send(task, CancellationToken.None),
            enqueueAt
        );
        // var jobData = JobStorage.Current.GetConnection().GetJobData(taskId);
        if (track != null && track != Guid.Empty)
            events.Publish(new OnCreatedJobEvent(
                (Guid)track,
                taskId,
                user,
                "Scheduled",
                $"[{task.GetType().Name}]{JsonSerializer.Serialize(task, task.GetType())}"
               )
            );

        return new BackgroundTask<TResult>(taskId, DateTimeOffset.UtcNow, "Scheduled");
    }

    public IBackgroundTask Enqueue<TResult>(ICommand<TResult> task, Guid? track = null, Guid? user = null)
    {
        var taskId = jobQueue.Enqueue<ICommandBus>(commands => commands.Send(task, CancellationToken.None));
        // var jobData = JobStorage.Current.GetConnection().GetJobData(taskId);
        if (track != null && track != Guid.Empty)
            events.Publish(new OnCreatedJobEvent(
                (Guid)track,
                taskId,
                user,
                "Enqueued",
                $"[{task.GetType().Name}]{JsonSerializer.Serialize(task, task.GetType())}"
                )
            );

        return new BackgroundTask<TResult>(taskId, DateTimeOffset.UtcNow, "Enqueued");
    }
    public void Dequeue(string taskId) => jobQueue.Dequeue(taskId);

    public async Task<IEnumerable<IBackgroundTask>> GetTasksAsync(Guid? user = null, params string[] tasksId)
    {
        var processes = await store.GetItemsAsync<Job>(x=>
            tasksId.Contains(x.Number) 
            || (x.User == user && x.User != Guid.Empty)
        );
        var jobs = tasksId.Select(id =>
        {
            // var job = JobStorage.Current.GetReadOnlyConnection().GetJobData(id);
            // var state = JobStorage.Current.GetReadOnlyConnection().GetStateData(id);
            var jobTracking = processes.FirstOrDefault(p => p.Number == id);
            
            return new BackgroundTask(id, 
                jobTracking?.CreatedAt ?? DateTime.Now,
                jobTracking?.Status ?? "Deleted")
            {
                Progress = jobTracking?.GetJobProgress(),
                Result = jobTracking?.GetJobResult(),
                Error = jobTracking?.Reason,
            };
        });
        return jobs.ToList();
        
    }

    

    public IBackgroundTask? Cancel(string taskId)
    {
        jobQueue.Cancel(taskId);
        return GetTasksAsync(null, [taskId]).Result.FirstOrDefault();
    } 
}