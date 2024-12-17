namespace OzonCard.Customer.Api.Models.BackgroundTask;

public record BackgroundTaskModel(
    string Id,
    DateTimeOffset QueuedAt,
    DateTimeOffset? CompletedAt,
    DateTimeOffset? ProcessedAt,
    string? Title,
    string Status,
    string? Error,
    object? Progress,
    object? Result
);
