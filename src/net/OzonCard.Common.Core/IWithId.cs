namespace OzonCard.Common.Core;

public interface IWithId<out TKey> where TKey : struct
{
    TKey Id { get; }
}

public interface IWithId : IWithId<Guid>
{
}