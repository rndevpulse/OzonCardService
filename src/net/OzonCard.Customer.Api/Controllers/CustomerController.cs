using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OzonCard.Common.Application.Customers.Commands;
using OzonCard.Common.Application.Customers.Queries;
using OzonCard.Common.Worker.Services;
using OzonCard.Customer.Api.Models.BackgroundTask;
using OzonCard.Customer.Api.Models.Customers;

namespace OzonCard.Customer.Api.Controllers;

public class CustomerController(
    ILogger<CustomerController> logger,
    IBackgroundJobsService jobsService
) : ApiController
{


    [HttpPost]
    public BackgroundTaskModel Upload(CustomersUploadCommand cmd, CancellationToken ct = default)
    {
        logger.LogInformation("Upload customers by '{user}': {@cmd}", UserClaimEmail, cmd);
        cmd.SetUserId(UserClaimSid);
        cmd.SetUser(UserClaimEmail ?? "Unknown");
        cmd.UseTracking();
        var task = jobsService.Enqueue(cmd, cmd.Tracking);
        return Mapper.Map<BackgroundTaskModel>(task);
    }



    [HttpPost("[action]")]
    public async Task<IEnumerable<CustomerModel>> Search(CustomersSearchQuery cmd, CancellationToken ct = default)
    {
        logger.LogInformation("Search customers by '{user}': {@cmd}", UserClaimEmail, cmd);
        var result = await Queries.Send(cmd, ct);
        return Mapper.Map<IEnumerable<CustomerModel>>(result);
    }

    [HttpPost("[action]")]
    public async Task<IEnumerable<string>> Category(CustomerUpdateCategoryCommand cmd, CancellationToken ct = default)
    {
        var result = await Commands.Send(cmd, ct);
        return result;
    }

    [HttpPost("[action]")]
    public async Task<decimal> Balance(CustomerUpdateBalanceCommand cmd, CancellationToken ct = default)
    {
        var result = await Commands.Send(cmd, ct);
        return result;
    }

    [HttpDelete("{id}")]
    public async Task<CustomerModel> Remove(Guid id, CancellationToken ct = default) =>
        Mapper.Map<CustomerModel>(await Commands.Send(new CustomerRemoveCommand(id), ct));
    
    [HttpPut]
    public async Task<CustomerModel> Update(CustomerUpdateCommand cmd, CancellationToken ct = default) =>
        Mapper.Map<CustomerModel>(await Commands.Send(cmd, ct));

}