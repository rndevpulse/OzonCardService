using Microsoft.Extensions.Logging;
using OzonCard.Biz.Client;
using OzonCard.Common.Application.Categories.Commands;
using OzonCard.Common.Application.Categories.Data;
using OzonCard.Common.Application.Common;
using OzonCard.Common.Application.Files;
using OzonCard.Common.Application.Organizations;
using OzonCard.Common.Core;
using OzonCard.Common.Core.Exceptions;
using OzonCard.Common.Domain.Files;
using OzonCard.Common.Worker.Services;
using OzonCard.Files;

namespace OzonCard.Common.Application.Categories.Handlers;

public class UpdateRangeCategoriesCommandHandler(
    IOrganizationRepository organizations,
    IFileRepository fileRepository,
    IFileManager fileManager,
    ILogger<UpdateRangeCategoriesCommandHandler> logger,
    IEventBus events
) : BaseCommandHandlerProgress(events), ICommandHandler<UpdateRangeCategoriesCommand, SaveFile>
{
    private CategoriesTaskProgress _progress = new();
    public async Task<SaveFile> Handle(UpdateRangeCategoriesCommand request, CancellationToken cancellationToken)
    {
        var org = await organizations.GetItemAsync(request.OrganizationId, cancellationToken);
        var currentCategory = org.Categories.FirstOrDefault(x => x.Id == request.CategoryId);
        var newCategory = org.Categories.FirstOrDefault(x => x.Id == request.SelectedCategoryId);
       
        
        if (currentCategory is null || newCategory is null)
            throw new BusinessException($"Invalid category id in request with '{org.Name}'");
        logger.LogDebug($"Try update category '{newCategory.Name}' to '{currentCategory.Name}' in '{org.Name}'");
        var client = new BizClient(org.Login, org.Password);
        var customers = new List<Guid>();
        var dateFrom = DateTime.Now.AddMonths(-1);
        var dateTo = DateTime.Now.AddDays(1);
        _progress.AddLog($"Запрос отчетов с {dateFrom:dd.MM.yyyy} по {dateTo:dd.MM.yyyy} для нахождения гостей с требуемой категорией");
        ReportProgress(request.Tracking, _progress);
        
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
                ReportProgress(request.Tracking, _progress);
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
                ReportProgress(request.Tracking, _progress);
            } 
        }
        _progress.AddLog("Обработка завершена.");
        
        
        var action = request.IsAppend ? "Добавление в {0} для {1}" : "Удаление из {0} для {1}";
        var fileName = string.Format(action, newCategory.Name, currentCategory.Name);
        
        await using var ms = new MemoryStream();
        await using var sw = new StreamWriter(ms);
        await sw.WriteAsync(_progress.Log);
        await sw.FlushAsync(cancellationToken);
        ms.Position = 0;
        var fileId = await fileManager.Save(ms, $"{fileName}.txt");
        var saveFile = new SaveFile(
            fileId,
            "txt",
            fileName,
            request.UserId);
        await fileRepository.AddAsync(saveFile);
        ReportProgress(request.Tracking, _progress, saveFile);
        return saveFile;
    }

}