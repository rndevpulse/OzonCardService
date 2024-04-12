using MediatR;

namespace OzonCard.Common.Core
{
    /// <summary>
    /// Command handler without result
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    public interface ICommandHandler<in TRequest> : IRequestHandler<TRequest>
        where TRequest : ICommand
    {
    }

    /// <summary>
    /// Command handler with result
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public interface ICommandHandler<in TRequest, TResult> : IRequestHandler<TRequest, TResult>
        where TRequest : ICommand<TResult>
    {
    }
}