using System.Reflection;

namespace OzonCard.Common.Infrastructure.Extensions;

public class InfrastructureOptions
{
    internal string? Connection;
    internal bool IsDevelopment = false;
    internal bool ServerWorker = false;
    internal Assembly[] Assemblies = [];
    public InfrastructureOptions SetConnection(string? value)
    {
        Connection = value;
        return this;
    }
    public InfrastructureOptions SetDevEnvironment(bool value)
    {
        IsDevelopment = value;
        return this;
    }
    public InfrastructureOptions SetServerWorker(bool value)
    {
        ServerWorker = value;
        return this;
    }
    public InfrastructureOptions SetAssemblies(Assembly[] value)
    {
        Assemblies = value;
        return this;
    }
}