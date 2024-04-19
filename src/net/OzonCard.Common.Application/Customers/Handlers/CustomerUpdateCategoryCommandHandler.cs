using OzonCard.Biz.Client;
using OzonCard.Common.Application.Customers.Commands;
using OzonCard.Common.Application.Organizations;
using OzonCard.Common.Core;
using OzonCard.Common.Core.Exceptions;
using OzonCard.Common.Domain.Organizations;

namespace OzonCard.Common.Application.Customers.Handlers;

public class CustomerUpdateCategoryCommandHandler(
    IOrganizationRepository orgRepository
) : ICommandHandler<CustomerUpdateCategoryCommand, string>
{
    public async Task<string> Handle(CustomerUpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var org = await orgRepository.GetItemAsync(request.OrganizationId, cancellationToken);
        if (org.Categories.All(x=>x.Id != request.CategoryId))
            throw EntityNotFoundException.For<Category>(request.CategoryId, $"in org '{org.Name}'");
        var client = new BizClient(org.Login, org.Password);
        Func<Guid,Guid,Guid,CancellationToken,Task<bool>> action = request.isRemove
            ? client.RemoveCategoryToCustomerAsync
            : client.AppendCategoryToCustomerAsync;
        await action.Invoke(request.Id, request.OrganizationId, request.CategoryId, cancellationToken);
        return org.Categories.First(x => x.Id == request.CategoryId).Name;
    }
}