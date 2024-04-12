using OzonCard.Common.Core;

namespace OzonCard.Common.Domain.Abstractions;

public class DomainObject : IWithId
{
    public Guid Id { get; private set; }
    public bool IsRemoved { get; private set; }

    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
    public DateTimeOffset CreatedAt { get; protected set; }

    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
    public DateTimeOffset UpdatedAt { get; protected set; }

    public uint Version { get; private set; }

    protected DomainObject(Guid? id = null)
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public virtual void Delete()
    {
        if (!IsRemoved)
            IsRemoved = true;
    }
}