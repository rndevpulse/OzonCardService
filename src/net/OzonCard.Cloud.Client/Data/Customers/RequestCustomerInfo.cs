using System.Text.Json.Serialization;

namespace OzonCard.Cloud.Client.Data.Customers;

public class RequestCustomerInfo : CreateOrUpdateCustomer
{
    public CustomerField Type { get; set; }
    public RequestCustomerInfo(Guid organizationId, CustomerField type) : base(organizationId)
    {
        Type = type;
    }
    
}