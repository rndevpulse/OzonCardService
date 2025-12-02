using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using OzonCard.Cloud.Client.Data.Errors;

namespace OzonCard.Cloud.Client;

public static class HttpExtensions
{
    private static readonly JsonSerializerOptions Options = new ()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public static async Task<TResult> CallAsync<TResult>(this HttpClient client, string method, object value,
        CancellationToken cancellationToken = default) =>
        await GetResultAsync<TResult>(
            await client.PostAsync(method, 
                JsonContent.Create(value, MediaTypeHeaderValue.Parse("application/json"), Options), 
                // JsonContent.Create(value, null, Options), 
                // new StringContent(JsonSerializer.Serialize(value, Options), Encoding.UTF8, "application/json"),
                cancellationToken),
            cancellationToken
        );
    
    public static async Task<TResult> CallAsync<TResult>(this HttpClient client, string method, CancellationToken cancellationToken = default) => 
        await GetResultAsync<TResult>(
            await client.GetAsync(method, cancellationToken),
            cancellationToken
        );
    
   
    private static async Task<TResult> GetResultAsync<TResult>(HttpResponseMessage response, CancellationToken cancellationToken = default)
    {
        if (!response.IsSuccessStatusCode)
            throw await CreateExceptionAsync(response);
        return await response.Content.ReadFromJsonAsync<TResult>(cancellationToken)
               ?? throw new Exception($"Response '{response.RequestMessage?.RequestUri}' returned null"); 
    }

    private static async Task<Exception> CreateExceptionAsync(HttpResponseMessage response, Exception? inner = null)
    {
        var request = response.RequestMessage?.RequestUri;
        var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();

        return new HttpRequestException(
            $"Response '{request}' returned error with status {(int)response.StatusCode} {error?.Code}: {error?.Message ?? "without details"}",
            inner,
            response.StatusCode
        );
    }
}