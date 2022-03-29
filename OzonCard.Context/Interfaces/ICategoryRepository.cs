using OzonCard.Data.Models;

namespace OzonCard.Context.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetCategories();
        Task AddCategory(Category category);
        Task AddRangeCategory(IEnumerable<Category> categories);


    }
}
