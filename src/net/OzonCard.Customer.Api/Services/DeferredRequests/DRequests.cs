namespace OzonCard.Customer.Api.Services.DeferredRequests;

public class DRequests
{
    
    public class Categories
    {
        public const string Remove = "d.request.categories.remove";
        public const string Append = "d.request.categories.append";
        
    }
    public class Companies
    {
        public const string Block = "d.request.companies.block";
        public const string UnBlock = "d.request.companies.unblock";
    }

    public class Reports
    {
        public const string Period = "d.request.report.period";
        public const string Transactions = "d.request.report.transactions";
    }
}