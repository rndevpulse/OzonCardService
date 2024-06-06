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
    IOrganizationRepository organizations
) : ICommandHandler<CustomersUpdateLastVisitCommand, IEnumerable<Customer>>
{
    
    
    public async Task<IEnumerable<Customer>> Handle(CustomersUpdateLastVisitCommand request, CancellationToken cancellationToken)
    {
        var org = await organizations.GetItemAsync(request.OrganizationId, cancellationToken);
        var client = new BizClient(org.Login, org.Password);
        var customers = await repository.GetItemsAsync(request.OrganizationId, cancellationToken);
        var result = new List<Customer>();
        foreach (var visit in request.CustomersVisit)
        {
            if (string.IsNullOrEmpty(visit.Card))
                continue;
            var customer = customers.FirstOrDefault(x => x.Cards.Any(c => c.Number == visit.Card));
            if (customer == null)
            {
                customer = await TryCreateCustomer(client, org, visit.Card, cancellationToken);
                await repository.AddAsync(customer);
                customer.Context = new CoreCustomerContext(customer, visitRepository);
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
            result.Add(customer);
        }
        return result;
    }
    
    private async Task<Customer?> TryCreateCustomer(BizClient client, Organization org, string card, CancellationToken ct)
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