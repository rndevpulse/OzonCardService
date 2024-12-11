using OzonCard.Common.Worker.Data;

namespace OzonCard.Common.Application.Customers.Data;

public record CustomersTaskProgress : NamedProgress
{
    public int CountAll { get; set; }
    public int CountNew { get; set; }
    public int CountFail { get; set; }
    public int CountBalance { get; set; }
    public int CountCategory { get; set; }
    public int CountProgram { get; set; }
    
}