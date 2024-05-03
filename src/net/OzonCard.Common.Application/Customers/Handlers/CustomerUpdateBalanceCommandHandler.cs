using OzonCard.Biz.Client;
using OzonCard.Common.Application.Customers.Commands;
using OzonCard.Common.Application.Organizations;
using OzonCard.Common.Core;
using OzonCard.Common.Core.Exceptions;
using OzonCard.Common.Domain.Organizations;

namespace OzonCard.Common.Application.Customers.Handlers;

public class CustomerUpdateBalanceCommandHandler(
    IOrganizationRepository organizations,
    ICustomerRepository customers
) : CustomerBaseHandler, ICommandHandler<CustomerUpdateBalanceCommand, decimal>
{
    public async Task<decimal> Handle(CustomerUpdateBalanceCommand request, CancellationToken cancellationToken)
    {
        var customer = await customers.GetItemAsync(request.Id, cancellationToken);
        var org = await organizations.GetItemAsync(customer.OrgId, cancellationToken);
        var program = org.Programs.FirstOrDefault(x => x.Id == request.ProgramId)
            ?? throw EntityNotFoundException.For<Program>(request.ProgramId);
        var wallet = program.Wallets.FirstOrDefault()
            ?? throw EntityNotFoundException.For<Wallet>("");
        var client = new BizClient(org.Login, org.Password);
        await TryRefreshBalance(client, customer.BizId, org.Id, wallet.Id, request.Balance, cancellationToken);
        return request.Balance;
    }
}