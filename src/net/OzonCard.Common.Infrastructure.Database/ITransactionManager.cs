namespace OzonCard.Common.Infrastructure.Database
{
    public interface ITransactionManager
    {
        bool StartTransaction();
        Task CommitAsync(CancellationToken ct = default);
        void HasError(Exception e);
    }
}