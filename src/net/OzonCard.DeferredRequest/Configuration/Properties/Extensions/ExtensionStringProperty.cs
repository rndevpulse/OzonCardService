namespace OzonCard.DeferredRequest.Configuration.Properties.Extensions;

public class ExtensionStringProperty : ExtensionProperty
{
    public ExtensionStringProperty(string name, string label, PropertyBehaviour behaviour = PropertyBehaviour.Undefined)
    {
        Name = name;
        Label = label;
        Behaviour = behaviour;
    }

    public ExtensionStringProperty()
    {
        Name = string.Empty;
        Label = string.Empty;
    }
    
    public override string Type => nameof(ExtensionStringProperty);
    public string Value { get; set; } = string.Empty;
}