using OzonCard.Biz.Client;
using OzonCard.Common.Application.Customers.Commands;
using OzonCard.Common.Application.Organizations;
using OzonCard.Common.Core;
using OzonCard.Common.Domain.Customers;

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
        var client = new BizClient(org.Login, org.Password);
        await client.UpdateCustomerAsync(customer.BizId, customer.Name, customer.OrgId, cancellationToken);
        return customer;
    }
}