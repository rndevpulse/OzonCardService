using Microsoft.Extensions.Logging;
using OzonCard.Biz.Client;
using OzonCard.Common.Application.Customers.Commands;
using OzonCard.Common.Application.Organizations;
using OzonCard.Common.Application.Visits;
using OzonCard.Common.Core;
using OzonCard.Common.Domain.Customers;
using OzonCard.Common.Domain.Organizations;

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
        var client = new BizClient(org.Login, org.Password);
        var customers = await repository.GetItemsAsync(request.OrganizationId, cancellationToken);
        var result = new List<Customer>();
        foreach (var visit in request.CardVisits)
        {
            var card = visit.Card?.Split(",").MaxBy(x=>x.Length);
            if (string.IsNullOrEmpty(card))
                continue;
            var customer = customers.FirstOrDefault(x => x.Cards.Any(c => c.Number == card));
            if (customer == null)
            {
                try
                {
                    customer = await CreateCustomer(client, org, card, cancellationToken);
                    await repository.AddAsync(customer);
                    customer.Context = new CoreCustomerContext(customer, visitRepository);
                }
                catch (Exception e)
                {
                    logger.LogError(e,$"Not success create customer with card '{card}' in organization '{org.Id}': {client.Reason}");
                    continue;
                }
                
            }
            // customer.LastVisit = visit.LastVisitDate;
            await customer.Context.UpdateAsync(
                visit.Visits.Select(v=>new CustomerVisit()
                {
                    CreatedAt = DateTimeOffset.UtcNow,
                    Customer = customer.Id,
                    Date = v.Date,
                    Sum = v.Sum
                }), cancellationToken);
            
            if (customer.CreatedBiz == null
                && request.Customers.FirstOrDefault(c => c.Id == customer.BizId) is { } visitInfo)
                customer.CreatedBiz = visitInfo.CreatedAt;
            
            result.Add(customer);
        }
        return result;
    }
    
    private async Task<Customer> CreateCustomer(BizClient client, Organization org, string card, CancellationToken ct)
    {
        var bizCustomer = await client.GetCustomerAsync(card, org.Id, ct);
        
        var customer = new Customer(Guid.NewGuid(), 
            bizCustomer.Name, bizCustomer.Id, org.Id, true,
            string.Empty, string.Empty, string.Empty, string.Empty
        );
        customer.TryAddCard(card,card);
        foreach (var valletDto in bizCustomer.WalletBalances)
        {
            var program = org.Programs.FirstOrDefault(x => x.Name == valletDto.Wallet.Name);
            if (program == null)
                continue;
            var wallet = program.Wallets.First();
            customer.TryAddWallet(wallet.Id, wallet.Name, wallet.ProgramType, wallet.Type);
        }
        return customer;
    }
}