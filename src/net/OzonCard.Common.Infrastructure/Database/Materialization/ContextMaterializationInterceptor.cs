using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using OzonCard.Common.Application.Customers;
using OzonCard.Common.Application.Visits;
using OzonCard.Common.Domain.Customers;

namespace OzonCard.Common.Infrastructure.Database.Materialization;

public class ContextMaterializationInterceptor : IMaterializationInterceptor
{
    public static ContextMaterializationInterceptor Instance { get; } = new();
    private ContextMaterializationInterceptor() { }
    
    public object InitializedInstance(MaterializationInterceptionData materializationData, object entity)
    {
        if (entity is not Customer customer)
            return entity;
        customer.Context = new CoreCustomerContext(
            customer,
            materializationData.Context.GetService<IVisitRepository>());
        return entity;
    }
}