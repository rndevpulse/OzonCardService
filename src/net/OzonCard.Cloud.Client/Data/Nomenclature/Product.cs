namespace OzonCard.Cloud.Client.Data.Nomenclature;

public class Product : Folder
{
    public string? Code { get; protected set;}

    
    public Product(
        Guid id, string name, string type, string description, string image, Guid? parent, 
        string? code
    ) : base(id, name, type, description, image, parent)
    {
        Code = code;
    }
}