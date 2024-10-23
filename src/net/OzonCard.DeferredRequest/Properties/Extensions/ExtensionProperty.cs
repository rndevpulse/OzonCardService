using System.Text.Json.Serialization;

namespace OzonCard.DeferredRequest.Properties.Extensions;


[JsonDerivedType(typeof(ExtensionStringProperty))]
[JsonDerivedType(typeof(ExtensionDateTimeProperty))]
[JsonDerivedType(typeof(ExtensionBoolProperty))]
[JsonConverter(typeof(ExtensionPropertyJsonConverter))]
public abstract record ExtensionProperty(
    string Name,
    string Label
)
{
    public abstract string Type { get; }
}