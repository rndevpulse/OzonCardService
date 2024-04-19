

namespace OzonCard.Common.Application.Customers.Data;

public class CustomersUpload
{
    public Guid OrganizationId { get; set; }
    public IEnumerable<Guid> CategoriesId { get; set; } = new List<Guid>();
    public Guid ProgramId { get; set; }
    public string FileReport { get; set; } = "";
    public decimal Balance { get; set; }
    public CustomersUploadOptions OptionsModel { get; set; } = new ();
    public SingleCustomerUpload? Customer { get; set; }
    
    public Guid UserId { get; private set; }
    public void SetUserId(Guid userId) => UserId = userId;
    public string User { get; private set; } = "";
    public void SetUser(string user) => User = user;
    
    public Guid TaskId { get; private set; }
    public void SetTaskId(Guid taskId) => TaskId = taskId;
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