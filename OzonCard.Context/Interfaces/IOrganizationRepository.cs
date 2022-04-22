using OzonCard.Data.Models;

namespace OzonCard.Context.Interfaces
{
    public interface IOrganizationRepository
    {
        //взаимодействие с организациями
        Task<IEnumerable<Organization>> GetMyOrganizations(Guid userId);
        Task<Organization?> GetMyOrganization(Guid userId, Guid organizationId);
        Task AddOrganizations(IEnumerable<Organization> organizations, Guid userId);
        Task<Organization?> GetOrganization(Guid organizationId);

        //взаимодействие с категориями
        Task<IEnumerable<Category>> GetCategories(Guid organizationId);
        Task AddCategory(Category category, Guid organizationId);
        Task AddRangeCategory(IEnumerable<Category> categories, Guid organizationId);


        //взаимодействие с программами питания
        Task<IEnumerable<CorporateNutrition>> GetCorporateNutritions(Guid organizationId);
        Task AddCorporateNutrition(CorporateNutrition сorporateNutrition, Guid organizationId);
        Task AddRangeCorporateNutrition(IEnumerable<CorporateNutrition> сorporateNutrition, Guid organizationId);


        //взаимодействие с пользователями организаций
        Task AddUser(User user);
        Task AddUserForOrganization(Guid userId, Guid organizationId);
        Task DelUserForOrganization(Guid userId, Guid organizationId);
        Task<IEnumerable<User>> GetUsers();
        
        
        Task AddFile(FileReport file);
        Task<IEnumerable<FileReport>> GetFiles(Guid userId);



        //взаимодействие с покупателями
        Task<IEnumerable<Customer>> GetCustomersForCardNumber(IEnumerable<string> enumerable);
        Task<IEnumerable<Customer>> GetCustomersForOrganization(Guid organizationId);
        Task AttachRangeCustomer(IEnumerable<Customer> customers);
        Task UpdateCustomer(Customer customer);
        Task RemoveFile(string url);
    }
}
