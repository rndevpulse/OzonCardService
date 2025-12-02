using Microsoft.Extensions.Logging;
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
        
        var organizations = await organization.CloudClient.GetOrganizationsAsync(cancellationToken);
        if (organizations.FirstOrDefault(x => x.Id == organization.TransportId) is { } org)
            organization.Name = org.Name;
        
        
        foreach (var category in await organization.CloudClient.GetCategoriesAsync(organization.TransportId, cancellationToken))
            organization.UpdateCategory(category.Id, category.Name, category.IsActive);
        
        foreach (var program in await organization.CloudClient.GetProgramsAsync(organization.TransportId, cancellationToken))
            organization.UpdatePrograms(
                program.Id,
                program.Name,
                program.ServiceTo == null || program.ServiceTo > DateTime.UtcNow,
                program.WalletId ?? Guid.Empty,
                program.ProgramType.ToString());
      
        return organization;
    }
}