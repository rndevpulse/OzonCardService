using Microsoft.Extensions.Logging;
using OzonCard.Biz.Client;
using OzonCard.Common.Application.Customers;
using OzonCard.Common.Application.Files;
using OzonCard.Common.Application.Organizations;
using OzonCard.Common.Application.Reports.Commands;
using OzonCard.Common.Core;
using OzonCard.Common.Core.Exceptions;
using OzonCard.Common.Domain.Files;
using OzonCard.Common.Domain.Organizations;
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
    ILogger<ReportPaymentsCommandHandler> logger
) : ICommandHandler<ReportPaymentsCommand, SaveFile>
{
    public async Task<SaveFile> Handle(ReportPaymentsCommand request, CancellationToken cancellationToken)
    {
        var org = await orgRepository.GetItemAsync(request.OrganizationId, cancellationToken);
        if (org.Members.All(x => x.Name != request.User))
            throw new BusinessException($"Organization for '{request.User}' not found");
        if (org.Programs.All(x => x.Id != request.ProgramId))
            throw EntityNotFoundException.For<Program>(request.ProgramId, $"in org '{org.Name}'");

        var client = new BizClient(org.Login, org.Password);
        var offset = TimeSpan.FromMinutes(request.Offset);
        var from = request.DateFrom.ToOffset(offset).Date;
        var to = request.DateTo.ToOffset(offset).Date.AddDays(1);
        var report = await client.GetProgramReport(
            org.Id,
            request.ProgramId,
            from,
            to,
            cancellationToken
        );
        logger.LogInformation($"Payment report for '{org.Name}' from '{from}' to '{to}' returned '{report.Count()}' rows");
        var customers = await customerRepository.GetItemsAsync(org.Id, cancellationToken);
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
        
        
        
        excelManager.CreateWorkbook(
            Path.Combine(fileManager.GetDirectory(), $"{request.TaskId}.xlsx"),
            new ProgramReportDataSet(resultReport.OrderBy(x=>x.Name)),
            $"{request.Title} в период с {request.DateFrom.Date} по {request.DateTo.Date.AddSeconds(-1)}"
            );
        
        var saveFile = new SaveFile(request.TaskId, "xlsx", request.Title, request.UserId);
        await fileRepository.AddAsync(saveFile);
        return saveFile;
    }
}