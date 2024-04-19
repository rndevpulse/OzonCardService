using System.Net.Http.Json;
using System.Web;
using OzonCard.Biz.Client.Models.Customers;
using OzonCard.Biz.Client.Models.Organizations;
using OzonCard.Biz.Client.Models.Reports;

namespace OzonCard.Biz.Client;



public class BizClient : DelegatingHandler, IAsyncDisposable, IBizClient
{
    private readonly string _login;
    private readonly string _password;
    private DateTime _expire = DateTime.Now;
    private string? _token, _reason = null;
    private bool _isLoginProcess = false;
    private int _status = 0;


    private readonly HttpClient _client;
    private readonly string _endpoint = "https://iiko.biz:9900/api/0";

    public string? Reason => _reason;
    public int Status => _status;
    public BizClient(string login, string password)
    {
        _login = login;
        _password = password;
        _client = new HttpClient(this)
        {
            BaseAddress = new Uri(_endpoint, UriKind.RelativeOrAbsolute)
        };
    }
    
    

    #region Requests
    private async Task<string> GetTokenAsync()
    {
        var values = HttpUtility.ParseQueryString(string.Empty);
        values["user_id"] = _login;
        // values["user_secret"] = GetPasswordShaHash(_password);
        values["user_secret"] = _password;
        return await _client.GetStringAsync($"auth/access_token?"+ values) 
               ?? throw new HttpRequestException();
    }

    public async Task<IEnumerable<OrganizationDto>> GetOrganizationsAsync(CancellationToken ct = default)
    {
        return await _client.GetFromJsonAsync<IEnumerable<OrganizationDto>>("organization/list", ct)
               ?? throw new Exception("Cannot load organizations");
    }

    public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync(Guid orgId, CancellationToken ct = default)
    {
        return await _client.GetFromJsonAsync<IEnumerable<CategoryDto>>($"organization/{orgId}/guest_categories", ct)
            ?? throw new Exception("Cannot load guest categories");
    }
    public async Task<IEnumerable<ProgramDto>> GetProgramsAsync(Guid orgId, CancellationToken ct = default)
    {
        return await _client.GetFromJsonAsync<IEnumerable<ProgramDto>>($"organization/{orgId}/corporate_nutritions", ct)
               ?? throw new Exception("Cannot load organizations program");
    }

    public async Task<Guid> CreateCustomerAsync(string name, string card, Guid orgId, CancellationToken ct = default)
    {
        return await _client.PostFromJsonAsync<Guid, CustomerContainerDto>(
            $"customers/create_or_update?organization={orgId}",
            new CustomerContainerDto(new CustomerDto(
                null, name, card, card)),
            ct);
    }
    
    public async Task<Guid> UpdateCustomerAsync(Guid bizId, string name, Guid orgId, CancellationToken ct = default)
    {
        return await _client.PostFromJsonAsync<Guid, CustomerContainerDto>(
            $"customers/create_or_update?organization={orgId}",
            new CustomerContainerDto(new CustomerDto(
                bizId, name, null, null)),
            ct);
    }

    public async Task<bool> AppendCategoryToCustomerAsync(Guid bizId, Guid orgId, Guid categoryId, CancellationToken ct = default)
    {
        var response = await _client.PostAsJsonAsync(
            $"customers/{bizId}/add_category?organization={orgId}&categoryId={categoryId}",
            new { },
            ct);
        return response.IsSuccessStatusCode;
    }
    
    public async Task<bool> RemoveCategoryToCustomerAsync(Guid bizId, Guid orgId, Guid categoryId, CancellationToken ct = default)
    {
        var response = await _client.PostAsJsonAsync(
            $"customers/{bizId}/remove_category?organization={orgId}&categoryId={categoryId}",
            new { },
            ct);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> AddCustomerToProgramAsync(Guid bizId, Guid orgId, Guid programId, CancellationToken ct = default)
    {
        var response = await _client.PostAsJsonAsync(
            $"customers/{bizId}/add_to_nutrition_organization?organization={orgId}&corporate_nutrition_id={programId}",
            new { },
            ct);
        return response.IsSuccessStatusCode;
    }

    public async Task<CustomerInfoDto> GetCustomerAsync(Guid bizId, Guid orgId, CancellationToken ct = default)
    {
        return await _client.GetFromJsonAsync<CustomerInfoDto>(
                   $"customers/get_customer_by_id?organization={orgId}&id={bizId}",
                   ct)
               ?? throw new Exception($"Cannot load customer for id '{bizId}'");
    }
    public async Task<CustomerInfoDto> GetCustomerAsync(string card, Guid orgId, CancellationToken ct = default)
    {
        return await _client.GetFromJsonAsync<CustomerInfoDto>(
                   $"customers/get_customer_by_card?organization={orgId}&card={card}",
                   ct)
               ?? throw new Exception($"Cannot load customer for card '{card}'");
    }

    public async Task<bool> IncBalanceForCustomer(Guid bizId, Guid orgId, Guid walletId, decimal balance,
        CancellationToken ct = default)
    {
        var response = await _client.PostAsJsonAsync(
            $"customers/refill_balance?",
            new ChangeBalanceDto(bizId, orgId, walletId, balance),
            ct);
        return response.IsSuccessStatusCode;
    }
    
    public async Task<bool> DecBalanceForCustomer(Guid bizId, Guid orgId, Guid walletId, decimal balance,
        CancellationToken ct = default)
    {
        var response = await _client.PostAsJsonAsync(
            $"customers/withdraw_balance?",
            new ChangeBalanceDto(bizId, orgId, walletId, balance),
            ct);
        return response.IsSuccessStatusCode;
    }


    public async Task<IEnumerable<ProgramReportDto>> GetProgramReports(Guid orgId, Guid programId, 
        DateTime dateFrom, DateTime dateTo, CancellationToken ct = default)
    {
        return await _client.GetFromJsonAsync<IEnumerable<ProgramReportDto>>(
               $"organization/{orgId}/corporate_nutrition_report?corporate_nutrition_id={programId}&date_from={dateFrom:yyyy-MM-dd}&date_to={dateTo:yyyy-MM-dd}",
               ct)
           ?? throw new Exception($"Cannot load program report");
    }

    public async Task<IEnumerable<TransactionsReportDto>> GetTransactionReports(Guid orgId, DateTime dateFrom,
        DateTime dateTo, TransactionType type = TransactionType.PayFromWallet, CancellationToken ct = default)
    {
        var result = await _client.GetFromJsonAsync<IEnumerable<TransactionsReportDto>>(
            $"organization/{orgId}/transactions_report?date_from={dateFrom:yyyy-MM-dd}&date_to={dateTo:yyyy-MM-dd}",
            ct);
        return result?.Where(x => x.TransactionType == type.ToString())
            ?? throw new Exception($"Cannot load transactions report");

    }

    public async Task<IEnumerable<ShortGuestInfoDto>> GetShortCustomersReport(Guid orgId, CancellationToken ct = default)
    {
        var result = await _client.GetFromJsonAsync<IEnumerable<ShortGuestInfoDto>>(
            $"customers/get_customers_by_organization_and_by_period?organizationId={orgId}&date_from={DateTime.Now:yyyy-MM-dd}&date_to={DateTime.Now.AddDays(-95):yyyy-MM-dd}",
            ct);
        return result ?? throw new Exception($"Cannot load short customers report");
    }

    #endregion
    private async Task LoginAsync()
    {
        _isLoginProcess = true;
        _token = await GetTokenAsync();
        _expire = DateTime.UtcNow.AddMinutes(15);
        _isLoginProcess = false;
    }
    
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (_isLoginProcess)
            return await base.SendAsync(request, cancellationToken);

        if (string.IsNullOrWhiteSpace(_token) || _expire <= DateTime.UtcNow)
            await LoginAsync();

        if (request.RequestUri != null)
        {
            var uriBuilder = new UriBuilder(request.RequestUri);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);

            query["access_token"] ??= _token;
            uriBuilder.Query = query.ToString();
            request.RequestUri = uriBuilder.Uri;
        }

        
        var response = await base.SendAsync(request, cancellationToken);
        if ((int)response.StatusCode == 429)
        {
            Thread.Sleep(500);
            await SendAsync(request, cancellationToken);
        }
        _status = (int)response.StatusCode;
        _reason = _status >= 400
            ? await response.Content.ReadAsStringAsync(cancellationToken)
            : null;

        return response;
    }
    
    public async ValueTask DisposeAsync()
    {
        _client.Dispose();
        GC.SuppressFinalize(this);
    }
}