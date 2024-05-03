using OzonCard.Common.Application.Customers.Commands;
using OzonCard.Common.Core;
using OzonCard.Common.Domain.Customers;

namespace OzonCard.Common.Application.Customers.Handlers;

public class CustomerRemoveCommandHandler(
    ICustomerRepository customers
) : ICommandHandler<CustomerRemoveCommand, Customer>
{

    public async Task<Customer> Handle(CustomerRemoveCommand request, CancellationToken cancellationToken)
    {
        var customer = await customers.GetItemAsync(request.Id, cancellationToken);
        customers.Remove(customer);
        return customer;
    }
}