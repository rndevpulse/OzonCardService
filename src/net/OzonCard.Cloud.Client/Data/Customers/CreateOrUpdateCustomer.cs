namespace OzonCard.Cloud.Client.Data.Customers;

public class CreateOrUpdateCustomer
{
    public CreateOrUpdateCustomer(Guid organizationId)
    {
        OrganizationId = organizationId;
    }

    public string? Phone { get; set; }
    public string? CardTrack { get; set; }
    public string? CardNumber { get; set; }
    public string? Name { get; set; }
    public string? MiddleName { get; set; }
    public string? SurName { get; set; }
    public DateTime? Birthday { get; set; }
    public Guid? Id { get; set; }
    public bool? IsDeleted { get; set; } 
    public Guid OrganizationId { get; set; }
    

}