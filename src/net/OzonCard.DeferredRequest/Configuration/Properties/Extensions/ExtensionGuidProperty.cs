namespace OzonCard.DeferredRequest.Configuration.Properties.Extensions;

public class ExtensionGuidProperty: ExtensionProperty
{
    public ExtensionGuidProperty(string name, string label, PropertyBehaviour behaviour = PropertyBehaviour.Undefined)
    {
        Name = name;
        Label = label;
        Behaviour = behaviour;
    }

    public ExtensionGuidProperty()
    {
        Name = string.Empty;
        Label = string.Empty;
    }
    public override string Type => nameof(ExtensionGuidProperty);
    public Guid Value { get; set; } = Guid.Empty;

}