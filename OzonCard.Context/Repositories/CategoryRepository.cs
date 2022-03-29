
using Microsoft.EntityFrameworkCore;
using OzonCard.Context.Interfaces;
using OzonCard.Data.Models;

namespace OzonCard.Context.Repositories
{
    public class CategoryRepository : BaseRepository, ICategoryRepository
	{
		public CategoryRepository(string connectionString, IRepositoryContextFactory contextFactory) : base(connectionString, contextFactory) { }

        public async Task AddCategory(Category category)
        {
			using (var context = ContextFactory.CreateDbContext(ConnectionString))
			{
				context.Categories.Add(category);
				await context.SaveChangesAsync();
			}
		}

        public async Task AddRangeCategory(IEnumerable<Category> categories)
        {
			using (var context = ContextFactory.CreateDbContext(ConnectionString))
			{
				context.Categories.AddRange(categories);
				await context.SaveChangesAsync();
			}
		}

        public async Task<IEnumerable<Category>> GetCategories()
        {
			using (var context = ContextFactory.CreateDbContext(ConnectionString))
			{
				return await context.Categories.ToListAsync();
			}
		}

	}
}
