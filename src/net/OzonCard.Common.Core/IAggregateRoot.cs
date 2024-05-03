namespace OzonCard.Common.Core;

public interface IAggregateRoot : IWithId
{
    IEnumerable<object> Dequeue();
    void Delete();
}
