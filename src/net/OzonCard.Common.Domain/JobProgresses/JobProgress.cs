using OzonCard.Common.Domain.Abstractions;
using OzonCard.Common.Worker.Data;

namespace OzonCard.Common.Domain.JobProgresses;

public record JobProgress : ValueObject, IJobProgress
{
    public string TaskId { get; set; }
    public Guid Track { get; set; }
    public string Path { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}