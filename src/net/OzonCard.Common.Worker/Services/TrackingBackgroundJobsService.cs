using System.Text.Json;
using Microsoft.Extensions.Configuration;
using OzonCard.Common.Worker.Data;
using OzonCard.Common.Worker.JobsProgress;

namespace OzonCard.Common.Worker.Services;

internal class TrackingBackgroundJobsService(
    IJobProgressRepository repository,
    IConfiguration configuration
    ) : ITrackingBackgroundJobs
{
    private string _path = Path.Combine(
        configuration.GetValue<string>("jobs:trackingPath") ?? Directory.GetCurrentDirectory(),
        "jobsTracking");
    public async Task<TStatus> CreateAsync<TStatus>(string taskId, Guid reference, CancellationToken ct) where TStatus : class, new()
    {
        var status = new TStatus();
        var name = $"{Guid.NewGuid()}.json";
        SaveToFile(status, name);
        await repository.AddAsync(taskId, reference, name, ct);
        return status;
    }

    public object? GetJobProgress(IJobProgress job) =>
        ReadFromFile(job.Path);

    public async Task<IJobProgress?> GetJobAsync(Guid reference, CancellationToken ct) =>
        await repository.FindJobAsync(reference, ct);

    public async Task<IEnumerable<IJobProgress>> GetJobsAsync(IEnumerable<string> ids, CancellationToken ct)
    {
        return await repository.GetJobsAsync(ids, ct);
    }

    public void ReportProgress<TProgress>(IJobProgress job, TProgress progress) =>
        SaveToFile(progress, job.Path);

    private void SaveToFile(object? value, string name)
    {
        if (!Directory.Exists(_path))
            Directory.CreateDirectory(_path);
        
        File.WriteAllText(
            Path.Combine(_path,name),
            JsonSerializer.Serialize(value)
        );
    }

    private object? ReadFromFile(string name)
    {
        if (!File.Exists(Path.Combine(_path, name)))
            return null;
        var value = File.ReadAllText(Path.Combine(_path, name));
        return JsonSerializer.Deserialize<JsonElement>(value);
    }
}
