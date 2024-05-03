using OzonCard.Common.Core;

namespace OzonCard.Common.Application.Customers.Commands;

public record CustomerUpdateCategoryCommand(
    Guid Id,
    IEnumerable<Guid> Categories,
    bool IsRemove
) : ICommand<IEnumerable<string>>;
