namespace OzonCard.Common.Worker.Data;

public abstract record NamedProgress<TStatus> : IProgress<TStatus>
{
    public string? Type { get; protected set; }
    public void SetType(string type) => Type = type;
    public abstract void Report(TStatus value);
}