namespace OzonCard.DeferredRequest.Properties.Extensions;

public record ExtensionBoolProperty(
    string Name,
    string Label
) : ExtensionProperty(Name, Label)
{
    public override string Type => nameof(ExtensionBoolProperty);
    public bool Value { get; set; } = false;

}