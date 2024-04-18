using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OzonCard.Common.Application.BackgroundTasks;
using OzonCard.Common.Infrastructure.BackgroundTasks;
using OzonCard.Customer.Api.Models.BackgroundTask;
using OzonCard.Customer.Api.Services.BackgroundTasks;

namespace OzonCard.Customer.Api.Controllers;

public class TasksController(
    ILogger<TasksController> logger,
    IBackgroundQueue queue
) : ApiController
{

    [HttpGet]
    public IEnumerable<BackgroundTaskModel> Index([FromQuery] IEnumerable<Guid> id, CancellationToken ct = default)
    {
        logger.LogDebug("Get tasks:");
        return Mapper.Map<IEnumerable<BackgroundTaskModel>>(queue.GetTasks(id.ToArray()));
    }

    [HttpGet("[action]")]
    public void Cancel(Guid id, CancellationToken ct = default)
    {
        logger.LogDebug($"CancelTask {id}");
        queue.Cancel(id);
    }
}