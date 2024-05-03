namespace OzonCard.Biz.Client.Models.Organizations;

public class OrganizationDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public bool IsActive { get; set; }
}