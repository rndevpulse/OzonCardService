using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using OzonCard.Rms.Client.Data;
using OzonCard.Rms.Client.Helpers;

namespace OzonCard.Rms.Client;

public class RmsClient : DelegatingHandler, IAsyncDisposable
{
    private readonly string _endpoint, _login, _password;
    private readonly HttpClient _client;
    private string? _token, _reason;
    private DateTime _expire;
    private bool _isLoginProcess;
    private int _status = 0;

    public string? Reason => _reason;
    public int Status => _status;
    
    public RmsClient(string endpoint, string login, string password) : base(new HttpClientHandler())
    {
        _endpoint = $"{endpoint.TrimEnd('/')}/";
        _login = login;
        _password = password;
        _expire = DateTime.UtcNow;
        _client = new HttpClient(this);
        _client.BaseAddress = new Uri(_endpoint, UriKind.RelativeOrAbsolute);
    }

    #region Service

    
     /// <summary>
    /// Get access token
    /// </summary>
    /// <returns></returns>
    private async Task<string> GetTokenAsync()
    {
        var values = HttpUtility.ParseQueryString(string.Empty);
        values["login"] = _login;
        values["pass"] = GetPasswordShaHash(_password);

        return await _client.GetStringAsync("api/auth?" + values.ToString());
    }
    
    private async Task LoginAsync()
    {
        _isLoginProcess = true;
        _token = await GetTokenAsync();
        _expire = DateTime.UtcNow.AddMinutes(15);
        _isLoginProcess = false;
    }

    private async Task LogoutAsync(CancellationToken ct = default)
    {
        await _client.GetAsync(
            $"api/logout?key={_token}",
            ct
        );

        _token = null;
        _expire = DateTime.UtcNow;
    }

    private string FormatRequestQuery(string method, IReadOnlyCollection<KeyValuePair<string, object>>? queryParams = null)
    {
        return queryParams == null || queryParams.Count == 0
            ? method
            : $"{method}?{string.Join('&', queryParams.Select(kv => $"{kv.Key}={kv.Value}"))}";
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

            query["key"] ??= _token;
            uriBuilder.Query = query.ToString();
            request.RequestUri = uriBuilder.Uri;
        }

        var response = await base.SendAsync(request, cancellationToken);

        _status = (int)response.StatusCode;
        _reason = _status >= 400
            ? await response.Content.ReadAsStringAsync(cancellationToken)
            : null;

        return response;
    }

    private static string GetPasswordShaHash(string password)
    {
        var bytes = Encoding.UTF8.GetBytes(password);
        var sha1 = SHA1.Create();
        var hashBytes = sha1.ComputeHash(bytes);

        return HexStringFromBytes(hashBytes);
    }

    private static string HexStringFromBytes(IEnumerable<byte> bytes)
    {
        var sb = new StringBuilder();
        foreach (var b in bytes)
        {
            var hex = b.ToString("x2");
            sb.Append(hex);
        }

        return sb.ToString();
    }

    public async ValueTask DisposeAsync()
    {
        if (!string.IsNullOrWhiteSpace(_token))
            await LogoutAsync();

        _client.Dispose();

        GC.SuppressFinalize(this);
    }

    #endregion

    public Task<ReportResponse<CustomerRowReport>> GetShortReportAsync(DateTime from, DateTime to, string paymentType, CancellationToken ct = default)
    {
        return CallMethodAsync<ReportResponse<CustomerRowReport>>("/api/v2/reports/olap",
            new ReportsHelper(from, to, paymentType).ShortSales,
            ct);
    }
    public Task<ReportResponse<TransactionRowReport>> GetTransactionsReportAsync(DateTime from, DateTime to, string paymentType, CancellationToken ct = default)
    {
        return CallMethodAsync<ReportResponse<TransactionRowReport>>("/api/v2/reports/olap",
            new ReportsHelper(from, to, paymentType).TransactionSales,
            ct);
    }
    private async Task<T> CallMethodAsync<T>(string method, object payload, CancellationToken ct = default)
    {
        var response = await _client.PostAsJsonAsync(method, payload, ct);
        if (response.IsSuccessStatusCode
            && await response.Content.ReadFromJsonAsync<T>(ct) is {} result)
            return result;
        throw new Exception("Report wos failed");
    }
    
}