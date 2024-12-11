namespace OzonCard.Common.Worker.Data;

public abstract record NamedProgress
{
    public string? Type { get; protected set; }
    public void SetType(string type) => Type = type;
}