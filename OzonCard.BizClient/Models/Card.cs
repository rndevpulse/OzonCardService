
namespace OzonCard.BizClient.Models
{
    public class Card
    {
        public string id { get; set; }
        public bool isActivated { get; set; }
        public object networkId { get; set; }
        public string number { get; set; }
        public string organizationId { get; set; }
        public string organizationName { get; set; }
        public string track { get; set; }
        public object validToDate { get; set; }
    }
}
