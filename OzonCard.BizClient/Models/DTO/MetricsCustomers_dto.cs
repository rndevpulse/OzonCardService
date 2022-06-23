using OzonCard.BizClient.Models.Data;

namespace OzonCard.BizClient.Models.DTO
{
    public class MetricsCustomers_dto
    {
        public IEnumerable<Guid> guestIds { get; set; }
        public IEnumerable<CounterPeriod> periods { get; set; }
        public IEnumerable<CounterMetric> metrics { get; set; }

    }
}
