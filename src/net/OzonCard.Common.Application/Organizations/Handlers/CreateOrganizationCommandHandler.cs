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

            var categories = await client.GetCategoriesAsync(org.Id, cancellationToken);
            organization.UpdateCategories(categories.Select(x =>
                new Category(x.Id)
                {
                    IsActive = x.IsActive,
                    Name = x.Name
                }));
            var programs = await client.GetProgramsAsync(org.Id, cancellationToken);
            organization.UpdatePrograms(programs.Select(p =>
            {
                var program = new Program(p.Id)
                {
                    IsActive = p.ServiceTo == null || p.ServiceTo > DateTime.UtcNow,
                    Name = p.Name,
                };
                foreach (var wallet in p.Wallets)
                    program.AddOrUpdateWallet(
                        new Wallet(wallet.Id, wallet.Name, wallet.ProgramType, wallet.Type));
                return program;
            }));
            organizations.Add(organization);
        }
        return organizations;
    }
}