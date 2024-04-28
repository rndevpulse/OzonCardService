using OzonCard.Biz.Client;
using OzonCard.Common.Application.Customers.Commands;
using OzonCard.Common.Application.Organizations;
using OzonCard.Common.Core;
using OzonCard.Common.Core.Exceptions;
using OzonCard.Common.Domain.Organizations;

namespace OzonCard.Common.Application.Customers.Handlers;

public class CustomerUpdateCategoryCommandHandler(
    IOrganizationRepository orgRepository
) : ICommandHandler<CustomerUpdateCategoryCommand, IEnumerable<string>>
{
    public async Task<IEnumerable<string>> Handle(CustomerUpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var org = await orgRepository.GetItemAsync(request.OrganizationId, cancellationToken);
        var selected = org.Categories
            .Where(x => request.Categories.Contains(x.Id))
            .ToArray();
        // if (org.Categories.All(x=>x.Id != request.CategoryId))
        //     throw EntityNotFoundException.For<Category>(request.CategoryId, $"in org '{org.Name}'");
        var client = new BizClient(org.Login, org.Password);
        Func<Guid,Guid,Guid,CancellationToken,Task<bool>> action = request.IsRemove
            ? client.RemoveCategoryToCustomerAsync
            : client.AppendCategoryToCustomerAsync;
        foreach (var category in selected)
            await action.Invoke(request.Id, request.OrganizationId, category.Id, cancellationToken);
        return selected.Select(x=>x.Name);
    }
}