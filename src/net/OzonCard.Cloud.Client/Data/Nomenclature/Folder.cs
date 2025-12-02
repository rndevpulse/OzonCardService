namespace OzonCard.Cloud.Client.Data.Nomenclature;

public class Folder
{
    public Guid? Parent { get;  protected set;}
    public Guid Id { get;  protected set;}
    public string Name { get; protected set;}
    public string Description { get; protected set;}
    public string Type { get; protected set;} 

    public string Image { get;  protected set;}

    public Folder(Guid id, string name, string type, string description, string image, Guid? parent)
    {
        Id = id;
        Name = name;
        Type = type;
        Description = description;
        Image = image;
        Parent = parent;
    }
}