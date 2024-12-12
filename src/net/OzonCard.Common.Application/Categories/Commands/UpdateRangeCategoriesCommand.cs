using OzonCard.Common.Core;
using OzonCard.Common.Domain.Files;

namespace OzonCard.Common.Application.Categories.Commands;

public record UpdateRangeCategoriesCommand(
    Guid OrganizationId,
    Guid CategoryId,
    Guid SelectedCategoryId,
    bool IsAppend,
    Guid Tracking,
    Guid UserId
    ) : ICommand<SaveFile>;