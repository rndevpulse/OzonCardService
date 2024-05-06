using Microsoft.Extensions.Logging;
using OzonCard.Biz.Client;
using OzonCard.Common.Application.Organizations.Commands;
using OzonCard.Common.Core;
using OzonCard.Common.Core.Exceptions;
using OzonCard.Common.Domain.Organizations;

namespace OzonCard.Common.Application.Organizations.Handlers;

public class UpdateOrganizationCommandHandler : ICommandHandler<UpdateOrganizationCommand, Organization>
{
    private readonly ILogger<UpdateOrganizationCommandHandler> _logger;
    private readonly IOrganizationRepository _repository;

    public UpdateOrganizationCommandHandler(
        ILogger<UpdateOrganizationCommandHandler> logger,
        IOrganizationRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<Organization> Handle(UpdateOrganizationCommand request, CancellationToken cancellationToken)
    {
        var organization = await _repository.TryGetItemAsync(request.Id, cancellationToken);
        if (organization == null)
            throw EntityNotFoundException.For<Organization>(request.Id);
        _logger.LogDebug($"Update organization '{organization.Name}'");

        var client = new BizClient(organization.Login, organization.Password);
        var orgs = await client.GetOrganizationsAsync(cancellationToken);
        if (orgs.FirstOrDefault(x => x.Id == request.Id) is { } org)
            organization.Name = org.Name;

        var categories = await client.GetCategoriesAsync(organization.Id, cancellationToken);
        foreach (var category in categories)
            organization.UpdateCategory(category.Id, category.Name, category.IsActive);

        var programs = await client.GetProgramsAsync(organization.Id, cancellationToken);
        foreach (var program in programs)
            organization.UpdatePrograms(
                program.Id,
                program.Name,
                program.ServiceTo == null || program.ServiceTo > DateTime.UtcNow,
                program.Wallets.FirstOrDefault()?.Id ?? Guid.Empty,
                program.Wallets.FirstOrDefault()?.Type ?? "");
        return organization;
    }
}