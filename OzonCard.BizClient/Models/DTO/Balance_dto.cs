
namespace OzonCard.BizClient.Models.DTO
{
    public class Balance_dto
    {
        public Guid customerId { get; set; }
        public Guid organizationId { get; set; }
        public Guid walletId { get; set; }
        public double sum { get; set; }
    }
}
