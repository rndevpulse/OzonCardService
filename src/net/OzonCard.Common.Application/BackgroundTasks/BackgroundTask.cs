using MediatR;
using OzonCard.Common.Core;

namespace OzonCard.Common.Application.BackgroundTasks;

public abstract class BackgroundTask
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTimeOffset QueuedAt { get; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? CompletedAt { get; protected set; }
    public TaskStatus Status { get; protected set; } = TaskStatus.Running;
    
    public object? Result { get; protected set; }
    public object? Progress { get; protected set; }
    
    public Exception? Error { get; protected set; }
    public Guid? Reference { get; protected set; }

    public abstract Task ExecuteAsync(IMediator mediator);
    internal readonly CancellationTokenSource CancellationTokenSource = new();
    
    
    public void Cancel() => CancellationTokenSource.Cancel();
}

public class BackgroundTask<TResult> : BackgroundTask
{
    public ICommand<TResult> Command { get; }
    public new TResult? Result { get; private set; }

    public BackgroundTask(ICommand<TResult> command, Guid? reference = null)
    {
        Command = command;
        Reference = reference;
    }
    public override async Task ExecuteAsync(IMediator mediator)
    {
        try
        {
            Result = await mediator.Send(Command, CancellationTokenSource.Token);
            CompletedAt = DateTimeOffset.UtcNow;
            Status = TaskStatus.Completed;
        }
        catch (Exception e)
        {
            Error = e;
            Status = TaskStatus.Failed;
        }
    }
}

public class BackgroundTask<TResult, TStatus> : BackgroundTask<TResult>, IProgress<TStatus>
{
    
    public BackgroundTask(ICommand<TResult> command, Guid? reference = null) : base(command, reference)
    {
    }
    public void Report(TStatus value) => Progress = value;
}
