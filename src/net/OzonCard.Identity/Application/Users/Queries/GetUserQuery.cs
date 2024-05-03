using OzonCard.Common.Core;
using OzonCard.Identity.Domain;

namespace OzonCard.Identity.Application.Users.Queries;

public record GetUserQuery(Guid Id) : IQuery<User>;