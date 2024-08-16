using OzonCard.Common.Application.Properties.Data;
using OzonCard.Common.Core;
using OzonCard.Common.Core.Exceptions;
using OzonCard.Common.Domain.Props;

namespace OzonCard.Common.Application.Properties.Commands;

public record SetMemberReportBatchPropCommand(
    Guid Member,
    Guid Organization,
    string Name,
    IEnumerable<ReportBatchProp> Props,
    Guid? Id = null
) : ICommand<MemberReportBatchProp>
{
    public class Handler(
        IPropertiesRepository repository
    ) : ICommandHandler<SetMemberReportBatchPropCommand, MemberReportBatchProp>
    {
        public async Task<MemberReportBatchProp> Handle(SetMemberReportBatchPropCommand request, CancellationToken cancellationToken)
        {
            MemberReportBatchProp memberProp;
            if (request.Id == null)
            {
                memberProp = new MemberReportBatchProp(
                    request.Member,
                    request.Name,
                    request.Organization);
                await repository.AddAsync(memberProp);
            }
            else
                memberProp = repository.GetQuery()
                    .OfType<MemberReportBatchProp>()
                    .FirstOrDefault(x=>x.Id == request.Id)
                    ?? throw EntityNotFoundException.For<MemberReportBatchProp>(request.Id);

            memberProp.Name = request.Name;
            memberProp.UpdateProperty(request.Props);
            return memberProp;
        }
    }
}