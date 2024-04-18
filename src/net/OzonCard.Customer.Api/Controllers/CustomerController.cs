using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OzonCard.Common.Application.BackgroundTasks;
using OzonCard.Common.Application.Customers.Commands;
using OzonCard.Common.Application.Customers.Data;
using OzonCard.Common.Application.Customers.Queries;
using OzonCard.Common.Infrastructure.BackgroundTasks;
using OzonCard.Customer.Api.Models.BackgroundTask;
using OzonCard.Customer.Api.Models.Customers;
using OzonCard.Customer.Api.Services.BackgroundTasks;

namespace OzonCard.Customer.Api.Controllers;

public class CustomerController(
    ILogger<CustomerController> logger,
    IBackgroundQueue queue
) : ApiController
{


    [HttpPost]
    public BackgroundTaskModel Upload(CustomersUploadCommand cmd, CancellationToken ct = default)
    {
        logger.LogInformation("Upload customers by '{user}': {@cmd}", UserClaimEmail, cmd);
        cmd.SetUserId(UserClaimSid);
        var task = queue.Enqueue<object,CustomersTaskProgress>(
            cmd,
            null);
        return Mapper.Map<BackgroundTaskModel>(task);
    }



    [HttpPost("[action]")]
    public async Task<IEnumerable<CustomerModel>> Search(CustomerSearchQuery cmd, CancellationToken ct = default)
    {
        logger.LogInformation("Search customers by '{user}': {@cmd}", UserClaimEmail, cmd);
        var result = await Queries.Send(cmd, ct);
        return Mapper.Map<IEnumerable<CustomerModel>>(result);
    }

    [HttpPost("[action]")]
    public async Task<CustomerModel> Category(CustomerUpdateCategoryCommand cmd, CancellationToken ct = default)
    {
        var result = await Commands.Send(cmd, ct);
        return Mapper.Map<CustomerModel>(result);
    }

    [HttpPost("[action]")]
    public async Task<CustomerModel> Balance(CustomerUpdateBalanceCommand cmd, CancellationToken ct = default)
    {
        var result = await Commands.Send(cmd, ct);
        return Mapper.Map<CustomerModel>(result);
    }
    
}