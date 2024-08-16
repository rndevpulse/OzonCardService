using System.Text.Json;
using OzonCard.Common.Domain.Abstractions;

namespace OzonCard.Common.Domain.Props;

public abstract class Property : AggregateRoot
{
    public Guid Reference { get; }
    public PropType Type { get; }

    public string Data { get; private set; } = "{}";
    
    public virtual T? GetProperty<T>()
    {
        return JsonSerializer.Deserialize<T>(Data)
            ?? default;
    }

    public virtual void UpdateProperty(object value)
    {
        Data = JsonSerializer.Serialize(value);
    }

    protected Property(Guid reference, PropType type, Guid? id = null) : base(id ?? Guid.NewGuid())
    {
        Reference = reference;
        Type = type;
    }
}