using OzonCard.Data.Models;

namespace OzonCard.Context.Interfaces
{
    public interface IOrganizationRepository
    {
        //взаимодействие с организациями
        Task<IEnumerable<Organization>> GetMyOrganizations(Guid userId);
        Task<Organization?> GetMyOrganization(Guid userId, Guid organizationId);
        Task AddOrganization(Organization organization, Guid userId);

        //взаимодействие с категориями
        Task<IEnumerable<Category>> GetCategories(Guid organizationId);
        Task AddCategory(Category category, Guid organizationId);
        Task AddRangeCategory(IEnumerable<Category> categories, Guid organizationId);


        //взаимодействие с программами питания
        Task<IEnumerable<CorporateNutrition>> GetCorporateNutritions(Guid organizationId);
        Task AddCorporateNutrition(CorporateNutrition сorporateNutrition, Guid organizationId);
        Task AddRangeCorporateNutrition(IEnumerable<CorporateNutrition> сorporateNutrition, Guid organizationId);


        //взаимодействие с пользователями организаций
        Task AddUserForOrganization(Guid userId, Guid organizationId);
        Task DelUserForOrganization(Guid userId, Guid organizationId);





    }
}
