using OzonCard.Common.Application.Customers.Commands;
using OzonCard.Common.Core;
using OzonCard.Common.Domain.Customers;

namespace OzonCard.Common.Application.Customers.Handlers;

public class CustomersUpdateLastVisitHandler(
    ICustomerRepository repository
) : ICommandHandler<CustomersUpdateLastVisit, IEnumerable<Customer>>
{
    
    
    public async Task<IEnumerable<Customer>> Handle(CustomersUpdateLastVisit request, CancellationToken cancellationToken)
    {
        var customers = await repository.GetItemsAsync(request.OrganizationId, cancellationToken);
        var result = new List<Customer>();
        foreach (var customer in customers)
        {
            var card = customer.Cards.FirstOrDefault()?.Number;
            if (string.IsNullOrEmpty(card))
                continue;
            if (request.CustomersVisit
                    .FirstOrDefault(x =>
                        x.Card?.Contains(card) == true) is not { } info) continue;
            customer.LastVisit = info.LastVisitDate;
            result.Add(customer);
        }
        return result;
    }
}