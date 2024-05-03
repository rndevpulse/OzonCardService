using OzonCard.Biz.Client.Models.Customers;
using OzonCard.Biz.Client.Models.Organizations;
using OzonCard.Biz.Client.Models.Reports;

namespace OzonCard.Biz.Client;

public interface IBizClient
{
    string? Reason { get; }
    int Status { get; }
    Task<IEnumerable<OrganizationDto>> GetOrganizationsAsync(CancellationToken ct = default);
    Task<IEnumerable<CategoryDto>> GetCategoriesAsync(Guid orgId, CancellationToken ct = default);
    Task<IEnumerable<ProgramDto>> GetProgramsAsync(Guid orgId, CancellationToken ct = default);
    Task<Guid> CreateCustomerAsync(string name, string card, Guid orgId, CancellationToken ct = default);
    Task<Guid> UpdateCustomerAsync(Guid bizId, string name, Guid orgId, CancellationToken ct = default);
    Task<bool> AppendCategoryToCustomerAsync(Guid bizId, Guid orgId, Guid categoryId, CancellationToken ct = default);
    Task<bool> RemoveCategoryToCustomerAsync(Guid bizId, Guid orgId, Guid categoryId, CancellationToken ct = default);
    Task<bool> AddCustomerToProgramAsync(Guid bizId, Guid orgId, Guid programId, CancellationToken ct = default);
    Task<CustomerInfoDto> GetCustomerAsync(Guid bizId, Guid orgId, CancellationToken ct = default);
    Task<CustomerInfoDto> GetCustomerAsync(string card, Guid orgId, CancellationToken ct = default);

    Task<bool> IncBalanceForCustomer(Guid bizId, Guid orgId, Guid walletId, decimal balance,
        CancellationToken ct = default);

    Task<bool> DecBalanceForCustomer(Guid bizId, Guid orgId, Guid walletId, decimal balance,
        CancellationToken ct = default);

    Task<IEnumerable<ProgramReportDto>> GetProgramReport(Guid orgId, Guid programId, 
        DateTime dateFrom, DateTime dateTo, CancellationToken ct = default);

    Task<IEnumerable<TransactionsReportDto>> GetTransactionReport(Guid orgId, DateTime dateFrom,
        DateTime dateTo, TransactionType type = TransactionType.PayFromWallet, CancellationToken ct = default);

    Task<IEnumerable<ShortGuestInfoDto>> GetShortCustomersReport(Guid orgId, CancellationToken ct = default);
}