using System.Text.Json;
using Microsoft.Extensions.Configuration;
using OzonCard.Common.Core;
using OzonCard.Common.Worker.Application.Jobs.Events;
using OzonCard.Common.Worker.Data;
using OzonCard.Common.Worker.Domain.Jobs;
using OzonCard.Common.Worker.Stores;

namespace OzonCard.Common.Worker.Services;


internal class TrackingBackgroundJobsService : ITrackingBackgroundJobs
{
    
    private readonly IEventBus _events;
    private readonly IStoreContext _store;
    private readonly string _path;

    public TrackingBackgroundJobsService(
        IEventBus events,
        IStoreContext store,
        IConfiguration configuration)
    {
        _events = events;
        _store = store;
        _path = Path.Combine(
            configuration.GetValue<string>("content") ?? Directory.GetCurrentDirectory(),
            "jobsTracking");
        
        if (!Directory.Exists(_path))
            Directory.CreateDirectory(_path);
    }

    public void Observe<TResult>(ICommand<TResult> task, string taskId, Guid track, Guid? user)
    {
        _events.Publish(new OnCreatedJobEvent(
            track,
            taskId,
            user,
            $"[{task.GetType().Name}]{JsonSerializer.Serialize(task, task.GetType())}"));
        // var job = new Job(
        //     track,
        //     taskId,
        //     user ?? Guid.Empty,
        //     $"[{task.GetType().Name}]{JsonSerializer.Serialize(task, task.GetType())}"
        // );
        // _store.Append<Job>(job);
    }
    

    // public JobProgress<object> GetJobProgress(IJobProgress job) =>
    //     ReadFromFile<object>(job.Path) 
    //     ?? new JobProgress<object>(null, null);


    public async Task<Job?> GetJobAsync(Guid track, CancellationToken ct) =>
        await _store.GetItemAsync<Job>(track, ct); 
        // await _repository.GetItemAsync(track, ct);
    

    public async Task<IEnumerable<Job>> GetJobsAsync(IEnumerable<string> ids, CancellationToken ct) =>
        await _store.GetItemsAsync<Job>(x=>ids.Contains(x.Number),ct); 
        // await _repository.GetItemsAsync(ids, ct);

    
    public void ReportProgress<TProgress>(Guid? track, TProgress progress, object? result = null) where TProgress : NamedProgress, new()
    {
        if (track == null)
            return;
        _events.Publish(new OnProgressJobEvent(
            (Guid)track,
            progress,
            result));
        // var value = ReadFromFile<TProgress>(job.Path) 
        //             ?? new JobProgress<TProgress>(new TProgress(), result);
        // value.Status.Report(progress);
        // value.Status.SetType(typeof(TProgress).Name);
        //
        // SaveToFile(value with {Result = result}, job.Path);
    } 

    // private void SaveToFile<TProgress>(JobProgress<TProgress> value, string name)
    // {
    //     File.WriteAllText(
    //         Path.Combine(_path,name),
    //         JsonSerializer.Serialize(value)
    //     );
    // }
    //
    // private JobProgress<TProgress>? ReadFromFile<TProgress>(string name)
    // {
    //     if (!File.Exists(Path.Combine(_path, name)))
    //         return default;
    //     var json = File.ReadAllText(Path.Combine(_path, name));
    //     return JsonSerializer.Deserialize<JobProgress<TProgress>>(json);
    // }
  
}
