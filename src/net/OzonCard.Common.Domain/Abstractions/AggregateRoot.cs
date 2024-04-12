using OzonCard.Common.Core;

namespace OzonCard.Common.Domain.Abstractions;

public abstract class AggregateRoot : DomainObject, IAggregateRoot
{
    [NonSerialized] private readonly Queue<object> _uncommittedEvents = new();

    protected AggregateRoot(Guid? id = null) : base(id)
    {
    }

    /// <summary>
    /// Get events objects from queue. Clear after accessing
    /// </summary>
    public IEnumerable<object> Dequeue()
    {
        while (_uncommittedEvents.TryDequeue(out var ev))
            yield return ev;
    }

    /// <summary>
    /// Add item to event queue
    /// </summary>
    /// <param name="e"></param>
    protected void Enqueue(object e)
    {
        _uncommittedEvents.Enqueue(e);
    }
}