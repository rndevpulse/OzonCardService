namespace OzonCard.Common.Core;

public interface IQueryBus
{
    /// <summary>
    /// Send command to event bus with result
    /// </summary>
    /// <param name="command"></param>
    /// <param name="ct"></param>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    Task<TResult> Send<TResult>(IQuery<TResult> command, CancellationToken ct = default);
}