using Microsoft.Extensions.Logging;
using OzonCard.Biz.Client;
using OzonCard.Common.Application.Customers;
using OzonCard.Common.Application.Files;
using OzonCard.Common.Application.Organizations;
using OzonCard.Common.Application.Reports.Commands;
using OzonCard.Common.Application.Reports.Data;
using OzonCard.Common.Core;
using OzonCard.Common.Core.Exceptions;
using OzonCard.Common.Domain.Files;
using OzonCard.Common.Domain.Organizations;
using OzonCard.Common.Worker.Data;
using OzonCard.Common.Worker.Services;
using OzonCard.Excel;
using OzonCard.Excel.DataSets.ProgramsReport;
using OzonCard.Files;

namespace OzonCard.Common.Application.Reports.Handlers;

public class ReportPaymentsCommandHandler(
    IFileRepository fileRepository,
    IFileManager fileManager,
    IExcelManager excelManager,
    IOrganizationRepository orgRepository,
    ICustomerRepository customerRepository,
    ITrackingBackgroundJobs tracking, 
    ILogger<ReportPaymentsCommandHandler> logger
) : ICommandHandler<ReportPaymentsCommand, SaveFile>
{
    private IJobProgress? _task;
    readonly ReportsTaskProgress _status = new();
    public async Task<SaveFile> Handle(ReportPaymentsCommand request, CancellationToken cancellationToken)
    {
        _task = request.Tracking is { } track
            ? await tracking.GetJobAsync(track, cancellationToken)
            : null;
        
        UpdateProgress("Собираем данные..", 3);
        
        var org = await orgRepository.GetItemAsync(request.OrganizationId, cancellationToken);
        if (org.Members.All(x => x.Name != request.User))
            throw new BusinessException($"Organization for '{request.User}' not found");
        if (org.Programs.All(x => x.Id != request.ProgramId))
            throw EntityNotFoundException.For<Program>(request.ProgramId, $"in org '{org.Name}'");

        var client = new BizClient(org.Login, org.Password);
        var offset = TimeSpan.FromMinutes(request.Offset);
        var from = request.DateFrom.ToOffset(offset).Date;
        var to = request.DateTo.ToOffset(offset).Date.AddDays(1);
        
        UpdateProgress("Запрашиваем отчет по программе питания..", 10);
        
        var report = await client.GetProgramReport(
            org.Id,
            request.ProgramId,
            from,
            to,
            cancellationToken
        );
        logger.LogInformation($"Payment report for '{org.Name}' from '{from}' to '{to}' returned '{report.Count()}' rows");
        
        UpdateProgress("Обрабатываем отчет по программе питания..", 65);
        
        var customers = await customerRepository.GetItemsAsync(org.Id, cancellationToken);
       
        UpdateProgress("Фильтруем и собираем результат..", 80);

        var usedCategoryFilter = org.Categories
            .Where(x=>request.CategoriesId.Contains(x.Id))
            .Select(x=>x.Name)
            .ToArray();
        var resultReport = new List<ItemProgramReportTable>();
        

        foreach (var rowReport in report)
        {
            if (rowReport.PaidOrdersCount == 0)
                continue;
            //include filter category
            if (usedCategoryFilter.Length != 0
                && usedCategoryFilter.Any(c=>!rowReport.GuestCategoryNames.Contains(c)))
                continue;
            
            var customer = customers.FirstOrDefault(x => x.BizId == rowReport.GuestId);
            resultReport.Add(new ItemProgramReportTable()
            {
                Name = rowReport.GuestName,
                Card = rowReport.GuestCardTrack,
                Categories = rowReport.GuestCategoryNames,
                TabNumber = customer?.TabNumber ?? "",
                Position = customer?.Position ?? "",
                PaidOrders = rowReport.PaidOrdersCount,
            });
        }

        UpdateProgress("Сохраняем результат..", 95);


        var fileId = Guid.NewGuid();
        excelManager.CreateWorkbook(
            Path.Combine(fileManager.GetDirectory(), $"{fileId}.xlsx"),
            new ProgramReportDataSet(resultReport.OrderBy(x=>x.Name)),
            $"{request.Title} в период с {request.DateFrom.Date} по {to.Date.AddSeconds(-1)}"
            );
        
        var saveFile = new SaveFile(fileId, "xlsx", request.Title, request.UserId);
        await fileRepository.AddAsync(saveFile);
        
        UpdateProgress("Отчет сохранен..", 100);

        return saveFile;
    }


    private void UpdateProgress(string description, int n)
    {
        tracking.ReportProgress(_task, _status with
        {
            Description = description,
            Progress = n
        });
    }
}