using OzonCard.Common.Application.Reports.Commands;
using OzonCard.Common.Core;

namespace OzonCard.Common.Application.Reports.Handlers;

public class ReportTransactionsCommandHandler : ICommandHandler<ReportTransactionsCommand, object>
{
    public Task<object> Handle(ReportTransactionsCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}