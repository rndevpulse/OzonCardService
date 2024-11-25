using OzonCard.Common.Core;

namespace OzonCard.Common.Application.Categories.Commands;

public record UpdateRangeCategoriesCommand(
    Guid OrganizationId,
    Guid CategoryId,
    Guid SelectedCategoryId,
    bool IsAppend,
    Guid Tracking
    ) : ICommand<int>;