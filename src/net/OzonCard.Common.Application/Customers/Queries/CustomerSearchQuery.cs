using OzonCard.Common.Core;
using OzonCard.Common.Domain.Customers;

namespace OzonCard.Common.Application.Customers.Queries;

public class CustomerSearchQuery : IQuery<Customer>
{
    public string Name { get; set; } = "";
    public string Card { get; set; } = "";
    public Guid OrganizationId { get; set; }
    public Guid CorporateNutritionId { get; set; }
    public string DateFrom { get; set; } = "";
    public string DateTo { get; set; } = "";
    public bool isOffline { get; set; }
}