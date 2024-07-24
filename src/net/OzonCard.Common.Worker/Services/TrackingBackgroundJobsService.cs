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
    

    public object? GetJobProgress(IJobProgress job) =>
        ReadFromFile<object>(job.Path);

    public async Task<IJobProgress?> GetJobAsync(string taskId, CancellationToken ct) =>
        await _repository.GetItemAsync(taskId, ct);
    public async Task<IJobProgress?> GetJobAsync(Guid track, CancellationToken ct) =>
        await _repository.GetItemAsync(track, ct);

    public async Task<IEnumerable<IJobProgress>> GetJobsAsync(IEnumerable<string> ids, CancellationToken ct)
    {
        return await _repository.GetItemsAsync(ids, ct);
    }

    public void ReportProgress<TProgress>(IJobProgress? job, TProgress progress) where TProgress : NamedProgress<TProgress>, new()
    {
        if (job == null)
            return;
        var value = ReadFromFile<TProgress>(job.Path) ?? new TProgress();
        value.Report(progress);
        value.SetType(typeof(TProgress).Name);
        SaveToFile(value, job.Path);
    } 

    private void SaveToFile(object? value, string name)
    {
        File.WriteAllText(
            Path.Combine(_path,name),
            JsonSerializer.Serialize(value)
        );
    }

    private T? ReadFromFile<T>(string name)
    {
        if (!File.Exists(Path.Combine(_path, name)))
            return default;
        var json = File.ReadAllText(Path.Combine(_path, name));
        return JsonSerializer.Deserialize<T>(json);
    }
  
}
