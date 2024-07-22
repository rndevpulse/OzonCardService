
using Hangfire;
using OzonCard.Common.Core;
using OzonCard.Common.Worker.Data;

namespace OzonCard.Common.Worker.Services;

internal class BackgroundJobService(
    IBackgroundJobQueue queue,
    ITrackingBackgroundJobs tracking
) : IBackgroundJobsService
{

    public void AppendSchedule<TResult>(string taskId, ICommand<TResult> task, string schedule = "*/10 * * * *")
    {
        queue.AppendSchedule<ICommandBus>(
            taskId, 
            commands => commands.Send(task, CancellationToken.None), 
            schedule);
    }



    public IBackgroundTask Enqueue<TResult>(ICommand<TResult> task)
    {
        var taskId = queue.Enqueue<ICommandBus>(commands => commands.Send(task, CancellationToken.None));
        var jobData = JobStorage.Current.GetConnection().GetJobData(taskId);
        return new BackgroundTask<TResult>(taskId, jobData.CreatedAt, jobData.State);
        // return tracking.Append(reference?.ToString() ?? id);
    }

    public async Task<IBackgroundTask> Enqueue<TResult, TStatus>(ICommand<TResult> task, Guid reference) where TStatus: class, new()
    {
        var taskId = queue.Enqueue<ICommandBus>(commands => commands.Send(task, CancellationToken.None));
        var jobData = JobStorage.Current.GetConnection().GetJobData(taskId);
        return new BackgroundTask<TResult, TStatus>(taskId, jobData.CreatedAt, jobData.State)
        {
            Progress = await tracking.CreateAsync<TStatus>(taskId, reference, CancellationToken.None)
        };
    }
    

    public IEnumerable<IBackgroundTask> GetTasks(params string[] tasksId)
    {
        var processes = tracking.GetJobsAsync(tasksId, CancellationToken.None).Result;
        var jobs = tasksId.Select(id =>
        {
            var job = JobStorage.Current.GetReadOnlyConnection().GetJobData(id);
            var jobTracking = processes.FirstOrDefault(p => p.TaskId == id);
            return new BackgroundTask(id, job.CreatedAt, job.State)
            {
                Progress = jobTracking == null
                    ? null
                    : tracking.GetJobProgress(jobTracking)
            };
        });
        return jobs.ToList();
        
    }
    
}