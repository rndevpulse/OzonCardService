

namespace OzonCard.BizClient.HttpClients
{
    public interface IClient
    {
        Task<T?> Send<T>(string query = "", string method = "GET", object? body = null, int timeout = 180);
    }
}
