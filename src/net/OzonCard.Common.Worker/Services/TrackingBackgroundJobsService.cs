using System.Text.Json;
using Microsoft.Extensions.Configuration;
using OzonCard.Common.Worker.Data;
using OzonCard.Common.Worker.JobsProgress;

namespace OzonCard.Common.Worker.Services;


internal class TrackingBackgroundJobsService : ITrackingBackgroundJobs
{
    
    private readonly IJobProgressRepository _repository;
    private readonly string _path;

    public TrackingBackgroundJobsService(
        IJobProgressRepository repository, 
        IConfiguration configuration)
    {
        _repository = repository;
        _path = Path.Combine(
            configuration.GetValue<string>("content") ?? Directory.GetCurrentDirectory(),
            "jobsTracking");
        
        if (!Directory.Exists(_path))
            Directory.CreateDirectory(_path);
    }

    public void Observe(string taskId, Guid track) =>
        _repository.Add(taskId, track, $"{track}.json");
    

    public JobProgress<object> GetJobProgress(IJobProgress job) =>
        ReadFromFile<object>(job.Path) 
        ?? new JobProgress<object>(null, null);

    public async Task<IJobProgress?> GetJobAsync(string taskId, CancellationToken ct) =>
        await _repository.GetItemAsync(taskId, ct);
    public async Task<IJobProgress?> GetJobAsync(Guid track, CancellationToken ct) =>
        await _repository.GetItemAsync(track, ct);

    public async Task<IEnumerable<IJobProgress>> GetJobsAsync(IEnumerable<string> ids, CancellationToken ct) =>
        await _repository.GetItemsAsync(ids, ct);

    
    public void ReportProgress<TProgress>(IJobProgress? job, TProgress progress, object? result = null) where TProgress : NamedProgress<TProgress>, new()
    {
        if (job == null)
            return;
        var value = ReadFromFile<TProgress>(job.Path) 
                    ?? new JobProgress<TProgress>(new TProgress(), result);
        value.Status.Report(progress);
        value.Status.SetType(typeof(TProgress).Name);
        
        SaveToFile(value with {Result = result}, job.Path);
    } 

    private void SaveToFile<TProgress>(JobProgress<TProgress> value, string name)
    {
        File.WriteAllText(
            Path.Combine(_path,name),
            JsonSerializer.Serialize(value)
        );
    }

    private JobProgress<TProgress>? ReadFromFile<TProgress>(string name)
    {
        if (!File.Exists(Path.Combine(_path, name)))
            return default;
        var json = File.ReadAllText(Path.Combine(_path, name));
        return JsonSerializer.Deserialize<JobProgress<TProgress>>(json);
    }
  
}
