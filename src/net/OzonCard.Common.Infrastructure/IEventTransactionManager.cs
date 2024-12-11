namespace OzonCard.Common.Infrastructure
{
    public interface IEventTransactionManager
    {
        bool StartTransaction();
        Task CommitAsync(CancellationToken ct = default);
    }
}