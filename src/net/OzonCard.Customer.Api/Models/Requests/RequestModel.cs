using OzonCard.DeferredRequest.Configuration.Properties.Extensions;

namespace OzonCard.Customer.Api.Models.Requests;

public record RequestModel(
    DateTimeOffset Schedule,
    int TimeOffset,
    IEnumerable<ExtensionProperty> Properties
);