using OzonCard.Common.Application.Reports.Commands;
using OzonCard.Common.Core;

namespace OzonCard.Common.Application.Reports.Handlers;

public class ReportPaymentsCommandHandler : ICommandHandler<ReportPaymentsCommand, object>
{
    public Task<object> Handle(ReportPaymentsCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}