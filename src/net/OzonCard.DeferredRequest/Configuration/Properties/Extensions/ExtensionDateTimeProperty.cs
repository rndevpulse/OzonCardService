namespace OzonCard.DeferredRequest.Configuration.Properties.Extensions;

public class ExtensionDateTimeProperty : ExtensionProperty
{
    public ExtensionDateTimeProperty(string name, string label, PropertyBehaviour behaviour = PropertyBehaviour.Undefined)
    {
        Name = name;
        Label = label;
        Behaviour = behaviour;
    }

    public ExtensionDateTimeProperty()
    {
        Name = string.Empty;
        Label = string.Empty;
    }
    
    public override string Type => nameof(ExtensionDateTimeProperty);
    public DateTimeOffset Value { get; set; }
}