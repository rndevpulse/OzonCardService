using OzonCard.Data.Models;

namespace OzonCard.Context.Interfaces
{
    public interface IBalanceRepository
    {
        Task<IEnumerable<Organization>> GetOrganizations();
        Task<IEnumerable<Customer>> GetCustomersOrganization(Guid organizationId);
        Task UpdateBalansCustomers(IEnumerable<CustomerWallet> customerWallets);
    }
}
