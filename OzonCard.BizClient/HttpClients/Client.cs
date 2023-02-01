using Newtonsoft.Json;
using Serilog;
using System.Text;

namespace OzonCard.BizClient.HttpClients
{
    public class Client : IClient
    {
        private readonly ILogger log = Log.ForContext(typeof(Client));
        private readonly HttpClient _httpClient;
        readonly int TimeDelayThread = 500;
        public Client(HttpClient httpClient)
        {
            _httpClient = httpClient;
            
        }

        public async Task<T?> Send<T>(string query = "", string method = "GET", object? body = null, int timeout = 180)
        {
            _httpClient.Timeout =  TimeSpan.FromSeconds(timeout);
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
                    else
                        return default;
                }
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    throw new UnauthorizedAccessException();
                if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                {
                    log.Debug("TooManyRequests api to {0}: {1}", method, query);
                    Thread.Sleep(TimeDelayThread);
                    return await Send<T>(query, method, body);
                }
                log.Error($"request to {request.RequestUri} with {JsonConvert.SerializeObject(body)} \n {await response.Content.ReadAsStringAsync()}");
                return default;
            }
            catch (UnauthorizedAccessException) { 
                log.Error("UnauthorizedAccessException Request api to {0}: {1}", method, query);
                throw new UnauthorizedAccessException(); 
            }
            catch (Exception ex)
            {
                log.Error(ex, "Request api to {0}: {1}", method, query);
                return default;
            }
        }

    }
}
