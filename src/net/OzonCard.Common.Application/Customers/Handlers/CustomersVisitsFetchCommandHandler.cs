using Microsoft.Extensions.Logging;
using OzonCard.Biz.Client;
using OzonCard.Common.Application.Customers.Commands;
using OzonCard.Common.Application.Customers.Data;
using OzonCard.Common.Application.Organizations;
using OzonCard.Common.Core;

namespace OzonCard.Common.Application.Customers.Handlers;

public class CustomersVisitsFetchCommandHandler(
    ILogger<CustomersVisitsFetchCommandHandler> logger,
    ICommandBus commands,
    IOrganizationRepository organizations
    ) : ICommandHandler<CustomersVisitsFetchCommand, SynchronizeResult>
{
    public async Task<SynchronizeResult> Handle(CustomersVisitsFetchCommand request, CancellationToken cancellationToken)
    {
        
        var from = DateTime.Now.AddDays(0 - request.Days);
        var to = DateTime.Now;
        var offset = TimeSpan.FromMinutes(180);
        var org = await organizations.GetItemAsync(request.OrgId, cancellationToken);

       
        try
        {
            logger.LogInformation($"CustomerLastVisitJob: for organization '{org.Name}' from '{from}' to '{to}'");
            var client = new BizClient(org.Login, org.Password);
            var shortReport = await client.GetShortCustomersReport(org.Id, from, to, cancellationToken);
            var customers = shortReport.Select(x => 
                    new CustomerInfoVisit(x.Id, x.Name, x.WhenCreated))
                .ToArray();
            if (customers.Length == 0)
                return new SynchronizeResult();
            var transactions = await client.GetTransactionReport(org.Id, from, to, ct:cancellationToken);
            var cardVisits = transactions
                .GroupBy(x => x.CardNumbers)
                .Select(x => new CardVisit(
                    x.Key,
                    x.Select(r => 
                        new CardVisitTransaction(
                            r.CreateDate(offset), 
                            r.TransactionSum ?? 0)
                    ).ToArray()
                ))
                .ToArray();
            if (cardVisits.Length == 0)
                return new SynchronizeResult();
            var result = await commands.Send(new CustomersUpdateLastVisitCommand(org.Id, cardVisits, customers), cancellationToken);
            logger.LogInformation($"updated '{result.Count()}' customers in '{org.Name}' from '{from}' to '{to}'");
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex,"Fail update visits for organization '{Name}'", org.Name);
        }

        return new SynchronizeResult();
    }
}