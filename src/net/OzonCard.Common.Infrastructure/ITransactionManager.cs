namespace OzonCard.Common.Infrastructure
{
    public interface ITransactionManager
    {
        bool StartTransaction();
        Task CommitAsync(CancellationToken ct = default);
        void HasError(Exception e);
    }
}