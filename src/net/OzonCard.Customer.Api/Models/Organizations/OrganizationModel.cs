namespace OzonCard.Customer.Api.Models.Organizations;

public class OrganizationModel
{
    public OrganizationModel(Guid id, string name, IEnumerable<ProgramModel> programs, IEnumerable<CategoryModel> categories)
    {
        Id = id;
        Name = name;
        Programs = programs;
        Categories = categories;
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public IEnumerable<ProgramModel> Programs { get; private set; }
    public IEnumerable<CategoryModel> Categories { get; private set; }
}