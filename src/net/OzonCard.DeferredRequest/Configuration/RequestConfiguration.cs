using OzonCard.DeferredRequest.Configuration.Properties.Extensions;

namespace OzonCard.DeferredRequest.Configuration;

public record RequestConfiguration(
    DateTimeOffset Schedule,
    int TimeOffset,
    IEnumerable<ExtensionProperty> Properties,
    IDictionary<string, object>? Features 
);