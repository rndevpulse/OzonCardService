

namespace OzonCard.BizClient.Models.DTO
{
    public class CustomerBiz_dto
    {
        public Customer_dto customer { get; set; }
        public CustomerBiz_dto() => customer = new Customer_dto();

    }
}
