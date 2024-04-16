namespace OzonCard.Common.Core;

public interface ICommandBus
{
    /// <summary>
    /// Send command to event bus with result
    /// </summary>
    /// <param name="command"></param>
    /// <param name="ct"></param>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    Task<TResult> Send<TResult>(ICommand<TResult> command, CancellationToken ct = default);
    Task Send(ICommand command, CancellationToken ct = default);
}