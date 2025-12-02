using Microsoft.Extensions.Logging;
using OzonCard.Cloud.Client.Data.Customers;
using OzonCard.Common.Application.Customers.Data;
using OzonCard.Common.Application.Customers.Queries;
using OzonCard.Common.Application.Organizations;
using OzonCard.Common.Core;
using OzonCard.Common.Core.Exceptions;
using OzonCard.Common.Domain.Organizations;
using Category = OzonCard.Common.Domain.Organizations.Category;

namespace OzonCard.Common.Application.Customers.Handlers;

public class CustomerSearchQueryHandler(
    IOrganizationRepository orgRepository,
    ICustomerRepository customerRepository,
    ILogger<CustomerSearchQueryHandler> logger
) : IQueryHandler<CustomersSearchQuery, IEnumerable<CustomerSearch>>
{
    public async Task<IEnumerable<CustomerSearch>> Handle(CustomersSearchQuery request, CancellationToken cancellationToken)
    {
        if (request is { Card: "", Name: "" })
            throw new BusinessException("Параметры поиска не заполнены");
        var org = await orgRepository.GetItemAsync(request.OrganizationId, cancellationToken);
        var program = org.Programs.FirstOrDefault(x => x.ProgramId == request.ProgramId)
                      ?? throw EntityNotFoundException.For<Program>(request.ProgramId, $"in org '{org.Name}'");
        var customers = await customerRepository.SearchCustomersAsync(
            org.Id, request.Name, request.Card, cancellationToken);
        if (!customers.Any())
            return ArraySegment<CustomerSearch>.Empty;
        
        
        var offset = TimeSpan.FromMinutes(request.Offset);
        var from = request.DateFrom.ToOffset(offset).Date;
        var to = request.DateTo.ToOffset(offset).Date.AddDays(1);
        
        logger.LogInformation($"Search customer for '{org.Name}' from '{from}' to '{to}' offset '{request.Offset}'");
        
        
        var result = customers.Select(async c => 
        {
            logger.LogInformation($"try get customer from biz: {c.BizId}");
            var bizCustomer = await org.CloudClient.GetCustomerAsync(
                new RequestCustomerInfo(org.TransportId, CustomerField.Id)
                {
                    Id = c.BizId,
                }, cancellationToken);
            logger.LogInformation($"customer success found from biz: {c.BizId}");

            // var rep = report.FirstOrDefault(r => r.GuestId == c.BizId);
            var visits = (await c.Context.GetVisitsAsync( from.ToUniversalTime(), to.ToUniversalTime(), cancellationToken)).ToList();
            // var shortRep = repTransactions.FirstOrDefault(r => r.Card?.Contains(request.Card) == true);
            return new CustomerSearch(
                c.Id, c.BizId, request.ProgramId, c.Name,
                string.Join(", ", c.Cards.Select(x => x.Number)),
                c.TabNumber ?? "", c.Position ?? "", c.Division ?? "",
                org.Name,
                bizCustomer.WalletBalances.FirstOrDefault(w => w.Id == program.WalletId)?.Balance,
                visits.Sum(v=>v.Sum),
                visits.Count,
                bizCustomer.Categories
                    .Where(cat=>cat.IsActive)
                    .Select(cat => new Category
                    {
                        CategoryId = cat.Id,
                        Name =  cat.Name
                    }),
                visits.GroupBy(t => t.Date.Date).Count(),
                // lastVisit?.Date,
                // lastVisit?.CreatedAt,
                c.LastVisit,
                c.UpdatedAt,
                // shortRep?.LastVisitDate.DateTime,
                // shortRep?.DaysGrant
                c.CreatedBiz
            );
        })
        .Select(t => t.Result)
        .ToList();
        
        return result;

    }
}