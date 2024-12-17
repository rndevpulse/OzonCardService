
using OzonCard.Common.Core;

namespace OzonCard.Common.Worker.Data;

public class BackgroundTask(string id, DateTimeOffset queuedAt, string status) : IBackgroundTask
{
    public string Id { get; } = id;
    public string Status { get; set; } = status;
    public DateTimeOffset QueuedAt { get; } = queuedAt;
    public DateTimeOffset? CompletedAt { get; set; }
    public DateTimeOffset? ProcessedAt { get; set; }
    public string? Title { get; set; }
    public string? Error { get; set; }
    public object? Progress { get; set; }
    public object? Result { get; set; }
   
}

public class BackgroundTask<TResult>(
    string id, 
    DateTimeOffset queuedAt, 
    string status = "Created"
) : BackgroundTask(id, queuedAt, status)
{
    public new TResult? Result { get; set; }
    public void SetResult(TResult result) => Result = result;
}


// public class BackgroundTask<TResult, TStatus>(
//     string id, 
//     DateTimeOffset queuedAt, 
//     string status = "Created"
// ) : BackgroundTask<TResult>(id, queuedAt, status) where TStatus : NamedProgress
// {
//     public new NamedProgress? Progress { get; init; }
// }
//
//



