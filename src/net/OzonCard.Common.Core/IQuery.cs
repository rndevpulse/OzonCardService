using MediatR;

namespace OzonCard.Common.Core;

public interface IQuery<out TResult> : IRequest<TResult>
{
}