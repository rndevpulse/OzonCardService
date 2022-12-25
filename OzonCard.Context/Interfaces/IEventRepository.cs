using OzonCard.Data.Models;

namespace OzonCard.Context.Interfaces
{
    public interface IEventRepository
    {
        Task<IEnumerable<Organization>> GetOrganizations();
        Task<IEnumerable<Event>> GetEventsOrganization(Guid organizationId, DateTime dateFrom, DateTime dateTo);
        Task<int> AppendEventsOrganization(IEnumerable<Event> events);
        Task<Event?> GetLastEventOrganization(Guid organizationId);
        Task<IEnumerable<CustomerReport>> GetCustomersReportOrganization(Guid organizationId, DateTime dateFrom, DateTime dateTo, string programmName);
        Task<IEnumerable<Customer>> GetCustomersOrganization(Guid organizationId);
        Task AttachRangeCustomer(IEnumerable<Customer> newCustomers);
        Task UpdateCategory(IEnumerable<CategoryCustomer> changedCategory, bool isRemove);
        Task SetCategories(IEnumerable<(Guid guestId, string guestCategoryNames)> enumerable, Guid organizationId);
    }
}
