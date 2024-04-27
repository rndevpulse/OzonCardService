using OzonCard.Common.Application.Customers.Commands;
using OzonCard.Common.Core;
using OzonCard.Common.Domain.Customers;

namespace OzonCard.Common.Application.Customers.Handlers;

public class CustomerUpdateCommandHandler(
    ICustomerRepository customers
) : ICommandHandler<CustomerUpdateCommand, Customer>
{
    public async Task<Customer> Handle(CustomerUpdateCommand request, CancellationToken cancellationToken)
    {
        var customer = await customers.GetItemAsync(request.Id, cancellationToken);
        customer.TabNumber = request.TabNumber;
        customer.Position = request.Position;
        customer.Division = request.Division;
        customer.Name = request.Name;
        return customer;
    }
}