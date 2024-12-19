using System.Reflection;
using OzonCard.Common.Infrastructure.Database.Extensions;

namespace OzonCard.Common.Infrastructure.Extensions;

public class InfrastructureOptions : InfrastructureDatabaseOptions
{
    public bool ServerWorker { get; set; } = false;
    public Assembly[] Assemblies { get; set; } = [];
  
}