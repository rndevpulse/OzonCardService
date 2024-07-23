using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OzonCard.Common.Worker.Services;
using OzonCard.Customer.Api.Models.BackgroundTask;

namespace OzonCard.Customer.Api.Controllers;

public class TasksController(
    ILogger<TasksController> logger,
    IBackgroundJobsService jobsService
) : ApiController
{

    [HttpGet]
    public object Index([FromQuery] IEnumerable<string> id, CancellationToken ct = default)
    {
        // logger.LogDebug("Get tasks:");
        return Mapper.Map<IEnumerable<BackgroundTaskModel>>(jobsService.GetTasks(id.ToArray()));
    }

    [HttpGet("[action]")]
    public BackgroundTaskModel Cancel(string id, CancellationToken ct = default)
    {
        logger.LogDebug($"CancelTask {id}");
        var result = jobsService.Cancel(id);
        return Mapper.Map<BackgroundTaskModel>(result);
    }
}