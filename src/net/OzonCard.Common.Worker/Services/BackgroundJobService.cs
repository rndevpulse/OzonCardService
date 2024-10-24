using Hangfire;
using OzonCard.Common.Core;
using OzonCard.Common.Worker.Data;

namespace OzonCard.Common.Worker.Services;

internal class BackgroundJobService(
    IBackgroundJobQueue jobQueue,
    ITrackingBackgroundJobs tracking
) : IBackgroundJobsService
{
    private string CastSate(string? state)
    {
        switch (state)
        {
            case "Succeeded":
                return "Completed";
            case "Deleted":
            case "Failed":
                return "Failed";
            default: 
                return "Running";
        }
    }
    private string? CastReason(string reason)
    {
        if (string.IsNullOrEmpty(reason)) 
            return null;
        if (reason.Contains("Retry attempt"))
            return reason.Replace("Retry attempt", "Попытка").Replace(" of ", "/");
        return "Не удалось выполнить задачу, попробуйте позже";
    }
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
            CastSate(jobData?.State));
    }



    public IBackgroundTask Enqueue<TResult>(ICommand<TResult> task, Guid? track = null)
    {
        var taskId = jobQueue.Enqueue<ICommandBus>(commands => commands.Send(task, CancellationToken.None));
        var jobData = JobStorage.Current.GetConnection().GetJobData(taskId);
        if (track != null && track != Guid.Empty)
            tracking.Observe(taskId, (Guid)track);
        return new BackgroundTask<TResult>(taskId, jobData.CreatedAt, CastSate(jobData.State));
    }
    public void Dequeue(string taskId) => jobQueue.Dequeue(taskId);


    public IEnumerable<IBackgroundTask> GetTasks(params string[] tasksId)
    {
        var processes = tracking.GetJobsAsync(tasksId, CancellationToken.None).Result;
        var jobs = tasksId.Select(id =>
        {
            var job = JobStorage.Current.GetReadOnlyConnection().GetJobData(id);
            var state = JobStorage.Current.GetReadOnlyConnection().GetStateData(id);
            var jobTracking = processes.FirstOrDefault(p => p.TaskId == id);
            var jobProgress = jobTracking == null
                ? null
                : tracking.GetJobProgress(jobTracking);
            return new BackgroundTask(id, 
                job?.CreatedAt ?? DateTime.Now,
                CastSate(job?.State ?? "Deleted"))
            {
                Progress = jobProgress?.Status,
                Result = jobProgress?.Result,
                Error = CastReason(state.Reason),
            };
        });
        return jobs.ToList();
        
    }

    

    public IBackgroundTask? Cancel(string taskId)
    {
        jobQueue.Cancel(taskId);
        return GetTasks([taskId]).FirstOrDefault();
    } 
}