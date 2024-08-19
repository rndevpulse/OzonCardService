
namespace OzonCard.Common.Domain.Props;

public class MemberReportBatchProp : Property
{
    public string Name { get; set; }
    public Guid Organization { get; set; }
    public MemberReportBatchProp(Guid reference, string name, Guid organization) 
        : base(reference, PropType.MemberBatch)
    {
        Name = name;
        Organization = organization;
    }
}