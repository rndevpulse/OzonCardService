using Microsoft.Extensions.Logging;
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
    ICustomerRepository customerRepository,
    ILogger<CustomerSearchQueryHandler> logger
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
        
        var offset = TimeSpan.FromMinutes(request.Offset);
        var from = request.DateFrom.ToOffset(offset).Date;
        var to = request.DateTo.ToOffset(offset).Date.AddDays(1);
        logger.LogInformation($"Search customer for '{org.Name}' from '{from}' to '{to}' offset '{request.Offset}'");

        // var report = await client.GetProgramReport(
        //     org.Id, request.ProgramId, 
        //     from, to,
        //     cancellationToken); 
        // var transactions =
        //     await client.GetTransactionReport(org.Id, from, to.AddDays(-1), ct: cancellationToken);
        // var repTransactions = transactions
        //     .GroupBy(x => x.CardNumbers)
        //     .Select(x=>new CustomerVisit(
        //         x.Key,
        //         x.Max(r=>r.CreateDate(offset)),
        //         x.GroupBy(t => t.CreateDate(offset)).Count())
        //     );
        var walletId = program.Wallets.First().Id;
        
        var result = customers.Select(async c => 
        {
            var bizCustomer = await client.GetCustomerAsync(c.BizId, org.Id, cancellationToken);
            // var rep = report.FirstOrDefault(r => r.GuestId == c.BizId);
            var visits = (await c.Context.GetVisitsAsync(from, to, cancellationToken)).ToList();
            // var shortRep = repTransactions.FirstOrDefault(r => r.Card?.Contains(request.Card) == true);
            return new CustomerSearch(
                c.Id, c.BizId, request.ProgramId, c.Name,
                string.Join(", ", c.Cards.Select(x => x.Number)),
                c.TabNumber ?? "", c.Position ?? "", c.Division ?? "",
                org.Name,
                bizCustomer.WalletBalances.FirstOrDefault(w => w.Wallet.Id == walletId)?.Balance,
                visits.Sum(v=>v.Sum),
                visits.Count,
                bizCustomer.Categories
                    .Where(cat=>cat.IsActive)
                    .Select(cat => new Category(cat.Id){Name =  cat.Name}),
                visits.Max(r => r.Date).Date,
                visits.GroupBy(t => t).Count()
                // shortRep?.LastVisitDate.DateTime,
                // shortRep?.DaysGrant
            );
        })
        .Select(t => t.Result)
        .ToList();
        
        return result;

    }
}