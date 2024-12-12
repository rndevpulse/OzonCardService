using Microsoft.Extensions.Logging;
using OzonCard.Common.Application.Categories.Commands;
using OzonCard.Common.Core;
using OzonCard.Common.Core.Exceptions;
using OzonCard.Common.Worker.Services;
using OzonCard.DeferredRequest.Configuration;
using OzonCard.DeferredRequest.Configuration.Properties.Extensions;
using OzonCard.DeferredRequest.Handler;

namespace OzonCard.Customer.Api.Services.DeferredRequests;

public class AppendCategoriesDeferredRequestHandler(
    ILogger<AppendCategoriesDeferredRequestHandler> logger,
    IBackgroundJobsService jobsService
) : IDeferredRequestsHandler
{
    public string Key => DRequests.Categories.Append;
    public string Name => "Добавить категорию пользователям";
    public IEnumerable<ExtensionProperty> Properties => GetProps();
    private IEnumerable<ExtensionProperty> GetProps() =>
        new List<ExtensionProperty>()
        {
            new ExtensionGuidProperty("organization", "Организация", PropertyBehaviour.OrganizationId),
            new ExtensionGuidProperty("category", "Текущая категория", PropertyBehaviour.CategoryId),
            new ExtensionGuidProperty("selectedCategory", "Добавляемая категория", PropertyBehaviour.CategoryId),
            
        };
    public Task<IBackgroundTask> AppendAsync(RequestConfiguration configuration, CancellationToken ct = default)
    {
        logger.LogDebug("Append new AppendCategoriesDeferredRequestHandler");

        var cmd = new UpdateRangeCategoriesCommand(
            configuration.GetProperty<ExtensionGuidProperty>("organization").Value,
            configuration.GetProperty<ExtensionGuidProperty>("category").Value,
            configuration.GetProperty<ExtensionGuidProperty>("selectedCategory").Value,
            true,
            Guid.NewGuid(),
            configuration.TryGetFeature<Guid>("userId")
        );
        if (Guid.Empty == cmd.CategoryId)
            throw new BusinessException("Не выбрана текущая категория");
        if (Guid.Empty == cmd.SelectedCategoryId)
            throw new BusinessException("Не выбран категория для добавления");
        return Task.FromResult(jobsService.Schedule(
            cmd, 
            configuration.ScheduleTimeZone,
            cmd.Tracking,
            configuration.TryGetFeature<Guid>("userId")
        ));
    }
}