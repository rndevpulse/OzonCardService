﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OzonCard.Common.Application.Reports.Commands;
using OzonCard.Common.Worker.Services;
using OzonCard.Customer.Api.Models.BackgroundTask;
using OzonCard.Identity.Domain;

namespace OzonCard.Customer.Api.Controllers;

[Authorize(UserRole.Report)]
public class ReportController(
    ILogger<ReportController> logger,
    IBackgroundJobsService jobsService
) : ApiController
{

    [HttpPost("[action]")]
    public BackgroundTaskModel Payments(ReportPaymentsCommand cmd, CancellationToken ct = default)
    {
        logger.LogInformation("Payments by '{user}': {@cmd}", UserClaimEmail,cmd);
        cmd.SetUserId(UserClaimSid);
        cmd.SetUser(UserClaimEmail ?? "Unknown");
        cmd.UseTracking();
        var task = jobsService.Enqueue(cmd, cmd.Tracking);
        return Mapper.Map<BackgroundTaskModel>(task);
    }
    
    [HttpPost("[action]")]
    public BackgroundTaskModel Transactions(ReportTransactionsCommand cmd, CancellationToken ct = default)
    {
        logger.LogInformation("Transactions by '{user}': {@cmd}", UserClaimEmail, cmd);
        cmd.SetUserId(UserClaimSid);
        cmd.SetUser(UserClaimEmail ?? "Unknown");
        cmd.UseTracking();
        var task = jobsService.Enqueue(cmd, cmd.Tracking);
        return Mapper.Map<BackgroundTaskModel>(task);
    }
}