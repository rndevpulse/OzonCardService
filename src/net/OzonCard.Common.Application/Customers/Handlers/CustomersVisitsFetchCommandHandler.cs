using Microsoft.Extensions.Configuration;
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
    IOrganizationRepository organizations,
    IConfiguration configuration
    ) : ICommandHandler<CustomersVisitsFetchCommand, object>
{
    public async Task<object> Handle(CustomersVisitsFetchCommand request, CancellationToken cancellationToken)
    {
        var period = 0 - configuration.GetValue("jobs:period", 5);
        var from = DateTime.Now.AddDays(period);
        var to = DateTime.Now;
        var offset = TimeSpan.FromMinutes(180);

        foreach (var org in await organizations.GetItemsAsync(cancellationToken))
        {
            try
            {
                logger.LogInformation($"CustomerLastVisitJob: for organization '{org.Name}' from '{from}' to '{to}'");
                var client = new BizClient(org.Login, org.Password);
                var shortReport = await client.GetShortCustomersReport(org.Id, from, to, cancellationToken);
                var customers = shortReport.Select(x => 
                        new CustomerInfoVisit(x.Id, x.Name, x.WhenCreated))
                    .ToArray();
                if (customers.Length == 0)
                    continue;
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
                    continue;
                var result = await commands.Send(new CustomersUpdateLastVisitCommand(org.Id, cardVisits, customers), cancellationToken);
                logger.LogInformation($"updated '{result.Count()}' customers in '{org.Name}' from '{from}' to '{to}'");
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex,"Fail update visits for organization '{Name}'", org.Name);
            }
        }

        return true;
    }
}