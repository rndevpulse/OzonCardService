using OzonCard.Data.Models;

namespace OzonCard.Context.Interfaces
{
    public interface IIdentityRepository
    {
        Task<User?> GetUser(string userName);
    }
}
