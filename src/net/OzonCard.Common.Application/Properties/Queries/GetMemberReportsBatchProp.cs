using OzonCard.Common.Core;
using OzonCard.Common.Domain.Props;

namespace OzonCard.Common.Application.Properties.Queries;

public record GetMemberReportsBatchProp(
    Guid Member
) : IQuery<IEnumerable<MemberReportBatchProp>>
{
    public class Handler(
        IPropertiesRepository repository
    ) : IQueryHandler<GetMemberReportsBatchProp, IEnumerable<MemberReportBatchProp>>
    {
        public async Task<IEnumerable<MemberReportBatchProp>> Handle(GetMemberReportsBatchProp request, CancellationToken cancellationToken)
        {
            var props = await repository.GetItemsAsync(
                request.Member, PropType.Member, cancellationToken);

            return props.OfType<MemberReportBatchProp>();

        }
    }
}