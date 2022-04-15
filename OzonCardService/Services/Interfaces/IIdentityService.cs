using OzonCard.Data.Models;
using OzonCardService.Models.DTO;
using System.Threading.Tasks;

namespace OzonCardService.Services.Interfaces
{
    public interface IIdentityService
    {
        Task<User> GetUser(string userName, string passsword);
        Task<Authenticate_dto> Authenticate(User user);
        Task<Authenticate_dto> RefreshToken(string refreshToken);
        Task<bool> LogoutToken(string refreshToken);
    }
}
