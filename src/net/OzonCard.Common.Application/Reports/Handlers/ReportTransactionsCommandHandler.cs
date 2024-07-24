using Microsoft.Extensions.Logging;
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
using OzonCard.Common.Worker.Data;
using OzonCard.Common.Worker.Services;
using OzonCard.Excel;
using OzonCard.Excel.DataSets.TransactionsReport;
using OzonCard.Files;

namespace OzonCard.Common.Application.Reports.Handlers;

public class ReportTransactionsCommandHandler(
    IFileRepository fileRepository,
    IFileManager fileManager,
    IExcelManager excelManager,
    IOrganizationRepository orgRepository,
    ICustomerRepository customerRepository,
    ITrackingBackgroundJobs tracking, 
    ILogger<ReportTransactionsCommandHandler> logger
) : ICommandHandler<ReportTransactionsCommand, SaveFile>
{
    
    private IJobProgress? _task;
    readonly ReportsTaskProgress _status = new();
    
    public async Task<SaveFile> Handle(ReportTransactionsCommand request, CancellationToken cancellationToken)
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
        
        logger.LogInformation($"TransactionReport for '{org.Name}' from '{from}' to '{to}' offset '{request.Offset}'");
        UpdateProgress("Запрашиваем отчет по транзакциям..", 10);

        var transactions = await client.GetTransactionReport(
            org.Id, 
            from, 
            to.AddDays(-1), 
            ct:cancellationToken);
        UpdateProgress("Запрашиваем отчет по программе питания..", 60);

        var report = await GetProgramReportAsync(
            client, org, request.CategoriesId, request.ProgramId, from, to, cancellationToken);
        var customers = await customerRepository.GetItemsAsync(
            org.Id, cancellationToken);
        
        UpdateProgress("Обрабатываем отчеты..", 80);

        var transactionsReportTable =
            (from t in transactions
                join r in report on t.CardNumbers.Split(',').MaxBy(number => number.Length) equals r.GuestCardTrack
                join c in customers on r.GuestId equals c.BizId
                select new ItemTransactionsReportTable
                {
                    Created = t.CreateDate(offset).DateTime,
                    Date = t.CreateDate(offset).ToString("yyyy-MM-dd"),
                    Time = t.CreateDate(offset).ToString("HH:mm.ss"),
                    Name = c.Name,
                    TabNumber = c.TabNumber ?? "",
                    Division = string.IsNullOrEmpty(c.Position) ? c.Division : c.Position,
                    Categories = r.GuestCategoryNames,
                    Eating = TimeOfDay.GetNameEating(t.CreateDate(offset).DateTime),
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

        UpdateProgress("Сохраняем результат..", 95);

        var fileId = Guid.NewGuid();
        excelManager.CreateWorkbook(
            Path.Combine(fileManager.GetDirectory(), $"{fileId}.xlsx"),
            new TransactionReportDataSet(
                transactionsReportTable, transactionsSummaryTable
            ),
            $"{request.Title} в период с {from} по {to.Date.AddSeconds(-1)}"
        );
        
        var saveFile = new SaveFile(fileId, "xlsx", request.Title, request.UserId);
        await fileRepository.AddAsync(saveFile);

        UpdateProgress("Отчет сохранен..", 100);

        return saveFile;
    }

    private async Task<IEnumerable<ProgramReportDto>> GetProgramReportAsync(BizClient client,
        Organization org, IEnumerable<Guid> categoriesId,
        Guid programId,
        DateTime from, DateTime to, CancellationToken ct)
    {
        var report = await client.GetProgramReport(
            org.Id,
            programId,
            from,
            to,
            ct
        );
        var usedCategoryFilter = org.Categories
            .Where(x=>categoriesId.Contains(x.Id))
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
    
    private void UpdateProgress(string description, int n)
    {
        tracking.ReportProgress(_task, _status with
        {
            Description = description,
            Progress = n
        });
    }
}