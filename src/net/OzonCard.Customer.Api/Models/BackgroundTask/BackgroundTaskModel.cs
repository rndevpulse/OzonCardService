namespace OzonCard.Customer.Api.Models.BackgroundTask;

public record BackgroundTaskModel(
    Guid Id,
    DateTimeOffset QueuedAt,
    DateTimeOffset? CompletedAt,
    string Status,
    string? Error,
    object? Progress
);
