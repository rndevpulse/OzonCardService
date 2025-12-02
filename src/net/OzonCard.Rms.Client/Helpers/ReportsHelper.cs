using System.Text.Json.Serialization;

namespace OzonCard.Rms.Client.Helpers;

public class ReportsHelper(DateTime from, DateTime to, string paymentName)
{
    public RequestOlap ShortSales => new RequestOlap(
        new[]
        {
            "Delivery.CustomerCardNumber",
            "Delivery.CustomerName"
        },
        new[]
        {
            "UniqOrderId.OrdersCount"
        },
        new Dictionary<string, object>()
        {
            {"OpenDate.Typed", new {
                FilterType = "DateRange",
                PeriodType = "CUSTOM",
                From = from.ToString("yyyy-MM-dd"), 
                To = to.ToString("yyyy-MM-dd"),
                IncludeLow = true,
                IncludeHigh = false
            }},
            { "OrderDeleted", new {
                    FilterType = "IncludeValues",
                    Values = new[] { "NOT_DELETED" }
            }},
            { "DeletedWithWriteoff", new {
                FilterType = "IncludeValues",
                Values = new[] { "NOT_DELETED" }
            }},
            { "PayTypes", new {
                FilterType = "IncludeValues",
                Values = new[] { paymentName }
            }},

        });
    
    public RequestOlap TransactionSales => new RequestOlap(
        new[]
        {
            "CloseTime",
            "Delivery.CustomerCardNumber",
            "Delivery.CustomerName"
        },
        [],
        new Dictionary<string, object>()
        {
            {"OpenDate.Typed", new {
                FilterType = "DateRange",
                PeriodType = "CUSTOM",
                From = from.ToString("yyyy-MM-dd"), 
                To = to.ToString("yyyy-MM-dd"),
                IncludeLow = true,
                IncludeHigh = false
            }},
            { "OrderDeleted", new {
                FilterType = "IncludeValues",
                Values = new[] { "NOT_DELETED" }
            }},
            { "DeletedWithWriteoff", new {
                FilterType = "IncludeValues",
                Values = new[] { "NOT_DELETED" }
            }},
            { "PayTypes", new {
                FilterType = "IncludeValues",
                Values = new[] { paymentName }
            }},

        });

    
}

public record RequestOlap(
    [property: JsonPropertyName("groupByRowFields")] string[] RowFields,
    [property: JsonPropertyName("aggregateFields")] string[] AggregateFields,
    [property: JsonPropertyName("filters")] IDictionary<string, object>? Filters = null,
    [property: JsonPropertyName("reportType")] string ReportType = "SALES",
    [property: JsonPropertyName("buildSummary")] bool Summary = false
    
);