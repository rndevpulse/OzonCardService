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
    IOrganizationRepository organizations
) : IJob
{
    
    public async Task Execute(IJobExecutionContext context)
    {
        var from = DateTime.Now.AddDays(-5);
        var to = DateTime.Now;
        var offset = TimeSpan.FromMinutes(180);

        foreach (var org in await organizations.GetItemsAsync())
        {
            logger.LogInformation($"CustomerLastVisitJob: for organization '{org.Name}' from '{from}' to '{to}'");
            var client = new BizClient(org.Login, org.Password);
            var transactions = await client.GetTransactionReport(org.Id, from, to);
            var customers = transactions
                .GroupBy(x => x.CardNumbers)
                .Select(x => new CustomerVisit(
                    x.Key,
                    x.Max(r => r.CreateDate(offset)),
                    x.GroupBy(t => t.CreateDate(offset)).Count())
                );
            var result = await commands.Send(new CustomersUpdateLastVisit(org.Id, customers));
            logger.LogInformation($"updated '{result.Count()}' customers in '{org.Name}' from '{from}' to '{to}'");

        }
    }
}