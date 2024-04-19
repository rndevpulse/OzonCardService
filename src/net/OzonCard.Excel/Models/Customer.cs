namespace OzonCard.Excel.Models;

public class Customer
{
    public string Name { get; init; } = "";
    public string Card { get; init; } = "";
    public string? TabNumber { get; init; }
    public string? Position { get; init; }
    public string? Division { get; init; }
}