
namespace OzonCard.Common.Domain.Props;

public class MemberReportBatchProp : Property
{
    public string Name { get; set; }
    public Guid Organization { get; }
    public MemberReportBatchProp(Guid reference, string name, Guid organization) 
        : base(reference, PropType.Member)
    {
        Name = name;
        Organization = organization;
    }
}