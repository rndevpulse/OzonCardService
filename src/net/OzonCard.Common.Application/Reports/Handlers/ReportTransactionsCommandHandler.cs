using Microsoft.Extensions.Logging;
using OzonCard.Common.Application.Common;
using OzonCard.Common.Application.Customers;
using OzonCard.Common.Application.Files;
using OzonCard.Common.Application.Organizations;
using OzonCard.Common.Application.Properties;
using OzonCard.Common.Application.Properties.Data;
using OzonCard.Common.Application.Reports.Commands;
using OzonCard.Common.Application.Reports.Data;
using OzonCard.Common.Core;
using OzonCard.Common.Core.Exceptions;
using OzonCard.Common.Domain.Files;
using OzonCard.Common.Domain.Organizations;
using OzonCard.Common.Domain.Props;
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
    IPropertiesRepository propertiesRepository,
    IEventBus events,
    ILogger<ReportTransactionsCommandHandler> logger
) : BaseCommandHandlerProgress(events), ICommandHandler<ReportTransactionsCommand, SaveFile>
{
    
    private Guid? _track;
    readonly ReportsTaskProgress _status = new();
    
    public async Task<SaveFile> Handle(ReportTransactionsCommand request, CancellationToken cancellationToken)
    {
        _track = request.Tracking;
        
        UpdateProgress("Собираем данные..", 3);
        
        var org = await orgRepository.GetItemAsync(request.OrganizationId, cancellationToken);
        if (org.Members.All(x => x.Name != request.User))
            throw new BusinessException($"Organization for '{request.User}' not found");
        if (org.Programs.All(x => x.ProgramId != request.ProgramId))
            throw EntityNotFoundException.For<Program>(request.ProgramId, $"in org '{org.Name}'");
        
        var offset = TimeSpan.FromMinutes(request.Offset);
        var from = request.DateFrom.ToOffset(offset).Date;
        var to = request.DateTo.ToOffset(offset).Date.AddDays(1).AddSeconds(-1);
        
        logger.LogInformation($"TransactionReport for '{org.Name}' from '{from:yyyy-MM-ddTHH:mm:ss}' to '{to:yyyy-MM-ddTHH:mm:ss}' offset '{request.Offset}'");
        UpdateProgress("Запрашиваем отчет по транзакциям..", 10);

        var response = await org.RmsClient.GetTransactionsReportAsync(
            from, to, org.PaymentName, cancellationToken);
       
        UpdateProgress("Запрашиваем отчет по программе питания..", 60);

        from = from.AddHours(-3);
        to = to.AddHours(-3);
        logger.LogInformation($"TransactionReport (ProgramReport) for '{org.Name}' from '{from:yyyy-MM-ddTHH:mm:ss}' to '{to:yyyy-MM-ddTHH:mm:ss}' offset '{request.Offset}'");

        var customers = await customerRepository.GetItemsAsync(
            org.Id, cancellationToken);
        
        UpdateProgress("Обрабатываем отчеты..", 80);

        var transactions = new List<ItemTransactionsReportTable>();
        foreach (var t in response.Data)
        {
            var customer = customers.FirstOrDefault(x => x.Cards.Any(c=>c.Number == t.Card));
            
            if (request.CategoriesId.Except(
                    customer?.Categories.Select(c => c.CategoryId) ?? []
                ).Any())
                continue;
            
            var categories = org.Categories.Where(x=>
                    customer?.Categories.Any(c=>c.CategoryId ==x.CategoryId) == true)
                .ToArray();
            transactions.Add(new ItemTransactionsReportTable
            {
                Created = t.CloseTime,
                Date = t.CloseTime.ToString("yyyy-MM-dd"),
                Time = t.CloseTime.ToString("HH:mm.ss"),
                Name = customer?.Name ?? t.Name,
                TabNumber = customer?.TabNumber ?? "",
                Division = customer?.Position ?? customer?.Division ?? "",
                Categories = string.Join(",", categories.Select(x => x.Name)),
                Eating = TimeOfDay.GetNameEating(t.CloseTime),
                Cards = t.Card,
            });
        }

        var transactionsSummaryTable = transactions
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
        
        var file = request.Batch == null 
                   || await propertiesRepository.GetItemAsync((Guid)request.Batch, cancellationToken) is not {} batch
            ? await SaveSimpleFileAsync(
                request, 
                new TransactionReportDataSet(transactions, transactionsSummaryTable), 
                cancellationToken)
            : await SaveBatchFilesAsync(org, batch, request, 
                transactions, transactionsSummaryTable,  cancellationToken);
        return file;
    }

    private async Task<SaveFile> SaveBatchFilesAsync(
        Organization organization,
        Property batch,
        ReportTransactionsCommand request,
        List<ItemTransactionsReportTable> transactions,
        List<ItemTransactionsSummaryTable> summary,
        CancellationToken ct)
    {
        UpdateProgress("Выполняется пакетное сохранение..", 90);
        
        var tempFolder = Path.Combine(fileManager.GetTempDirectory(), request.Title);
        var offset = TimeSpan.FromMinutes(request.Offset);
        var from = request.DateFrom.ToOffset(offset).Date;
        var to = request.DateTo.ToOffset(offset).Date.AddDays(1);
        
        //сохранить общий файл
        excelManager.CreateWorkbook(
            Path.Combine(tempFolder, $"{request.Title} - Общий.xlsx"),
            new TransactionReportDataSet(transactions, summary),
            $"{request.Title} - Общий: в период с {from} по {to.Date.AddSeconds(-1)}"
        );
        foreach (var batchProp in batch.GetProperty<IEnumerable<ReportBatchProp>>() ??
                                  ArraySegment<ReportBatchProp>.Empty)
        {
            var aggregationFilter = organization.Categories
                .Where(x => batchProp.Aggregations.Contains(x.CategoryId))
                .Select(x => x.Name)
                .ToArray();
            var aggregationTransactions = transactions.Where(x =>
                    aggregationFilter.Any(f => x.Categories.Contains(f)))
                .ToList();
            var aggregationSummary = summary.Where(x =>
                    aggregationFilter.Any(f => x.Categories.Contains(f)))
                .ToList();
            //сохраняем каждый батч отдельно
            excelManager.CreateWorkbook(
                Path.Combine(tempFolder, $"{request.Title} - {batchProp.Name}.xlsx"),
                new TransactionReportDataSet(aggregationTransactions, aggregationSummary),
                $"{request.Title} - {batchProp.Name}: в период с {from} по {to.Date.AddSeconds(-1)}"
            );
        }
        UpdateProgress("Упаковываем архив..", 98);
        //упаковываем все батчи в архив и кладем его в базу
        var fileId = await fileManager.SaveAsBatch(tempFolder);
        var saveFile = new SaveFile(fileId, "zip", request.Title, request.UserId);
        await fileRepository.AddAsync(saveFile);
        UpdateProgress("Пакет сохранен..", 100, saveFile);
        return saveFile;
    }

    private async Task<SaveFile> SaveSimpleFileAsync(ReportTransactionsCommand request, TransactionReportDataSet dataSet,
        CancellationToken ct)
    {
        UpdateProgress("Сохраняем результат..", 95);
        var offset = TimeSpan.FromMinutes(request.Offset);
        var to = request.DateTo.ToOffset(offset).Date.AddDays(1);
        var from = request.DateFrom.ToOffset(offset).Date;

        var fileId = Guid.NewGuid();
        excelManager.CreateWorkbook(
            Path.Combine(fileManager.GetDirectory(), $"{fileId}.xlsx"),
            dataSet,
            $"{request.Title} в период с {from} по {to.Date.AddSeconds(-1)}"
        );
        
        var saveFile = new SaveFile(fileId, "xlsx", request.Title, request.UserId);
        await fileRepository.AddAsync(saveFile);

        UpdateProgress("Отчет сохранен..", 100, saveFile);

        return saveFile;
    }

    private void UpdateProgress(string description, int n, SaveFile? result = null)
    {
        ReportProgress(_track, _status with
        {
            Description = description,
            Progress = n
        }, result);
    }
}