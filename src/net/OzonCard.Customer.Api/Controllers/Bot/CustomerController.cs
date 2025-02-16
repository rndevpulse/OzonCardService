using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OzonCard.Common.Application.Customers.Commands;
using OzonCard.Common.Application.Customers.Data;
using OzonCard.Common.Application.Customers.Queries;
using OzonCard.Common.Worker.Services;
using OzonCard.Customer.Api.Models.BackgroundTask;
using OzonCard.Customer.Api.Models.Customers;

namespace OzonCard.Customer.Api.Controllers.Bot;

public class CustomerController(
    ILogger<CustomerController> logger,
    IBackgroundJobsService jobsService
) : BotController
{
    [HttpPost("[action]")]
    public Task Upload(CustomerUploadModel model, CancellationToken ct = default)
    {
        logger.LogInformation("Upload customer by bot");
        jobsService.Enqueue(new CustomersUploadCommand
        {
            OrganizationId = model.OrganizationId,
            CategoriesId = model.CategoriesId,
            ProgramId = model.ProgramId,
            Balance = model.Balance,
            Customer = new SingleCustomerUpload
            {
                Card = model.Card,
                Name = model.Name,
            },
            Options = new CustomersUploadOptions
            {
                Rename = true,
                RefreshBalance = true
            }
        });
        return Task.CompletedTask;
    }

    [HttpPost("[action]")]
    public async Task<IEnumerable<CustomerModel>> Search(CustomersSearchQuery cmd, CancellationToken ct = default)
    {
        logger.LogInformation("Search customers by bot");
        var result = await Queries.Send(cmd, ct);
        return Mapper.Map<IEnumerable<CustomerModel>>(result);
    }

}