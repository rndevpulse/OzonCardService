namespace OzonCard.Common.Worker.Data;

public interface IJobProgress
{
    string TaskId { get; set; }
    Guid Track { get; set; }

    string Path { get; set; }
    DateTimeOffset CreatedAt { get; set; }
}