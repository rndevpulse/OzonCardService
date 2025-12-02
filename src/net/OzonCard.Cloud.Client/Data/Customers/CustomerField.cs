using System.Text.Json.Serialization;

namespace OzonCard.Cloud.Client.Data.Customers;


[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CustomerField
{
    Phone,
    CardTrack,
    CardNumber,
    Id,
}