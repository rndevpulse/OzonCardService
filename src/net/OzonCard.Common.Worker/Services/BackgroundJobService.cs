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

    public IBackgroundTask Schedule<TResult>(ICommand<TResult> task, DateTimeOffset enqueueAt, 
        Guid? track = null,
        Guid? user = null,
        string title = "")
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
                $"[{task.GetType().Name}]{JsonSerializer.Serialize(task, task.GetType())}",
                title
               )
            );

        return new BackgroundTask<TResult>(taskId, DateTimeOffset.UtcNow, "Scheduled"){
            Title = title,
        };
    }

    public IBackgroundTask Enqueue<TResult>(ICommand<TResult> task,  
        Guid? track = null,
        Guid? user = null,
        string title = "")
    {
        var taskId = jobQueue.Enqueue<ICommandBus>(commands => commands.Send(task, CancellationToken.None));
        // var jobData = JobStorage.Current.GetConnection().GetJobData(taskId);
        if (track != null && track != Guid.Empty)
            events.Publish(new OnCreatedJobEvent(
                (Guid)track,
                taskId,
                user,
                "Enqueued",
                $"[{task.GetType().Name}]{JsonSerializer.Serialize(task, task.GetType())}",
                title
                )
            );

        return new BackgroundTask<TResult>(taskId, DateTimeOffset.UtcNow, "Enqueued")
        {
            Title = title,
        };
    }
    public void Dequeue(string taskId) => jobQueue.Dequeue(taskId);

    public async Task<IEnumerable<IBackgroundTask>> GetTasksAsync(Guid? user = null, params string[] tasksId)
    {
        var processes = await store.GetItemsAsync<Job>(x=>
            tasksId.Contains(x.Number) 
            || (x.User != Guid.Empty && x.User == user)
        );
        var from = DateTime.UtcNow.AddMonths(-1);
        var jobs = processes
            .Where(x=>x.Status != "isDeleted")
            .Where(x=>x.Closed == null || x.Closed > from)
            .Select(job =>
           new BackgroundTask(job.Number, 
               job.CreatedAt,
               job.Status)
            {
                Progress = job.GetJobProgress(),
                Result = job.GetJobResult(),
                Error = job.Reason,
                CompletedAt = job.Closed,
                ProcessedAt = job.ProcessedAt,
                Title = job.Title,
            }
        );
        return jobs.ToList();
    }

    

    public IBackgroundTask? Cancel(string taskId)
    {
        jobQueue.Cancel(taskId);
        return GetTasksAsync(null, [taskId]).Result.FirstOrDefault();
    }

    public IBackgroundTask? Remove(string taskId)
    {
        jobQueue.Cancel(taskId);
        var job = store.GetItemAsync<Job>(x=>x.Number == taskId).Result;
        if (job == null) return null;
        
        job.Status = "isDeleted";
        return new BackgroundTask(job.Number, 
            job.CreatedAt,
            job.Status)
        {
            Progress = job.GetJobProgress(),
            Result = job.GetJobResult(),
            Error = job.Reason,
            CompletedAt = job.Closed,
            ProcessedAt = job.ProcessedAt,
            Title = job.Title,
        };
    }
}