namespace OzonCard.Customer.Api.Models.BackgroundTask;

public record BackgroundTaskModel(
    string Id,
    DateTimeOffset QueuedAt,
    DateTimeOffset? CompletedAt,
    string Status,
    string? Error,
    object? Progress
);
