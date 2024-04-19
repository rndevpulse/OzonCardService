namespace OzonCard.Biz.Client.Models.Customers;

public class ShortGuestInfoDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public string Phone { get; set; } = "";
    public DateTime WhenCreated { get; set; }
    public DateTime LastVisitDate { get; set; }
}