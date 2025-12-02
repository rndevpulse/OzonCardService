namespace OzonCard.Cloud.Client.Data.Checkin;

public class CalculateCheckin
{
    public string? Coupon { get; set; }
    public Guid? TerminalGroupId { get; set; }
    public Guid OrganizationId { get; }
    
    public CalculateCheckinOrder Order { get; set; }

    public CalculateCheckin(Guid organizationId, Guid orderTypeId, string phone,
        IEnumerable<CalculateCheckinOrderItem> items)
    {
        OrganizationId = organizationId;
        Order = new CalculateCheckinOrder(orderTypeId, phone, items);
    }
}

public record CalculateCheckinOrder(
    Guid OrderTypeId,
    string Phone,
    IEnumerable<CalculateCheckinOrderItem> Items
);

public record CalculateCheckinOrderItem(
    Guid ProductId,
    Guid PositionId,
    decimal Price,
    decimal Amount,
    string Type = "Product"
);

