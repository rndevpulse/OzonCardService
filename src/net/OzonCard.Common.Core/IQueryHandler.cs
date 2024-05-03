using MediatR;

namespace OzonCard.Common.Core
{
    /// <summary>
    /// Base query handler
    /// </summary>
    /// <typeparam name="TRequest">Request body</typeparam>
    /// <typeparam name="TResponse">Response body</typeparam>
    public interface IQueryHandler<in TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : IQuery<TResponse>
    {
    }
}