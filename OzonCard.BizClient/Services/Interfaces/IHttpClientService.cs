
using OzonCard.BizClient.Models;
using OzonCard.BizClient.Models.Data;

namespace OzonCard.BizClient.Services.Interfaces
{
    public interface IHttpClientService
    {

        Task<string> CreateToken(Identification identification);
        Task<Session?> GetSession(Identification identification);
        Task<Session?> GetSession(string login, string password);
        Task<IEnumerable<Organization>> GetOrganizations(Session access_session);

    }
}
