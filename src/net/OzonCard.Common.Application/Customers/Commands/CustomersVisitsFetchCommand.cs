using OzonCard.Common.Application.Customers.Data;
using OzonCard.Common.Core;

namespace OzonCard.Common.Application.Customers.Commands;

public record CustomersVisitsFetchCommand(
    Guid OrgId,
    int Days
) :ICommand<SynchronizeResult>;