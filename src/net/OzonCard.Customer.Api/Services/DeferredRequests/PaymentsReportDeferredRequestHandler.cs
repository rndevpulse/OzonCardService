using Microsoft.Extensions.Logging;
using OzonCard.Common.Application.Reports.Commands;
using OzonCard.Common.Core;
using OzonCard.Common.Core.Exceptions;
using OzonCard.Common.Worker.Services;
using OzonCard.DeferredRequest.Configuration;
using OzonCard.DeferredRequest.Configuration.Properties.Extensions;
using OzonCard.DeferredRequest.Handler;

namespace OzonCard.Customer.Api.Services.DeferredRequests;


public class PaymentsReportDeferredRequestHandler(
    ILogger<PaymentsReportDeferredRequestHandler> logger,
    IBackgroundJobsService jobsService
) : IDeferredRequestsHandler
{
    public string Key => DRequests.Reports.Payments;
    public string Name => "Отчет за период";

    public IEnumerable<ExtensionProperty> Properties => GetProps();

    private IEnumerable<ExtensionProperty> GetProps() =>
        new List<ExtensionProperty>()
        {
            new ExtensionStringProperty("title", "Наименование"),
            new ExtensionGuidProperty("organization", "Организация", PropertyBehaviour.OrganizationId),
            new ExtensionGuidProperty("batch", "Групповой отчет", PropertyBehaviour.BatchId),
            new ExtensionGuidProperty("program", "Программа", PropertyBehaviour.ProgramId),
            new ExtensionDateTimeProperty("dateFrom", "Дата с", PropertyBehaviour.DateStart),
            new ExtensionDateTimeProperty("dateTo", "Дата по", PropertyBehaviour.DateEnd),
        };

    public Task<IBackgroundTask> AppendAsync(
        RequestConfiguration configuration,
        CancellationToken ct = default)
    {
        logger.LogDebug("Append new PaymentsReportDeferredRequestHandler");
        var cmd = new ReportPaymentsCommand()
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
        //TODO generate Exceptions
        if (string.IsNullOrWhiteSpace(cmd.Title))
            throw new BusinessException("Не указано наименование отчета");
        return Task.FromResult(jobsService.Schedule(cmd, configuration.ScheduleTimeZone, cmd.Tracking));
    }
}