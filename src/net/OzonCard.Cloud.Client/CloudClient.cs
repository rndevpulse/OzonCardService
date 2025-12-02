using System.Net.Http.Headers;
using OzonCard.Cloud.Client.Data.Categories;
using OzonCard.Cloud.Client.Data.Checkin;
using OzonCard.Cloud.Client.Data.Customers;
using OzonCard.Cloud.Client.Data.Logins;
using OzonCard.Cloud.Client.Data.Nomenclature;
using OzonCard.Cloud.Client.Data.OrderTypes;
using OzonCard.Cloud.Client.Data.Organizations;
using OzonCard.Cloud.Client.Data.Programs;

namespace OzonCard.Cloud.Client;

public class CloudClient : DelegatingHandler, IAsyncDisposable
{
    private readonly string _token;
    private readonly HttpClient _client;
    
    private string? _access;
    private DateTime _expire;
    private bool _isLoginProcess;
    
    public CloudClient(string token) : base(new HttpClientHandler())
    {
        _token = token;
        _client = new HttpClient(this);
        _client.BaseAddress = new Uri("https://api-ru.iiko.services/", UriKind.RelativeOrAbsolute);
    }

    #region Services

    private async Task LoginAsync(CancellationToken ct = default)
    {
        _isLoginProcess = true;
        var response = await _client.CallAsync<Auth>("/api/1/access_token", new Login(_token), ct);
        _access = response.Token;
        _expire = DateTime.UtcNow.AddHours(1);
        _isLoginProcess = false;
    }
    
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (_isLoginProcess)
            return await base.SendAsync(request, cancellationToken);

        if (string.IsNullOrWhiteSpace(_access) || _expire <= DateTime.UtcNow)
            await LoginAsync(cancellationToken);

        if (!string.IsNullOrWhiteSpace(_access))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _access);

        var response = await base.SendAsync(request, cancellationToken);
        return response;
    }
    
    public ValueTask DisposeAsync()
    {
        _client.Dispose();
        GC.SuppressFinalize(this);
        return default;
    }

    #endregion


    public async Task<IEnumerable<Organization>> GetOrganizationsAsync(CancellationToken ct = default)
    {
        var response = await _client.CallAsync<ResultOrganizations>("/api/1/organizations", ct);
        return response.Organizations;
    }

    public async Task<Guid> CreateOrUpdateCustomerAsync(CreateOrUpdateCustomer customer, CancellationToken ct = default)
    {
        var response = await _client.CallAsync<CustomerId>("/api/1/loyalty/iiko/customer/create_or_update", customer, ct);
        return response.Id;
    }

    public async Task AddCardAsync(AddCardCustomer request, CancellationToken ct = default)
    {
        await _client.CallAsync<object>("api/1/loyalty/iiko/customer/card/add", request, ct);
    }

    public Task<Customer> GetCustomerAsync(RequestCustomerInfo customer, CancellationToken ct = default) =>
        _client.CallAsync<Customer>("/api/1/loyalty/iiko/customer/info", customer, ct);


    public async Task<IEnumerable<LoyaltyProgram>> GetProgramsAsync(Guid organizationId, CancellationToken ct = default)
    {
        var response = await _client.CallAsync<ResultPrograms>("/api/1/loyalty/iiko/program",
            new { organizationId, withoutMarketingCampaigns = true }, ct);
        return response.Programs;
    }

    public async Task AddToProgramAsync(Guid id, Guid programId, Guid organizationId, CancellationToken ct = default)
    {
        await _client.CallAsync<object>("/api/1/loyalty/iiko/customer/program/add",
            new
            {
                customerId = id,
                programId,
                organizationId
            }, cancellationToken: ct);
    }
    
    public async Task<IEnumerable<OrderType>> GetOrderTypesAsync(Guid organizationId, CancellationToken ct = default)
    {
        var response = await _client.CallAsync<OrderTypesResult>(
            "/api/1/deliveries/order_types", 
            new {organizationIds = new[]{organizationId}},
            ct
        );
        return response?.OrderTypes
                   .FirstOrDefault(x => x.OrganizationId == organizationId)?.Items
               ?? [];
    }
    
    public async Task<CalculateResult> CalculateCheckinAsync(CalculateCheckin content, CancellationToken ct = default)
    {
        return await _client.CallAsync<CalculateResult>(
            "/api/1/loyalty/iiko/calculate", 
            content,
            ct
        );
    }

    public async Task<IEnumerable<Category>> GetCategoriesAsync(Guid organizationId, CancellationToken ct = default)
    {
        var response = await _client.CallAsync<GuestCategoriesResult>(
            "/api/1/loyalty/iiko/customer_category", 
            new { organizationId },
            ct
        );
        return response.GuestCategories;
    }
    
    public async Task<IEnumerable<Product>> GetCatalogAsync(Guid organizationId, CancellationToken ct = default)
    {
        var response = await _client.CallAsync<CatalogResult>(
            "/api/1/nomenclature", 
            new { organizationId },
            ct
        );
        return response.Products;
    }

    public async Task AddCustomerCategoryAsync(Guid organizationId, Guid customerId, Guid categoryId,
        CancellationToken ct = default)
    {
        await _client.CallAsync<object>(
            "/api/1/loyalty/iiko/customer_category/add",
            new
            {
                organizationId,
                customerId,
                categoryId
            }, ct);
    }
    public async Task RemoveCustomerCategoryAsync(Guid organizationId, Guid customerId, Guid categoryId,
        CancellationToken ct = default)
    {
        await _client.CallAsync<object>(
            "/api/1/loyalty/iiko/customer_category/remove",
            new
            {
                organizationId,
                customerId,
                categoryId
            }, ct);
    }

    public async Task IncCustomerBalanceAsync(Guid organizationId, Guid customerId, Guid walletId,
        decimal sum, CancellationToken ct = default)
    {
        await _client.CallAsync<object>(
            "/api/1/loyalty/iiko/customer/wallet/topup",
            new
            {
                organizationId,
                customerId,
                walletId,
                sum
            }, ct);
    }
    public async Task DecCustomerBalanceAsync(Guid organizationId, Guid customerId, Guid walletId,
        decimal sum, CancellationToken ct = default)
    {
        await _client.CallAsync<object>(
            "/api/1/loyalty/iiko/customer/wallet/chargeoff",
            new
            {
                organizationId,
                customerId,
                walletId,
                sum
            }, ct);
    }

}