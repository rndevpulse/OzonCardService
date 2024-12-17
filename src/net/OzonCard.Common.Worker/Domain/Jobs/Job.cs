using System.Text.Json;
using OzonCard.Common.Core;

namespace OzonCard.Common.Worker.Domain.Jobs;

public class Job : IWithId
{
    public Guid Id { get; }
    public DateTimeOffset CreatedAt { get; protected set; }
    public DateTimeOffset? ProcessedAt { get; set; }
    public string Number { get; protected set; }
    public Guid User { get; protected set; }
    public string Arguments { get; set; }
    public string Status { get; set; }
    public DateTimeOffset? Closed { get; set; }
    public string? Progress { get; set; }
    public string? Result { get; set; }
    public string? Reason { get; set; }
    public string? Title { get; set; }
    
    
    public Job(Guid id, string number, Guid user, string status, string arguments)
    {
        Id = id;
        Status = status;
        Number = number;
        User = user;
        Arguments = arguments;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public object? GetJobProgress()
    {
        return JsonSerializer.Deserialize<object>(Progress ?? "{}");
    }

    public object? GetJobResult()
    {
        return JsonSerializer.Deserialize<object>(Result ?? "{}");
    }
}