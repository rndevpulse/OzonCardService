using MediatR;

namespace OzonCard.Common.Core
{
    public interface IEvent : INotification
    {
        Guid Aggregate { get; }
    }
}