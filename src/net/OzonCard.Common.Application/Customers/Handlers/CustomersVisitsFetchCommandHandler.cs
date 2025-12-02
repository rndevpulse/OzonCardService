using Microsoft.Extensions.Logging;
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
        
        var from = DateTime.Now;
        var to = DateTime.Now.AddDays(1);
        // var offset = TimeSpan.FromMinutes(180);
        var org = await organizations.GetItemAsync(request.OrgId, cancellationToken);

       
        try
        {
            logger.LogInformation($"CustomerLastVisitJob: for organization '{org.Name}' from '{from}' to '{to}'");
            
            var report = await org.RmsClient.GetTransactionsReportAsync(from, to, org.PaymentName, cancellationToken);
            // var customers = shortReport.Select(x => 
            //         new CustomerInfoVisit(x.Id, x.Name, x.WhenCreated))
            //     .ToArray();
            // if (customers.Length == 0)
            //     return new SynchronizeResult();
            
            
            var cardVisits = report.Data
                .GroupBy(x => x.Card)
                .Select(x => new CardVisit(
                    x.Key,
                    x.Select(r => 
                        new CardVisitTransaction(
                            r.CloseTime, 
                            r.Sum)
                    ).ToArray()
                ))
                .ToArray();
            if (cardVisits.Length == 0)
                return new SynchronizeResult();
            var result = await commands.Send(new CustomersUpdateLastVisitCommand(org.Id, cardVisits), cancellationToken);
            logger.LogInformation($"updated '{result.Count()}' customers in '{org.Name}' from '{from}' to '{to}'");
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex,"Fail update visits for organization '{Name}'", org.Name);
        }

        return new SynchronizeResult();
    }
}