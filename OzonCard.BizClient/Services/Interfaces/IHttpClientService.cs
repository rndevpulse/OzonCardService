﻿
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



        Task<Guid> CreateCustomer(Session access_session, string name, string card, Guid organizationId);
        Task<bool> AddCategotyCustomer(Session access_session, Guid iikoBizId, Guid organizationId, Guid categoryId);
        Task<Guid> AddCorporateNutritionCustomer(Session access_session, Guid iikoBizId, Guid organizationId, Guid corporateNutritionId);
        Task<Customer?> GetCustomerForId(Session session, Guid iikoBizId, Guid organizationId);
        Task<double?> GetCustomerBalanceForId(Session session, Guid iikoBizId, Guid organizationId, Guid walletId);

        Task<bool> AddBalanceByCustomer(Session session, Guid iikoBizId, Guid organizationId, Guid walletId, double balance);
        Task<bool> DelBalanceByCustomer(Session session, Guid iikoBizId, Guid organizationId, Guid walletId, double balance);
    }
}
