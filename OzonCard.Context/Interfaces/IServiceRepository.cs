
namespace OzonCard.Context.Interfaces
{
    public interface IServiceRepository
    {
        Task<bool> CreateBackup(string path);
        Task<bool> RemoveOldFile(int countDays);
        Task<bool> RemoveOldTokensRefresh(int countDays);
        Task<bool> RemoveOldEvents(int countDays);
    }
}
