namespace OzonCard.Customer.Api.Models.Organizations;

public class ProgramModel
{
    public ProgramModel(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; }
}