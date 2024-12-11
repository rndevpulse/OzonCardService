
namespace OzonCard.Common.Core
{
    public interface IEventBus
    {
        Task Publish(IEvent e, CancellationToken ct = default);
    }
}