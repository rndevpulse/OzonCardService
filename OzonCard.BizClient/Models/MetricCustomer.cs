using OzonCard.BizClient.Models.Data;

namespace OzonCard.BizClient.Models
{
    public class MetricCustomer
    {
        public Guid GuestId { get; set; }
        public double Value { get; set; }
        public CounterMetric Metric { get; set; }
        public CounterPeriod Period { get; set; }
    }
}
