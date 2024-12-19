

namespace OzonCard.Common.Infrastructure.Database.Extensions;

public class InfrastructureDatabaseOptions
{
    public string Connection { get; set; } = "";
    public string Provider { get; set; } = "sqlserver"; 
    public bool IsDevelopment { get; set; } = false;
    
}