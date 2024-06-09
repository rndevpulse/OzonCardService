using Microsoft.Extensions.Logging;
using OzonCard.Biz.Client;
using OzonCard.Common.Application.Organizations.Commands;
using OzonCard.Common.Application.Resources;
using OzonCard.Common.Core;
using OzonCard.Common.Core.Exceptions;
using OzonCard.Common.Domain.Organizations;

namespace OzonCard.Common.Application.Organizations.Handlers;

public class CreateOrganizationCommandHandler : ICommandHandler<CreateOrganizationsCommand, IEnumerable<Organization>>
{
    private readonly ILogger<CreateOrganizationCommandHandler> _logger;
    private readonly IOrganizationRepository _repository;

    public CreateOrganizationCommandHandler(
        ILogger<CreateOrganizationCommandHandler> logger, 
        IOrganizationRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<IEnumerable<Organization>> Handle(CreateOrganizationsCommand request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.User))
            throw new BusinessException(Messages.UserIsNullException);
        
        _logger.LogDebug("Create new organizations");
        
        var client = new BizClient(request.Login, request.Password);
        var organizations = new List<Organization>();
        foreach (var org in await client.GetOrganizationsAsync(cancellationToken))
        {
            
            var organization = await _repository.TryGetItemAsync(org.Id, cancellationToken);
            if (organization == null)
            {
                _logger.LogDebug($"Create new organization {org.Name}");
                organization = new Organization(org.Id, org.Name, request.Login, request.Password);
                await _repository.AddAsync(organization);
            }

            organization.Name = org.Name;
            
            organization.AddOrUpdateMember(request.UserId, request.User);

            foreach (var category in await client.GetCategoriesAsync(organization.Id, cancellationToken))
                organization.UpdateCategory(category.Id, category.Name, category.IsActive);
            
            foreach (var program in await client.GetProgramsAsync(organization.Id, cancellationToken))
                organization.UpdatePrograms(
                    program.Id,
                    program.Name,
                    program.ServiceTo == null || program.ServiceTo > DateTime.UtcNow,
                    program.Wallets.FirstOrDefault()?.Id ?? Guid.Empty,
                    program.Wallets.FirstOrDefault()?.Type ?? "");
            organizations.Add(organization);
        }
        return organizations;
    }
}