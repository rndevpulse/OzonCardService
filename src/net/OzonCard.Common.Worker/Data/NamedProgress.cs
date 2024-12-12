namespace OzonCard.Common.Worker.Data;

public abstract record NamedProgress
{
    public abstract string Type { get; }
}