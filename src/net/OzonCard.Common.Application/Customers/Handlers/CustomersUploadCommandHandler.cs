using Microsoft.Extensions.Logging;
using OzonCard.Common.Application.BackgroundTasks;
using OzonCard.Common.Application.Customers.Commands;
using OzonCard.Common.Application.Files;
using OzonCard.Common.Application.Organizations;
using OzonCard.Common.Core;
using OzonCard.Common.Domain.Customers;

namespace OzonCard.Common.Application.Customers.Handlers;

public class CustomersUploadCommandHandler(
    ILogger<CustomersUploadCommandHandler> logger,
    IOrganizationRepository orgRepository,
    ICustomerRepository customerRepository,
    IFileRepository fileRepository,
    IBackgroundQueue queue
) : ICommandHandler<CustomersUploadCommand, IEnumerable<Customer>>
{
    public Task<IEnumerable<Customer>> Handle(CustomersUploadCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}