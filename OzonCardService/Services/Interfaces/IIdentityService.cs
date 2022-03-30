using OzonCard.Data.Models;
using System.Threading.Tasks;

namespace OzonCardService.Services.Interfaces
{
    public interface IIdentityService
    {
        Task<User> GetUser(string userName);
    }
}
