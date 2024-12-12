using Microsoft.Extensions.Logging;
using OzonCard.Common.Application.Categories.Commands;
using OzonCard.Common.Core;
using OzonCard.Common.Core.Exceptions;
using OzonCard.Common.Worker.Services;
using OzonCard.DeferredRequest.Configuration;
using OzonCard.DeferredRequest.Configuration.Properties.Extensions;
using OzonCard.DeferredRequest.Handler;

namespace OzonCard.Customer.Api.Services.DeferredRequests;

public class CompaniesUnblockDeferredRequestHandler(
    ILogger<CompaniesUnblockDeferredRequestHandler> logger,
    IBackgroundJobsService jobsService
) : IDeferredRequestsHandler
{
    public string Key => DRequests.Companies.UnBlock;
    public string Name => "Разблокировать компанию";
    public IEnumerable<ExtensionProperty> Properties => GetProps();
    private IEnumerable<ExtensionProperty> GetProps() =>
        new List<ExtensionProperty>()
        {
            new ExtensionGuidProperty("organization", "Организация", PropertyBehaviour.OrganizationId),
            new ExtensionGuidProperty("category", "Компания", PropertyBehaviour.CategoryId),
            new ExtensionGuidProperty("selectedCategory", "Удалить из", PropertyBehaviour.CategoryId),
            
        };
    public Task<IBackgroundTask> AppendAsync(RequestConfiguration configuration, CancellationToken ct = default)
    {
        logger.LogDebug("Append new CompaniesUnblockDeferredRequestHandler");

        var cmd = new UpdateRangeCategoriesCommand(
            configuration.GetProperty<ExtensionGuidProperty>("organization").Value,
            configuration.GetProperty<ExtensionGuidProperty>("category").Value,
            configuration.GetProperty<ExtensionGuidProperty>("selectedCategory").Value,
            false,
            Guid.NewGuid(),
            configuration.TryGetFeature<Guid>("userId")
        );
        if (Guid.Empty == cmd.CategoryId)
            throw new BusinessException("Не выбрана компания");
        if (Guid.Empty == cmd.SelectedCategoryId)
            throw new BusinessException("Не выбран категория для разблокировки");
        return Task.FromResult(jobsService.Schedule(
            cmd, 
            configuration.ScheduleTimeZone, 
            cmd.Tracking,
            configuration.TryGetFeature<Guid>("userId")
        ));
    }
}