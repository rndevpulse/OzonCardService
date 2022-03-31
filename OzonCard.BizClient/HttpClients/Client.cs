using Newtonsoft.Json;
using Serilog;
using System.Text;

namespace OzonCard.BizClient.HttpClients
{
    public class Client : IClient
    {
        private readonly ILogger log = Log.ForContext(typeof(Client));
        private readonly HttpClient _httpClient;
        public Client(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<T?> Send<T>(string query = "", string method = "GET", object? body = null)
        {
            log.Debug("Request api to {0}: {1}", method, query);
            var request = new HttpRequestMessage(new HttpMethod(method), query);

            if (body != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
            }
            try
            {
                using var response = await _httpClient.SendAsync(request);
                log.Debug("HTTP RESPONSE status: {0}", response.StatusCode);

                if (response.IsSuccessStatusCode)
                {
                    var valueContent = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrWhiteSpace(valueContent))
                        return JsonConvert.DeserializeObject<T>(valueContent);
                }

                return default;
            }
            catch (Exception ex) 
            { 
                log.Error(ex, "Request api to {0}: {1}", method, query);
                return default; 
            }
        }

    }
}
