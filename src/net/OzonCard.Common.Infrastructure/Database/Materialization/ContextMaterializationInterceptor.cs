using Microsoft.EntityFrameworkCore.Diagnostics;

namespace OzonCard.Common.Infrastructure.Database.Materialization;

public class ContextMaterializationInterceptor : IMaterializationInterceptor
{
    public static ContextMaterializationInterceptor Instance { get; } = new();
    private ContextMaterializationInterceptor() { }
    
    public object InitializedInstance(MaterializationInterceptionData materializationData, object entity)
    {
        return entity;
    }
}