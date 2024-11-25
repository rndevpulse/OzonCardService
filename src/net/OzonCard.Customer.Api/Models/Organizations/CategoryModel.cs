namespace OzonCard.Customer.Api.Models.Organizations;

public class CategoryModel
{
    public CategoryModel(Guid id, string name, bool isActive)
    {
        Id = id;
        Name = name;
        IsActive = isActive;
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public bool IsActive { get; private set; }

}