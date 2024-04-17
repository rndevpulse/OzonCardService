using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OzonCard.Customer.Api.Services.BackgroundTasks;

namespace OzonCard.Customer.Api.Controllers;

public class TasksController(
    ILogger<TasksController> logger,
    IBackgroundQueue queue
) : ApiController
{

    [HttpGet]
    public async Task<object> Index([FromQuery] IEnumerable<Guid> id, CancellationToken ct = default)
    {
        logger.LogDebug("Get tasks:");
        
    }

    [HttpGet("[action]")]
    public async Task<object> Cancel(Guid id, CancellationToken ct = default)
    {
        logger.LogDebug($"CancelTask {id}");
        

    }
}