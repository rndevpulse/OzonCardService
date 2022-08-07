using Microsoft.EntityFrameworkCore;
using OzonCard.Context.Interfaces;
using OzonCard.Data.Models;

namespace OzonCard.Context.Repositories
{
    public class BalanceRepository : BaseRepository, IBalanceRepository
    {
        public BalanceRepository(string connectionString, IRepositoryContextFactory contextFactory) : base(connectionString, contextFactory) { }

        public async Task<IEnumerable<Customer>> GetCustomersOrganization(Guid organizationId)
        {
            using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                var organizations = await context.Customers
                   .Include(x => x.Organization)
                   .Include(x => x.Wallets)
                   .Include(x => x.Categories)
                   .ThenInclude(x => x.Category)
                   .Where(x=>x.Organization.Id == organizationId)
                   .ToListAsync();
                return organizations;
            }
        }

        public async Task<IEnumerable<Organization>> GetOrganizations()
        {
            using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                var organizations = await context.Organizations
                   .Include(x => x.Categories)
                   .Include(x => x.CorporateNutritions)
                   .ThenInclude(x => x.Wallets)
                   .ToListAsync();
                return organizations;
            }
        }

        public async Task UpdateBalansCustomers(IEnumerable<CustomerWallet> customerWallets)
        {
            using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                context.CustomerWallets.UpdateRange(customerWallets);
                //context.Entry(customerWallets).Property(e=>e.First().Balance).IsModified = true;
                await context.SaveChangesAsync();
            }
        }
    }
}
