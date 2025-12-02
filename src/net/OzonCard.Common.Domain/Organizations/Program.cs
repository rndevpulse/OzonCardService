using OzonCard.Common.Domain.Abstractions;

namespace OzonCard.Common.Domain.Organizations;

public record Program : ValueObject
{
    public string Name { get; set; } = "";
    public bool IsActive { get; set; }
    public Guid ProgramId { get; set; }
    public Guid? WalletId { get; set; }
    public string WalletType { get; set; }
}