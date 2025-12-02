using OzonCard.Common.Application.Customers.Commands;
using OzonCard.Common.Application.Organizations;
using OzonCard.Common.Core;
using OzonCard.Common.Domain.Organizations;

namespace OzonCard.Common.Application.Customers.Handlers;

public class CustomerUpdateCategoryCommandHandler(
    IOrganizationRepository organizations,
    ICustomerRepository customers
) : ICommandHandler<CustomerUpdateCategoryCommand, IEnumerable<string>>
{
    public async Task<IEnumerable<string>> Handle(CustomerUpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var customer = await customers.GetItemAsync(request.Id, cancellationToken);
        var org = await organizations.GetItemAsync(customer.OrgId, cancellationToken);
        var selected = org.Categories
            .Where(x => request.Categories.Contains(x.CategoryId))
            .ToArray();
        // if (org.Categories.All(x=>x.Id != request.CategoryId))
        //     throw EntityNotFoundException.For<Category>(request.CategoryId, $"in org '{org.Name}'");
        // Func<Guid,Guid,Guid,CancellationToken,Task> action = request.IsRemove
        //     ? org.CloudClient.RemoveCustomerCategoryAsync
        //     :  org.CloudClient.AddCustomerCategoryAsync;
        foreach (var category in selected)
            try
            {
                // await action.Invoke(customer.BizId, customer.OrgId, category.CategoryId, cancellationToken);
                if (request.IsRemove)
                {
                    await org.CloudClient.RemoveCustomerCategoryAsync(customer.BizId, customer.OrgId, category.CategoryId, cancellationToken);
                    customer.RemoveCategory(category);
                    continue;
                }
                await org.CloudClient.AddCustomerCategoryAsync(customer.BizId, customer.OrgId, category.CategoryId, cancellationToken);
                customer.AddCategory(category);
            }
            catch (Exception)
            {
                //ignore
            }
        return selected.Select(x=>x.Name);
    }
}