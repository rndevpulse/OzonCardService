
using OzonCard.Cloud.Client.Data.Customers;
using OzonCard.Common.Application.Common;
using OzonCard.Common.Application.Customers.Data;
using OzonCard.Common.Core;
using OzonCard.Common.Domain.Organizations;

namespace OzonCard.Common.Application.Customers.Handlers;

public abstract class CustomerBaseHandler(IEventBus events) : BaseCommandHandlerProgress(events)
{
    protected readonly CustomersTaskProgress Progress = new();
    protected async Task TryRefreshBalance(Organization organization, Guid bizId, Guid walletId, decimal balance, CancellationToken ct)
    {
        var bizCustomer = await organization.CloudClient.GetCustomerAsync(new RequestCustomerInfo(organization.TransportId, CustomerField.Id)
        {
            Id = bizId,
        }, ct);
        var currentBalance = bizCustomer.WalletBalances?.
            FirstOrDefault(x => x.Id == walletId)
            ?.Balance ?? null;
       
        if (currentBalance == null || currentBalance == balance)
            return;
        if (currentBalance < balance)
            await organization.CloudClient.IncCustomerBalanceAsync(
                organization.TransportId,
                bizId,
                walletId,
                balance - (decimal)currentBalance, ct);
        else
            await organization.CloudClient.DecCustomerBalanceAsync(
                organization.TransportId,
                bizId,
                walletId, (decimal)currentBalance - balance, ct);
        Progress.CountBalance++;
    }
}