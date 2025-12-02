using System.Text.Json;
using System.Text.Json.Serialization;

namespace OzonCard.Cloud.Client;

public class DateTimeConverter: JsonConverter<DateTime>
{
   
    private string DateTimeFormat { get; set; } = "yyyy-MM-dd HH:mm:ss.fff";

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return DateTime.ParseExact(reader.GetString()!, DateTimeFormat, null);
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(DateTimeFormat));
    }
}