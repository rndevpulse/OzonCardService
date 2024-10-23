namespace OzonCard.DeferredRequest.Properties.Extensions;

public record ExtensionDateTimeProperty(
    string Name,
    string Label
) : ExtensionProperty(Name, Label)
{
    public override string Type => nameof(ExtensionDateTimeProperty);
    public DateTimeOffset Value { get; set; }
}