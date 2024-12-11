using Microsoft.Extensions.Logging;
using OzonCard.Common.Application.Reports.Commands;
using OzonCard.Common.Core;
using OzonCard.Common.Core.Exceptions;
using OzonCard.Common.Worker.Services;
using OzonCard.DeferredRequest.Configuration;
using OzonCard.DeferredRequest.Configuration.Properties.Extensions;
using OzonCard.DeferredRequest.Handler;

namespace OzonCard.Customer.Api.Services.DeferredRequests;


public class TransactionsReportDeferredRequestHandler(
    ILogger<TransactionsReportDeferredRequestHandler> logger,
    IBackgroundJobsService jobsService
) : IDeferredRequestsHandler
{
    public string Key => DRequests.Reports.Transactions;
    public string Name => "Отчет по операциям";

    public IEnumerable<ExtensionProperty> Properties => GetProps();

    private IEnumerable<ExtensionProperty> GetProps() =>
        new List<ExtensionProperty>()
        {
            new ExtensionStringProperty("title", "Наименование"){Value = "Отчет по операциям"},
            new ExtensionGuidProperty("organization", "Организация", PropertyBehaviour.OrganizationId),
            new ExtensionGuidProperty("batch", "Групповой отчет", PropertyBehaviour.BatchId),
            new ExtensionGuidProperty("program", "Программа", PropertyBehaviour.ProgramId),
            new ExtensionDateTimeProperty("dateFrom", "Дата с", PropertyBehaviour.DateStart) { Value = DateTimeOffset.Now.AddDays(-DateTimeOffset.Now.Day + 1)},
            new ExtensionDateTimeProperty("dateTo", "Дата по", PropertyBehaviour.DateEnd) { Value = DateTimeOffset.Now.AddMonths(1).AddDays(-DateTimeOffset.Now.Day)},
        };

    public Task<IBackgroundTask> AppendAsync(
        RequestConfiguration configuration,
        CancellationToken ct = default)
    {
        logger.LogDebug("Append new TransactionsReportDeferredRequestHandler");
        var cmd = new ReportTransactionsCommand()
        {
            Title = configuration.GetProperty<ExtensionStringProperty>("title").Value,
            OrganizationId = configuration.GetProperty<ExtensionGuidProperty>("organization").Value,
            ProgramId = configuration.GetProperty<ExtensionGuidProperty>("program").Value,
            Batch = configuration.GetProperty<ExtensionGuidProperty>("batch").Value,
            DateFrom = configuration.GetProperty<ExtensionDateTimeProperty>("dateFrom").Value,
            DateTo = configuration.GetProperty<ExtensionDateTimeProperty>("dateTo").Value,
            Offset = configuration.TimeOffset,
            
            UserId = configuration.TryGetFeature<Guid>("userId") is var userId ? userId : Guid.Empty, 
            User = configuration.TryGetFeature<string>("user") is var user ? user ?? string.Empty : string.Empty,
            Tracking = Guid.NewGuid(),
        };
        if (string.IsNullOrWhiteSpace(cmd.Title))
            throw new BusinessException("Не указано наименование отчета");
        if (Guid.Empty == cmd.ProgramId)
            throw new BusinessException("Не выбрана маркетинговая программа");
        if (Guid.Empty == cmd.Batch)
            throw new BusinessException("Не выбран шаблон для сохранения отчетов");

        return Task.FromResult(jobsService.Schedule(
            cmd,
            configuration.ScheduleTimeZone,
            cmd.Tracking,
            cmd.UserId
        ));
    }
}