namespace OzonCard.Cloud.Client.Data.TerminalTablesGroups;

public record TerminalGroupsResult(IEnumerable<TerminalGroupsOrganization> TerminalGroups);
public record TerminalGroupsOrganization(IEnumerable<TerminalGroup> Items, Guid OrganizationId);
public record TerminalGroup(Guid Id, string Name);
