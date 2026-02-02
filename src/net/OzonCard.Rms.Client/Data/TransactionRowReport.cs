using System.Text.Json.Serialization;

namespace OzonCard.Rms.Client.Data;

public class TransactionRowReport
{
    [JsonPropertyName("CloseTime")]
    public DateTime CloseTime { get; set; }
    
    [JsonPropertyName("Delivery.CustomerCardNumber")]
    public string? Card { get; set; } = string.Empty;
    
    [JsonPropertyName("Delivery.CustomerName")]
    public string Name { get; set; } = string.Empty;
    
    [JsonPropertyName("DishDiscountSumInt")]
    public decimal Sum { get; set; }
}