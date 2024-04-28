using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OzonCard.Common.Application.BackgroundTasks;
using OzonCard.Common.Application.Reports.Commands;
using OzonCard.Customer.Api.Models.BackgroundTask;
using OzonCard.Identity.Domain;

namespace OzonCard.Customer.Api.Controllers;

[Authorize(UserRole.Report)]
public class ReportController(
    ILogger<ReportController> logger,
    IBackgroundQueue queue
) : ApiController
{

    [HttpPost("[action]")]
    public BackgroundTaskModel Payments(ReportPaymentsCommand cmd, CancellationToken ct = default)
    {
        logger.LogInformation("Payments by '{user}': {@cmd}", UserClaimEmail,cmd);
        var reference = Guid.NewGuid();
        cmd.SetTaskId(reference);
        cmd.SetUserId(UserClaimSid);
        cmd.SetUser(UserClaimEmail ?? "Unknown");
        var task = queue.Enqueue(
            cmd,
            reference);
        return Mapper.Map<BackgroundTaskModel>(task);
    }
    
    [HttpPost("[action]")]
    public BackgroundTaskModel Transactions(ReportTransactionsCommand cmd, CancellationToken ct = default)
    {
        logger.LogInformation("Transactions by '{user}': {@cmd}", UserClaimEmail, cmd);
        var reference = Guid.NewGuid();
        cmd.SetTaskId(reference);
        cmd.SetUserId(UserClaimSid);
        var task = queue.Enqueue(
            cmd,
            null);
        return Mapper.Map<BackgroundTaskModel>(task);
    }
}