namespace OzonCard.DeferredRequest.Configuration.Properties.Extensions;

public class ExtensionBoolProperty : ExtensionProperty
{
    public ExtensionBoolProperty(string name, string label, PropertyBehaviour behaviour = PropertyBehaviour.Undefined)
    {
        Name = name;
        Label = label;
        Behaviour = behaviour;
    }

    public ExtensionBoolProperty()
    {
        Name = string.Empty;
        Label = string.Empty;
    }
    
    public override string Type => nameof(ExtensionBoolProperty);
    public bool Value { get; set; } = false;

}