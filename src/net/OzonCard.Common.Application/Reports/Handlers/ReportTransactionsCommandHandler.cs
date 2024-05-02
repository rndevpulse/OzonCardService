using OzonCard.Biz.Client;
using OzonCard.Biz.Client.Models.Reports;
using OzonCard.Common.Application.Customers;
using OzonCard.Common.Application.Files;
using OzonCard.Common.Application.Organizations;
using OzonCard.Common.Application.Reports.Commands;
using OzonCard.Common.Application.Reports.Data;
using OzonCard.Common.Core;
using OzonCard.Common.Core.Exceptions;
using OzonCard.Common.Domain.Files;
using OzonCard.Common.Domain.Organizations;
using OzonCard.Excel;
using OzonCard.Excel.DataSets.TransactionsReport;
using OzonCard.Files;

namespace OzonCard.Common.Application.Reports.Handlers;

public class ReportTransactionsCommandHandler(
    IFileRepository fileRepository,
    IFileManager fileManager,
    IExcelManager excelManager,
    IOrganizationRepository orgRepository,
    ICustomerRepository customerRepository
) : ICommandHandler<ReportTransactionsCommand, SaveFile>
{
    public async Task<SaveFile> Handle(ReportTransactionsCommand request, CancellationToken cancellationToken)
    {
        var org = await orgRepository.GetItemAsync(request.OrganizationId, cancellationToken);
        if (org.Members.All(x => x.Name != request.User))
            throw new BusinessException($"Organization for '{request.User}' not found");
        if (org.Programs.All(x => x.Id != request.ProgramId))
            throw EntityNotFoundException.For<Program>(request.ProgramId, $"in org '{org.Name}'");

        var client = new BizClient(org.Login, org.Password);
        var transactions = await client.GetTransactionReport(
            org.Id, 
            request.DateFrom.LocalDateTime, 
            request.DateTo.LocalDateTime, 
            ct:cancellationToken);

        var report = await GetProgramReportAsync(
            client, org, request, cancellationToken);
        var customers = await customerRepository.GetItemsAsync(
            org.Id, cancellationToken);

        var transactionsReportTable =
            (from t in transactions
                join r in report on t.CardNumbers.Split(',').MaxBy(number => number.Length) equals r.GuestCardTrack
                join c in customers on r.GuestId equals c.BizId
                select new ItemTransactionsReportTable
                {
                    Created = t.CreateDate.ToUniversalTime(),
                    Date = t.CreateDate.ToUniversalTime().ToString("yyyy-MM-dd"),
                    Time = t.CreateDate.ToUniversalTime().ToString("HH:mm.ss"),
                    Name = c.Name,
                    TabNumber = c.TabNumber ?? "",
                    Division = string.IsNullOrEmpty(c.Position) ? c.Division : c.Position,
                    Categories = r.GuestCategoryNames,
                    Eating = GetNameEating(t.CreateDate.ToUniversalTime()),
                    Cards = r.GuestCardTrack,
                })
            .OrderByDescending(x=>x.Created)
            .ToList();
        var transactionsSummaryTable = transactionsReportTable
            .GroupBy(x => new { x.Name, x.Cards })
            .Select(x =>
            {
                var customer = x.First();
                return new ItemTransactionsSummaryTable
                {
                    Name = x.Key.Name,
                    Categories = customer.Categories,
                    Division = customer.Division,
                    CountDay = x.GroupBy(t => t.Date).Count()
                };
            })
            .OrderBy(x=>x.Name)
            .ToList();

        excelManager.CreateWorkbook(
            Path.Combine(fileManager.GetDirectory(), $"{request.TaskId}.xlsx"),
            new TransactionReportDataSet(
                transactionsReportTable, transactionsSummaryTable
            ),
            $"{request.Title} в период с {request.DateFrom.Date} по {request.DateTo.Date.AddSeconds(-1)}"
        );
        
        var saveFile = new SaveFile(request.TaskId, "xlsx", request.Title, request.UserId);
        await fileRepository.AddAsync(saveFile);
        return saveFile;
    }
    
    
    private string GetNameEating(DateTime date)
    {
        var time = date.TimeOfDay;
        if (time > new TimeSpan(3, 0, 0) && time <= new TimeSpan(11, 0, 0))
            return "Завтрак";
        if (time > new TimeSpan(11, 0, 0) && time <= new TimeSpan(16, 0, 0))
            return "Обед";
        if (time > new TimeSpan(16, 0, 0) && time <= new TimeSpan(21, 0, 0))
            return "Ужин";
        return "Ночной ужин";
    }


    private async Task<IEnumerable<ProgramReportDto>> GetProgramReportAsync(BizClient client,
        Organization org,
        ReportOption request, CancellationToken ct)
    {
        var report = await client.GetProgramReport(
            org.Id,
            request.ProgramId,
            request.DateFrom.LocalDateTime,
            request.DateTo.LocalDateTime.AddDays(-1),
            ct
        );
        var usedCategoryFilter = org.Categories
            .Where(x=>request.CategoriesId.Contains(x.Id))
            .Select(x=>x.Name)
            .ToArray();

        if (usedCategoryFilter.Length != 0)
        {
            report = report
                .Where(rowReport => usedCategoryFilter
                    .Any(c => rowReport.GuestCategoryNames.Contains(c)))
                .ToArray();
        }

        return report;
    }
}