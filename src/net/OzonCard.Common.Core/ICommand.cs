using MediatR;

namespace OzonCard.Common.Core
{
    public interface ICommand : IRequest
    {
    
    }

    public interface ICommand<out TResult> : IRequest<TResult>
    {
    
    }
}