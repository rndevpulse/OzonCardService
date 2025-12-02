using System.Text.Json.Serialization;

namespace OzonCard.Cloud.Client.Data.Customers;

public class Customer
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string MiddleName { get; set; }
    public string Comment { get; set; }
    public string Phone { get; set; }
    [JsonConverter(typeof(DateTimeConverter))] public DateTime? Birthday { get; set; }
    public string Email { get; set; }
    public int Sex { get; set; }
    public int ConsentStatus { get; set; }
    public bool Anonymized { get; set; }
    public IEnumerable<Card> Cards { get; set; }
    public IEnumerable<Category> Categories { get; set; }
    public IEnumerable<WalletBalance> WalletBalances { get; set; }
    public string UserData { get; set; }
    public bool IsDeleted { get; set; }
    [JsonConverter(typeof(DateTimeConverter))] public DateTime? WhenRegistered { get; set; }
    [JsonConverter(typeof(DateTimeConverter))] public DateTime? LastProcessedOrderDate { get; set; }
    [JsonConverter(typeof(DateTimeConverter))] public DateTime? FirstOrderDate { get; set; }
    public Guid? LastVisitedOrganizationId { get; set; }
    public Guid? RegistrationOrganizationId { get; set; }
}
public class Card
{
    public Guid Id { get; set; }
    public string Track { get; set; }
    public string Number { get; set; }
    [JsonConverter(typeof(DateTimeConverter))] public DateTime? ValidToDate { get; set; }
}

public class Category
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public bool IsDefaultForNewGuests { get; set; }
}
public class WalletBalance
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public WalletBalanceType Type { get; set; }
    public decimal Balance { get; set; }
}

public enum WalletBalanceType
{
    Deposit = 0,
    Bonus = 1, 
    Products = 2,
    Discount = 3,
    Certificate = 4,
}