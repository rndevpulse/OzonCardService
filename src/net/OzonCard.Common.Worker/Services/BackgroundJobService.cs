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
            var jobTracking = processes.FirstOrDefault(p => p.TaskId == id);
            return new BackgroundTask(id, job.CreatedAt, CastSate(job.State))
            {
                Progress = jobTracking == null
                    ? null
                    : tracking.GetJobProgress(jobTracking)
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