using Microsoft.Extensions.Logging;
using OzonCard.Biz.Client;
using OzonCard.Common.Application.Categories.Commands;
using OzonCard.Common.Application.Categories.Data;
using OzonCard.Common.Application.Organizations;
using OzonCard.Common.Core;
using OzonCard.Common.Core.Exceptions;
using OzonCard.Common.Worker.Services;

namespace OzonCard.Common.Application.Categories.Handlers;

public class UpdateRangeCategoriesCommandHandler(
    IOrganizationRepository organizations,
    ILogger<UpdateRangeCategoriesCommandHandler> logger,
    ITrackingBackgroundJobs tracking
) : ICommandHandler<UpdateRangeCategoriesCommand, int>
{
    private CategoriesTaskProgress _progress = new();
    public async Task<int> Handle(UpdateRangeCategoriesCommand request, CancellationToken cancellationToken)
    {
        var org = await organizations.GetItemAsync(request.OrganizationId, cancellationToken);
        var currentCategory = org.Categories.FirstOrDefault(x => x.Id == request.CategoryId);
        var newCategory = org.Categories.FirstOrDefault(x => x.Id == request.SelectedCategoryId);
        var task = request.Tracking is { } track
            ? await tracking.GetJobAsync(track, cancellationToken)
            : null;
        
        if (currentCategory is null || newCategory is null)
            throw new BusinessException($"Invalid category id in request with '{org.Name}'");
        logger.LogDebug($"Try update category '{newCategory.Name}' to '{currentCategory.Name}' in '{org.Name}'");
        var client = new BizClient(org.Login, org.Password);
        var customers = new List<Guid>();
        var dateFrom = DateTime.Now.AddMonths(-1);
        var dateTo = DateTime.Now.AddDays(1);
        _progress.AddLog($"Запрос отчетов с {dateFrom:dd.MM.yyyy} по {dateTo:dd.MM.yyyy} для нахождения гостей с требуемой категорией");
        tracking.ReportProgress(task, _progress);
        
        foreach (var program in org.Programs)
        {
            logger.LogDebug($"Search users in '{program.Name}' with program '{program.Name}'");
            try
            {
                var report = await client.GetProgramReport(
                    org.Id,
                    program.Id,
                    dateFrom,
                    dateTo,
                    cancellationToken);
                customers.AddRange(
                    report.Where(x => x.GuestCategoryNames.Contains(currentCategory.Name))
                        .Select(x=>x.GuestId)
                );
                _progress.AddLog($"Отчет по программе '{program.Name}': {customers.Count} искомых гостей");
                _progress.All = customers.Distinct().Count();
                tracking.ReportProgress(task, _progress);
            }
            catch (Exception e)
            {
                logger.LogWarning($"Report not get from biz for program '{program.Name}' in {org.Name}", e);
            }
            
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
            finally
            {
                _progress.Processed += 1;
                tracking.ReportProgress(task, _progress);
            } 
        }
        _progress.AddLog("Обработка завершена.");
        tracking.ReportProgress(task, _progress);
        return customers.Count;
    }

}