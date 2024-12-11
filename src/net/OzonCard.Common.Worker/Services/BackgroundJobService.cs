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
            case "Scheduled":
                return "Scheduled";
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

    public IBackgroundTask Schedule<TResult>(ICommand<TResult> task, DateTimeOffset enqueueAt, Guid? track = null, Guid? user = null)
    {
        var taskId = jobQueue.Schedule<ICommandBus>(
            commands => commands.Send(task, CancellationToken.None),
            enqueueAt
        );
        var jobData = JobStorage.Current.GetConnection().GetJobData(taskId);
        if (track != null && track != Guid.Empty)
            tracking.Observe(task, taskId, (Guid)track, user);
        return new BackgroundTask<TResult>(taskId, jobData.CreatedAt, CastSate(jobData.State));
    }

    public IBackgroundTask Enqueue<TResult>(ICommand<TResult> task, Guid? track = null, Guid? user = null)
    {
        var taskId = jobQueue.Enqueue<ICommandBus>(commands => commands.Send(task, CancellationToken.None));
        var jobData = JobStorage.Current.GetConnection().GetJobData(taskId);
        if (track != null && track != Guid.Empty)
            tracking.Observe(task, taskId, (Guid)track, user);
        return new BackgroundTask<TResult>(taskId, jobData.CreatedAt, CastSate(jobData.State));
    }
    public void Dequeue(string taskId) => jobQueue.Dequeue(taskId);//TODO добавить событие изменение задачи в самом хангфаере 


    public IEnumerable<IBackgroundTask> GetTasks(Guid? user = null, params string[] tasksId)
    {
        var processes = tracking.GetJobsAsync(tasksId, CancellationToken.None).Result;
        var jobs = tasksId.Select(id =>
        {
            var job = JobStorage.Current.GetReadOnlyConnection().GetJobData(id);
            var state = JobStorage.Current.GetReadOnlyConnection().GetStateData(id);
            var jobTracking = processes.FirstOrDefault(p => p.Number == id);
            
            return new BackgroundTask(id, 
                job?.CreatedAt ?? DateTime.Now,
                CastSate(job?.State ?? "Deleted"))
            {
                Progress = jobTracking?.GetJobProgress(),
                Result = jobTracking?.GetJobResult(),
                Error = CastReason(state.Reason),
            };
        });
        return jobs.ToList();
        
    }

    

    public IBackgroundTask? Cancel(string taskId)
    {
        jobQueue.Cancel(taskId);
        //TODO добавить событие изменение задачи в самом хангфаере 
        return GetTasks(null, [taskId]).FirstOrDefault();
    } 
}