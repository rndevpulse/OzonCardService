using System.Text.Json.Serialization;

namespace OzonCard.DeferredRequest.Configuration.Properties.Extensions;


[JsonDerivedType(typeof(ExtensionStringProperty))]
[JsonDerivedType(typeof(ExtensionDateTimeProperty))]
[JsonDerivedType(typeof(ExtensionBoolProperty))]
[JsonDerivedType(typeof(ExtensionGuidProperty))]
[JsonConverter(typeof(ExtensionPropertyJsonConverter))]
public abstract class ExtensionProperty
{
    public string Name { get; set; }
    public string Label { get; set; }
    //поведение в UI
    public PropertyBehaviour Behaviour { get; set; } = PropertyBehaviour.Undefined;
    public abstract string Type { get; }
}