using OzonCard.Biz.Client;
using OzonCard.Common.Application.Customers.Data;
using OzonCard.Common.Application.Customers.Queries;
using OzonCard.Common.Application.Organizations;
using OzonCard.Common.Core;
using OzonCard.Common.Core.Exceptions;
using OzonCard.Common.Domain.Organizations;

namespace OzonCard.Common.Application.Customers.Handlers;

public class CustomerSearchQueryHandler(
    IOrganizationRepository orgRepository,
    ICustomerRepository customerRepository
) : IQueryHandler<CustomersSearchQuery, IEnumerable<CustomerSearch>>
{
    public async Task<IEnumerable<CustomerSearch>> Handle(CustomersSearchQuery request, CancellationToken cancellationToken)
    {
        var org = await orgRepository.GetItemAsync(request.OrganizationId, cancellationToken);
        var program = org.Programs.FirstOrDefault(x => x.Id == request.ProgramId)
                      ?? throw EntityNotFoundException.For<Program>(request.ProgramId, $"in org '{org.Name}'");
        var customers = await customerRepository.SearchCustomersAsync(
            org.Id, request.Name, request.Card, cancellationToken);
        if (!customers.Any())
            return ArraySegment<CustomerSearch>.Empty;
        
        var client = new BizClient(org.Login, org.Password);

        var report = await client.GetProgramReport(
            org.Id, request.ProgramId, 
            request.DateFrom, request.DateTo,
            cancellationToken); 
        var transactions =
            await client.GetTransactionReport(org.Id, request.DateFrom, request.DateTo, ct: cancellationToken);
        var offset = TimeSpan.FromMinutes(180);
        var repTransactions = transactions
            .GroupBy(x => x.CardNumbers)
            .Select(x=>new
            {
                Card = x.Key,
                LastVisitDate = x.Max(r=>r.CreateDate(offset)),
                DaysGrant = x.GroupBy(t => t.CreateDate(offset)).Count()
            });
        var walletId = program.Wallets.First().Id;
        
        var result = customers.Select(async c => 
        {
            var bizCustomer = await client.GetCustomerAsync(c.BizId, org.Id, cancellationToken);
            var rep = report.FirstOrDefault(r => r.GuestId == c.BizId);
            var shortRep = repTransactions.FirstOrDefault(r => r.Card == request.Card);
            return new CustomerSearch(
                c.Id, c.BizId, c.Name,
                rep?.GuestCardTrack ?? string.Join(", ", c.Cards.Select(x => x.Number)),
                c.TabNumber ?? "", c.Position ?? "", c.Division ?? "",
                org.Name,
                bizCustomer.WalletBalances.FirstOrDefault(w => w.Wallet.Id == walletId)?.Balance,
                rep?.PayFromWalletSum,
                rep?.PaidOrdersCount,
                bizCustomer.Categories.Select(cat => cat.Name),
                shortRep?.LastVisitDate.DateTime,
                shortRep?.DaysGrant
            );
        })
        .Select(t => t.Result)
        .ToList();
        
        return result;

    }
}