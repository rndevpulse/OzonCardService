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
    public async Task<IEnumerable<BackgroundTaskModel>> Index([FromQuery] IEnumerable<string> id, CancellationToken ct = default)
    {
        // logger.LogDebug("Get tasks:");
        return Mapper.Map<IEnumerable<BackgroundTaskModel>>(
            await jobsService.GetTasksAsync(UserClaimSid, id.ToArray())
        );
    }

    [HttpGet("[action]")]
    public BackgroundTaskModel Cancel(string id, CancellationToken ct = default)
    {
        var result = jobsService.Cancel(id);
        return Mapper.Map<BackgroundTaskModel>(result);
    }
    
    [HttpGet("[action]")]
    public BackgroundTaskModel Remove(string id, CancellationToken ct = default)
    {
        var result = jobsService.Remove(id);
        return Mapper.Map<BackgroundTaskModel>(result);
    }
}