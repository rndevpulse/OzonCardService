namespace OzonCard.Common.Core;

public interface IBackgroundTask
{
    string Id { get; }
    DateTimeOffset QueuedAt { get; }
    string Status { get; set; }
    DateTimeOffset? CompletedAt { get; set; }
    object? Progress{ get; protected set; }
    object? Result { get; protected set; }
    string? Error { get; set; }
}