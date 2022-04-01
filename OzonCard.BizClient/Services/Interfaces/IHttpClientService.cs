
using OzonCard.BizClient.Models;
using OzonCard.BizClient.Models.Data;

namespace OzonCard.BizClient.Services.Interfaces
{
    public interface IHttpClientService
    {

        protected Task<string> CreateToken(Identification identification);
        Task<Session?> GetSession(string login, string password);
        Task<IEnumerable<Organization>> GetOrganizations(Session access_session);
        Task<IEnumerable<Category>> GetOrganizationCategories(Session access_session, Guid organizationId);
        Task<IEnumerable<CorporateNutrition>> GetOrganizationCorporateNutritions(Session access_session, Guid organizationId);

    }
}
