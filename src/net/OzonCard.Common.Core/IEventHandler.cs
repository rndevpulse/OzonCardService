using MediatR;

namespace OzonCard.Common.Core
{
    public interface IEventHandler<in TEvent> : INotificationHandler<TEvent> where TEvent : IEvent
    {
        
    }
}