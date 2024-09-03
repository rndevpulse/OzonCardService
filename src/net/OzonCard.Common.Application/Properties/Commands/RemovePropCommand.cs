using OzonCard.Common.Core;
using OzonCard.Common.Domain.Props;

namespace OzonCard.Common.Application.Properties.Commands;

public record RemovePropCommand(
    Guid Id
) : ICommand<Property>
{
    public class Handler(
        IPropertiesRepository repository
    ) : ICommandHandler<RemovePropCommand, Property>
    {
        public async Task<Property> Handle(RemovePropCommand request, CancellationToken cancellationToken)
        {
            var prop = await repository.GetItemAsync(request.Id, cancellationToken);
            repository.Remove(prop);
            return prop;
        }
    }
}