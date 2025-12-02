namespace OzonCard.Cloud.Client.Data.OrderTypes;

public record OrderTypesResult(IEnumerable<OrderTypesOrganization> OrderTypes);

public record OrderTypesOrganization(IEnumerable<OrderType> Items, Guid OrganizationId);

public record OrderType(Guid Id, string Name, string OrderServiceType);
