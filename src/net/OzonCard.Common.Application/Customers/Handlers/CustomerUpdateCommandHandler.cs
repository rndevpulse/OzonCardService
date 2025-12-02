using OzonCard.Cloud.Client.Data.Customers;
using OzonCard.Common.Application.Customers.Commands;
using OzonCard.Common.Application.Organizations;
using OzonCard.Common.Core;
using Customer = OzonCard.Common.Domain.Customers.Customer;

namespace OzonCard.Common.Application.Customers.Handlers;

public class CustomerUpdateCommandHandler(
    ICustomerRepository customers,
    IOrganizationRepository organizations
) : ICommandHandler<CustomerUpdateCommand, Customer>
{
    public async Task<Customer> Handle(CustomerUpdateCommand request, CancellationToken cancellationToken)
    {
        var customer = await customers.GetItemAsync(request.Id, cancellationToken);
        customer.TabNumber = request.TabNumber;
        customer.Position = request.Position;
        customer.Division = request.Division;
        if (customer.Name == request.Name) 
            return customer;
        customer.Name = request.Name;
        var org = await organizations.GetItemAsync(customer.OrgId, cancellationToken);
        await org.CloudClient.CreateOrUpdateCustomerAsync(new CreateOrUpdateCustomer(org.TransportId)
        {
            Id = customer.BizId,
            Name = customer.Name,
        }, cancellationToken);
        return customer;
    }
}