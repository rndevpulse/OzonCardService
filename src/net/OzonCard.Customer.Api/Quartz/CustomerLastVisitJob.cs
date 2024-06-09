using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OzonCard.Biz.Client;
using OzonCard.Common.Application.Customers.Commands;
using OzonCard.Common.Application.Customers.Data;
using OzonCard.Common.Application.Organizations;
using OzonCard.Common.Core;
using Quartz;

namespace OzonCard.Customer.Api.Quartz;

public class CustomerLastVisitJob(
    ILogger<CustomerLastVisitJob> logger,
    ICommandBus commands,
    IOrganizationRepository organizations,
    IConfiguration configuration
) : IJob
{
    
    public async Task Execute(IJobExecutionContext context)
    {
        var period = 0 - configuration.GetValue("jobs:period", 5);
        var from = DateTime.Now.AddDays(period);
        var to = DateTime.Now;
        var offset = TimeSpan.FromMinutes(180);

        foreach (var org in await organizations.GetItemsAsync())
        {
            logger.LogInformation($"CustomerLastVisitJob: for organization '{org.Name}' from '{from}' to '{to}'");
            var client = new BizClient(org.Login, org.Password);
            var transactions = await client.GetTransactionReport(org.Id, from, to);
            var customers = transactions
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
            if (customers.Length == 0)
                continue;
            var result = await commands.Send(new CustomersUpdateLastVisitCommand(org.Id, customers));
            logger.LogInformation($"updated '{result.Count()}' customers in '{org.Name}' from '{from}' to '{to}'");

        }
    }
}