using OzonCard.Common.Core;
using OzonCard.Identity.Domain;

namespace OzonCard.Identity.Application.Users.Queries;

public record GetUsersQuery(): IQuery<IEnumerable<User>>;