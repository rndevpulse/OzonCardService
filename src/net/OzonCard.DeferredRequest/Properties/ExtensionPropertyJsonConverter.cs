using System.Text.Json;
using System.Text.Json.Serialization;
using OzonCard.DeferredRequest.Properties.Extensions;

namespace OzonCard.DeferredRequest.Properties;

public class ExtensionPropertyJsonConverter : JsonConverter<ExtensionProperty>
{
    public override bool CanConvert(Type typeToConvert) =>
        typeof(ExtensionProperty).IsAssignableFrom(typeToConvert);

    public override ExtensionProperty Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using (var jsonDoc = JsonDocument.ParseValue(ref reader))
        {
            if (jsonDoc.RootElement.TryGetProperty("Type", out var property)
                || jsonDoc.RootElement.TryGetProperty("type", out property))
            {
                switch (property.ToString())
                {
                    case nameof(ExtensionStringProperty):
                        return jsonDoc.RootElement.Deserialize<ExtensionStringProperty>(options)
                               ?? new ExtensionStringProperty(string.Empty, string.Empty);
                    case nameof(ExtensionBoolProperty):
                        return jsonDoc.RootElement.Deserialize<ExtensionBoolProperty>(options)
                               ?? new ExtensionBoolProperty(string.Empty, string.Empty);
                    case nameof(ExtensionDateTimeProperty):
                        return jsonDoc.RootElement.Deserialize<ExtensionDateTimeProperty>(options)
                               ?? new ExtensionDateTimeProperty(string.Empty, string.Empty);
                    default:
                        throw new JsonException($"'Value' doesn't match a known derived type: {jsonDoc.RootElement}");
                }
            }
            throw new JsonException($"'Value' doesn't match a known derived type: {jsonDoc.RootElement}");
        }
    }

    public override void Write(Utf8JsonWriter writer, ExtensionProperty member, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, (object)member, options);
    }
    
}