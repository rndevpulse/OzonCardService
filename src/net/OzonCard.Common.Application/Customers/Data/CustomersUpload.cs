

using OzonCard.Common.Application.Common;

namespace OzonCard.Common.Application.Customers.Data;

public class CustomersUpload : MemberInfo
{
    public Guid OrganizationId { get; set; }
    public IEnumerable<Guid> CategoriesId { get; set; } = new List<Guid>();
    public Guid ProgramId { get; set; }
    public string FileReport { get; set; } = "";
    public decimal Balance { get; set; }
    public CustomersUploadOptions OptionsModel { get; set; } = new ();
    public SingleCustomerUpload? Customer { get; set; }
   
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