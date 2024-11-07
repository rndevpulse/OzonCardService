using System.Text.Json.Serialization;

namespace OzonCard.DeferredRequest.Configuration.Properties.Extensions;


[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PropertyBehaviour
{
    Undefined,
    OrganizationId,
    ProgramId,
    CategoryId,
    BatchId,
    DateStart,
    DateEnd,
}