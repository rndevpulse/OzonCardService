
using Microsoft.EntityFrameworkCore;
using OzonCard.Context.Interfaces;
using OzonCard.Data.Models;

namespace OzonCard.Context.Repositories
{
    public class OrganizationRepository : BaseRepository, IOrganizationRepository
    {
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
                    organization.Categories.Find(x => x.Equals(category)).IsActive = category.IsActive;
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
                    category.IsActive = olds.FirstOrDefault(x => x.Equals(category))?.IsActive ?? category.IsActive;
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
                return Categories ?? new List<Category>();
            }
        }

        #endregion


        #region Взаимодействие с организациями

        public async Task AddOrganization(Organization organization, Guid userId)
        {
            using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                var user = await context.Users
                    .Where(x => x.Id == userId)
                    .FirstOrDefaultAsync();
                if (user != null)
                {
                    organization.Users.Add(user);
                    context.Organizations.Add(organization);
                    await context.SaveChangesAsync();
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
                   .ToListAsync();
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
                var result = сorporateNutrition.Except(organization?.CorporateNutritions ?? new List<CorporateNutrition>());
                organization?.CorporateNutritions.AddRange(result);
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
                return corporateNutritions ?? new List<CorporateNutrition>();
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

        #endregion


        #region ----------
        #endregion




    }
}
