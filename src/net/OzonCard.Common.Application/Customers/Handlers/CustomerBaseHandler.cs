using OzonCard.Biz.Client;
using OzonCard.Common.Application.Customers.Data;

namespace OzonCard.Common.Application.Customers.Handlers;

public abstract class CustomerBaseHandler
{
    protected readonly CustomersTaskProgress Progress = new();
    protected async Task TryRefreshBalance(BizClient client, Guid bizId, Guid orgId, Guid walletId, decimal balance, CancellationToken ct)
    {
        var bizCustomer = await client.GetCustomerAsync(bizId, orgId, ct);
        var currentBalance = bizCustomer.WalletBalances?.
            FirstOrDefault(x => x.Wallet.Id == walletId)
            ?.Balance ?? null;
       
        if (currentBalance == null || currentBalance == balance)
            return;
        if (currentBalance < balance)
            await client.IncBalanceForCustomer(bizId, orgId, walletId, balance - (decimal)currentBalance, ct);
        else
            await client.DecBalanceForCustomer(bizId, orgId, walletId, (decimal)currentBalance - balance, ct);
        Progress.CountBalance++;
    }
}