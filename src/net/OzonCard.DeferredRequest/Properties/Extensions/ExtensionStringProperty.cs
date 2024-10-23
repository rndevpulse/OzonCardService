namespace OzonCard.DeferredRequest.Properties.Extensions;

public record ExtensionStringProperty(
    string Name,
    string Label
) : ExtensionProperty(Name, Label)
{
    public override string Type => nameof(ExtensionStringProperty);
    public string Value { get; set; } = string.Empty;
}