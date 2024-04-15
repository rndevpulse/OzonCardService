using System.Net.Http.Json;

namespace OzonCard.Biz.Client;

public static class HttpClientExtensions
{
    public static async Task<TResponse?> PostFromJsonAsync<TResponse, TRequest>(
        this HttpClient client,
        string request,
        TRequest payload,
        CancellationToken ct = default
    )
        where TRequest : class
    {
        var response = await client.PostAsJsonAsync<TRequest>(request, payload, ct);
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<TResponse>(ct);
        throw await response.ToException(); 
    }
    
    private static async Task<Exception> ToException(this HttpResponseMessage message)
    {
        return new HttpRequestException(
            await message.Content.ReadAsStringAsync(),
            null,
            message.StatusCode
        );
    }
}