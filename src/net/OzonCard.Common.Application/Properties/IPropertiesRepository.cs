using OzonCard.Common.Core;
using OzonCard.Common.Domain.Props;

namespace OzonCard.Common.Application.Properties;

public interface IPropertiesRepository : IRepository<Property>
{
    Task<IEnumerable<Property>> GetItemsAsync(Guid reference, PropType type, CancellationToken ct = default);
}