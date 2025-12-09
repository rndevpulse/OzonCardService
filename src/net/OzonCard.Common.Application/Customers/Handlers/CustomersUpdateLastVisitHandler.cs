using Microsoft.Extensions.Logging;
using OzonCard.Cloud.Client.Data.Customers;
using OzonCard.Common.Application.Customers.Commands;
using OzonCard.Common.Application.Organizations;
using OzonCard.Common.Application.Visits;
using OzonCard.Common.Core;
using OzonCard.Common.Domain.Customers;
using OzonCard.Common.Domain.Organizations;
using Customer = OzonCard.Common.Domain.Customers.Customer;

namespace OzonCard.Common.Application.Customers.Handlers;

public class CustomersUpdateLastVisitHandler(
    ICustomerRepository repository,
    IVisitRepository visitRepository,
    IOrganizationRepository organizations,
    ILogger<CustomersUpdateLastVisitHandler> logger
) : ICommandHandler<CustomersUpdateLastVisitCommand, IEnumerable<Customer>>
{
    
    
    public async Task<IEnumerable<Customer>> Handle(CustomersUpdateLastVisitCommand request, CancellationToken cancellationToken)
    {
        var org = await organizations.GetItemAsync(request.OrganizationId, cancellationToken);
        var result = new List<Customer>();
        foreach (var visit in request.CardVisits)
        {
            var card = visit.Card?.Split(",").MaxBy(x=>x.Length);
            if (string.IsNullOrEmpty(card))
                continue;
            var customer = await repository.GetCustomerByCardAsync(request.OrganizationId,card, cancellationToken);
            if (customer == null)
            {
                try
                {
                    customer = await CreateCustomer(org, card, cancellationToken);
                    await repository.AddAsync(customer);
                    customer.Context = new CoreCustomerContext(customer, visitRepository);
                    logger.LogInformation($"Created customer '{card}' in organization '{org.Name}'");
                }
                catch (Exception e)
                {
                    logger.LogError(e,$"Not success create customer with card '{card}' in organization '{org.Id}'");
                    continue;
                }
                
            }
            await customer.Context.UpdateAsync(
                visit.Visits.Select(v=>new CustomerVisit()
                {
                    CreatedAt = DateTimeOffset.UtcNow,
                    Customer = customer.Id,
                    Date = v.Date.ToUniversalTime(),
                    Sum = v.Sum
                }), cancellationToken);
            
            result.Add(customer);
        }
        return result;
    }
    
    private async Task<Customer> CreateCustomer(Organization org, string card, CancellationToken ct)
    {
        var bizCustomer = await org.CloudClient.GetCustomerAsync(
            new RequestCustomerInfo(org.TransportId, CustomerField.CardNumber)
            {
                CardNumber = card
            }, ct);
        
        var customer = new Customer(Guid.NewGuid(), 
            bizCustomer.Name, bizCustomer.Id, org.Id, true,
            string.Empty, string.Empty, string.Empty, string.Empty
        );
        customer.TryAddCard(card,card);
        foreach (var category in bizCustomer.Categories)
            customer.AddCategory(category.Id);
        
        customer.CreatedBiz = bizCustomer.WhenRegistered;
        
        return customer;
    }
}