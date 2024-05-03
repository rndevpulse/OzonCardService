namespace OzonCard.BizClient.Models;

public class ShortGuestInfo
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }
    public DateTime? Birthday { get; set; }
    public string Email { get; set; }
    public string Surname { get; set; }
    public string Sex { get; set; }
    public string Comment { get; set; }
    public DateTime WhenCreated { get; set; }
    public DateTime LastVisitDate { get; set; }
}
