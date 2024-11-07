using Microsoft.Extensions.Logging;
using OzonCard.Biz.Client;
using OzonCard.Biz.Client.Models.Reports;
using OzonCard.Common.Application.Categories.Commands;
using OzonCard.Common.Application.Organizations;
using OzonCard.Common.Core;
using OzonCard.Common.Core.Exceptions;

namespace OzonCard.Common.Application.Categories.Handlers;

public class UpdateRangeCategoriesCommandHandler(
    IOrganizationRepository organizations,
    ILogger<UpdateRangeCategoriesCommandHandler> logger
) : ICommandHandler<UpdateRangeCategoriesCommand, int>
{
    public async Task<int> Handle(UpdateRangeCategoriesCommand request, CancellationToken cancellationToken)
    {
        var org = await organizations.GetItemAsync(request.OrganizationId, cancellationToken);
        var currentCategory = org.Categories.FirstOrDefault(x => x.Id == request.CategoryId);
        var newCategory = org.Categories.FirstOrDefault(x => x.Id == request.SelectedCategoryId);
        
        if (currentCategory is null || newCategory is null)
            throw new BusinessException($"Invalid category id in request with '{org.Name}'");
        logger.LogDebug($"Try update category '{newCategory.Name}' to '{currentCategory.Name}' in '{org.Name}'");
        var client = new BizClient(org.Login, org.Password);
        var customers = new List<Guid>();
        foreach (var program in org.Programs)
        {
            logger.LogDebug($"Search users in '{program.Name}' with program '{program.Name}'");

            var report = await client.GetProgramReport(
                org.Id,
                program.Id,
                DateTime.Now.AddMonths(-1),
                DateTime.Now,
                cancellationToken);
            customers.AddRange(
                report.Where(x => x.GuestCategoryNames.Contains(currentCategory.Name))
                    .Select(x=>x.GuestId)
            );
        }
        customers = customers.Distinct().ToList();
        logger.LogDebug("Find '{customerCount}' users in '{orgName}'", customers.Count, org.Name);
        foreach (var customer in customers)
        {
            try
            {
                if (request.IsAppend)
                    await client.AppendCategoryToCustomerAsync(customer, org.Id, newCategory.Id, cancellationToken);
                else
                    await client.RemoveCategoryToCustomerAsync(customer, org.Id, newCategory.Id, cancellationToken);
            }
            catch (Exception)
            {
                continue;
            }
            
        }

        return customers.Count;
    }

}