using System.Text.Json.Serialization;

namespace OzonCard.Rms.Client.Data;

public class CustomerRowReport
{
    [JsonPropertyName("UniqOrderId.OrdersCount")]
    public int Count { get; set; }
    
    [JsonPropertyName("Delivery.CustomerCardNumber")]
    public string Card { get; set; } = string.Empty;
    
    [JsonPropertyName("Delivery.CustomerName")]
    public string Name { get; set; } = string.Empty;
}