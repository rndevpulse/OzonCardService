
using Microsoft.EntityFrameworkCore;
using OzonCard.Context.Interfaces;
using OzonCard.Data.Models;
using Serilog;

namespace OzonCard.Context.Repositories
{
    public class OrganizationRepository : BaseRepository, IOrganizationRepository
    {
        private readonly ILogger log = Log.ForContext(typeof(OrganizationRepository));


        public OrganizationRepository(string connectionString, IRepositoryContextFactory contextFactory) : base(connectionString, contextFactory) { }



        #region Взаимодействие с категориями

        public async Task AddCategory(Category category, Guid organizationId)
        {
            using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                var organization = await context.Organizations
                    .Where(x => x.Id == organizationId)
                    .Include(x => x.Categories)
                    .FirstOrDefaultAsync();
                if (organization == null)
                    return;
                if (organization.Categories.Contains(category))
                    organization.Categories.Find(x => x.Equals(category)).isActive = category.isActive;
                else
                    organization.Categories.Add(category);
                await context.SaveChangesAsync();
            }
        }

        public async Task AddRangeCategory(IEnumerable<Category> categories, Guid organizationId)
        {
            using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                var organization = await context.Organizations
                    .Where(x => x.Id == organizationId)
                    .Include(x => x.Categories)
                    .FirstOrDefaultAsync();
                var result = categories.Except(organization?.Categories ?? new List<Category>());

                var olds = categories.Except(result);
                foreach (var category in organization?.Categories)
                {
                    category.isActive = olds.FirstOrDefault(x => x.Equals(category))?.isActive ?? category.isActive;
                }
                organization?.Categories.AddRange(result);
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Category>> GetCategories(Guid organizationId)
        {
            using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                var Categories = await context.Organizations
                    .Where(x => x.Id == organizationId)
                    .Include(x => x.Categories)
                    .Select(x => x.Categories)
                    .FirstOrDefaultAsync();
                return Categories?.Where(x => x.isActive) ?? new List<Category>();
            }
        }

        #endregion


        #region Взаимодействие с организациями

        public async Task AddOrganizations(IEnumerable<Organization> organizations, Guid userId)
        {
            using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                var user = await context.Users
                    .Where(x => x.Id == userId)
                    .FirstOrDefaultAsync();
                if (user != null)
                {
                    foreach (var organization in organizations)
                        organization.Users.Add(user);
                    context.Organizations.AddRange(organizations);
                    try
                    {
                        await context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        log.Warning(ex, "SQL AddOrganizations");
                    }
                }
            }
        }
        public async Task<Organization?> GetMyOrganization(Guid userId, Guid organizationId)
        {
            using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                var organizations = await context.Users
                   .Where(x => x.Id == userId)
                   .Include(x => x.Organizations)
                   .SelectMany(x => x.Organizations)
                   .Where(x => x.Id == organizationId)
                   .FirstOrDefaultAsync();
                return organizations;
            }
        }

        public async Task<IEnumerable<Organization>> GetMyOrganizations(Guid userId)
        {
            using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                var organizations = await context.Users
                   .Where(x => x.Id == userId)
                   .Include(x => x.Organizations)
                   .SelectMany(x => x.Organizations)
                   .Include(x => x.CorporateNutritions.Where(c => c.isActive))
                   .Include(x => x.Categories.Where(c => c.isActive))
                   .ToListAsync();
                return organizations;
            }
        }

        public async Task<Organization?> GetOrganization(Guid organizationId)
        {
            using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                var organizations = await context.Organizations
                   .Where(x => x.Id == organizationId)
                   .Include(x => x.Users)
                   .Include(x => x.Categories)
                   .Include(x => x.CorporateNutritions)
                   .ThenInclude(x => x.Wallets)
                   .FirstOrDefaultAsync();
                return organizations;
            }
        }

        #endregion


        #region Взаимодействие с программами питания
        public async Task AddCorporateNutrition(CorporateNutrition сorporateNutrition, Guid organizationId)
        {
            using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                var organization = await context.Organizations
                    .Where(x => x.Id == organizationId)
                    .Include(x => x.CorporateNutritions)
                    .FirstOrDefaultAsync();
                if (organization?.CorporateNutritions.Contains(сorporateNutrition) == false)
                {
                    organization.CorporateNutritions.Add(сorporateNutrition);
                    await context.SaveChangesAsync();
                }
            }
        }
        public async Task AddRangeCorporateNutrition(IEnumerable<CorporateNutrition> сorporateNutrition, Guid organizationId)
        {
            using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                var organization = await context.Organizations
                    .Where(x => x.Id == organizationId)
                    .Include(x => x.CorporateNutritions)
                    .FirstOrDefaultAsync();


                var result = сorporateNutrition.Except(organization?.CorporateNutritions ?? new List<CorporateNutrition>()).ToList();
                organization?.CorporateNutritions.AddRange(result);
                

                var olds = organization?.CorporateNutritions.Except(сorporateNutrition);
                foreach (var corpNut in olds)
                {
                    corpNut.isActive = false;
                }

                await context.SaveChangesAsync();
            }
        }


        public async Task<IEnumerable<CorporateNutrition>> GetCorporateNutritions(Guid organizationId)
        {
            using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                var corporateNutritions = await context.Organizations
                    .Where(x => x.Id == organizationId)
                    .Include(x => x.CorporateNutritions)
                    .Select(x => x.CorporateNutritions)
                    .FirstOrDefaultAsync();
                return corporateNutritions?.Where(x => x.isActive) ?? new List<CorporateNutrition>();
            }
        }








        #endregion


        #region Взаимодействие с пользователями организаций

        public async Task AddUserForOrganization(Guid userId, Guid organizationId)
        {
            using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                var user = await context.Users
                    .Where(x => x.Id == userId)
                    .FirstOrDefaultAsync();
                if (user != null)
                {
                    var organization = await context.Organizations
                       .FirstOrDefaultAsync(x => x.Id == organizationId);
                    organization?.Users.Add(user);
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task DelUserForOrganization(Guid userId, Guid organizationId)
        {
            using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                var user = await context.Users
                    .Where(x => x.Id == userId)
                    .FirstOrDefaultAsync();
                if (user != null)
                {
                    var organization = await context.Organizations
                       .FirstOrDefaultAsync(x => x.Id == organizationId);
                    organization?.Users.Remove(user);
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task AddUser(User user)
        {
            using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                if (context.Users.Any(x => x.Mail == user.Mail))
                    throw new NullReferenceException("User login is olready use");
                context.Users.Add(user);
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                var users = await context.Users
                    .Where(x => x.Mail != "admin")
                    .Include(x => x.Organizations)
                    .ToListAsync();
                return users;
            }
        }


        #endregion


        #region Файлы


        public async Task AddFile(FileReport file)
        {
            using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                context.FileReports.Add(file);
                await context.SaveChangesAsync();
            }
        }

        public async Task RemoveFiles(DateTime dateTime)
        {
            using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                var remove = await context.FileReports
                    .Where(x => x.Created < dateTime)
                    .ToListAsync();
                context.FileReports.RemoveRange(remove);
                await context.SaveChangesAsync();
            }
        }


        #endregion


        #region Покупатели



        public async Task<IEnumerable<Customer>> GetCustomersForCardNumber(IEnumerable<string> cardnumbers)
        {
            using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                var customers = await context.Cards
                    .Where(x => cardnumbers.Contains(x.Track))
                    .Include(x =>x.Customer)
                    .Select(x=>x.Customer)
                    .ToListAsync();

                customers = await context.Customers
                    .Where(x => customers.Contains(x))
                    .Include(x => x.Categories)
                    .Include(x => x.Wallets)
                    .ThenInclude(x => x.Wallet)
                    .Include(x => x.Cards)
                    .ToListAsync();

                return customers;
            }
        }
        public async Task<IEnumerable<Customer>> GetCustomersForOrganization(Guid organizationId)
        {
            using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                var customers = await context.Customers
                    .Where(x => x.Organization.Id == organizationId)
                    .ToListAsync();
                return customers;
            }
        }
        public async Task AttachRangeCustomer(IEnumerable<Customer> customers)
        {
            using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                context.Customers.AttachRange(customers);
                
                await context.SaveChangesAsync();
            }
        }
        public async Task UpdateCustomer(Customer customer)
        {
            using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                context.Customers.AttachRange(customer);
                context.Entry(customer).Property(e => e.TabNumber).IsModified = true;
                context.Entry(customer).Property(e => e.Name).IsModified = true;
                context.Entry(customer).Property(e => e.Position).IsModified = true;
                await context.SaveChangesAsync();
            }
        }

        



        #endregion
    }
}
