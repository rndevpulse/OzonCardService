using OzonCard.Common.Core;
using OzonCard.Common.Domain.Files;

namespace OzonCard.Common.Application.Files.Queries;

public record GetFilesQuery(string? User):IQuery<IEnumerable<SaveFile>>;