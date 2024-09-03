using Microsoft.AspNetCore.Mvc;
using OzonCard.Common.Application.Properties.Commands;
using OzonCard.Common.Application.Properties.Queries;
using OzonCard.Customer.Api.Models.Props;
using Task = DocumentFormat.OpenXml.Office2021.DocumentTasks.Task;

namespace OzonCard.Customer.Api.Controllers;

public class PropsController : ApiController
{

    [HttpGet]
    public async Task<IEnumerable<ReportBatchModel>> Index(CancellationToken ct = default)
    {
        var result = await Queries.Send(
            new GetMemberReportsBatchProp(UserClaimSid),
            ct);
        return Mapper.Map<IEnumerable<ReportBatchModel>>(result);
    }

    [HttpPost]
    public async Task<ReportBatchModel> AddOrUpdate(ReportBatchModel model, CancellationToken ct = default)
    {
        var result = await Commands.Send(
            new SetMemberReportBatchPropCommand(
                UserClaimSid,
                model.Organization,
                model.Name,
                model.Properties,
                model.Id),
            ct);
        return Mapper.Map<ReportBatchModel>(result);
    }

    [HttpDelete]
    public async Task<IActionResult> Remove(Guid id, CancellationToken ct = default)
    {
        await Commands.Send(new RemovePropCommand(id), ct);
        return Ok();
    }
}