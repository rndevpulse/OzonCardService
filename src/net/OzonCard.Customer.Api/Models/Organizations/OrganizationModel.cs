namespace OzonCard.Customer.Api.Models.Organizations;

public class OrganizationModel
{
    public OrganizationModel(string name, IEnumerable<ProgramModel> programs, IEnumerable<CategoryModel> categories)
    {
        Name = name;
        Programs = programs;
        Categories = categories;
    }

    public string Name { get; private set; }
    public IEnumerable<ProgramModel> Programs { get; private set; }
    public IEnumerable<CategoryModel> Categories { get; private set; }
}