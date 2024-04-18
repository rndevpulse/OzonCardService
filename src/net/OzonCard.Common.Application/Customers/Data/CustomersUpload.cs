

namespace OzonCard.Common.Application.Customers.Data;

public class CustomersUpload
{
    public Guid OrganizationId { get; set; }
    public IEnumerable<Guid> CategoriesId { get; set; } = new List<Guid>();
    public Guid CorporateNutritionId { get; set; }
    public string FileReport { get; set; } = "";
    public double Balance { get; set; }
    public CustomersUploadOptions OptionsModel { get; set; } = new ();
    public SingleCustomerUpload? Customer { get; set; }
    
    public Guid UserId { get; private set; }
    public void SetUserId(Guid userId) => UserId = userId;
}

public class CustomersUploadOptions
{
    public bool RefreshBalance { get; set; }
    public bool Rename { get; set; }
}

public class SingleCustomerUpload
{
    public string Name { get; set; } = "";
    public string Card { get; set; } = "";
}