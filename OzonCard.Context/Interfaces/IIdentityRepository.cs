using OzonCard.Data.Models;

namespace OzonCard.Context.Interfaces
{
    public interface IIdentityRepository
    {
        Task<User?> GetUser(string userName, Guid password);
        Task<bool> AddRefreshToken(User user, RefreshToken? refreshToken = null);
        Task<User?> GetUser(string refreshToken);
    }
}
