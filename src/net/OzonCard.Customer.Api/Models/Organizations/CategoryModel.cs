namespace OzonCard.Customer.Api.Models.Organizations;

public class CategoryModel
{
    public CategoryModel(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; }
}