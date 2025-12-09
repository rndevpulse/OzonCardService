using Microsoft.Extensions.Logging;
using OzonCard.Cloud.Client;
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
        
        var result = new List<Organization>();
        var client = new CloudClient(request.Token);
        var cloudOrganizations = await client.GetOrganizationsAsync(cancellationToken);
        foreach (var cloudOrganization in cloudOrganizations)
        {
            var organization = await _repository.GetOrganizationByTransportId(cloudOrganization.Id, cancellationToken);
            if (organization == null)
            {
                organization = new Organization(
                    Guid.NewGuid(), 
                    string.Empty,
                    request.Login, 
                    request.Password,
                    request.Endpoint,
                    request.Token,
                    cloudOrganization.Id
                );
                await _repository.AddAsync(organization);
            }
            
            organization.Name = cloudOrganization.Name;
            organization.Token = request.Token;
       
            organization.AddOrUpdateMember(request.UserId, request.User);
        
            foreach (var category in await client.GetCategoriesAsync(organization.Id, cancellationToken))
                organization.UpdateCategory(category.Id, category.Name, category.IsActive);
        
            foreach (var program in await client.GetProgramsAsync(organization.Id, cancellationToken))
                organization.UpdatePrograms(
                    program.Id,
                    program.Name,
                    program.ServiceTo == null || program.ServiceTo > DateTime.UtcNow,
                    program.WalletId ?? Guid.Empty,
                    program.ProgramType.ToString());
            result.Add(organization);
        }
        
       
       
        return result;
    }
}