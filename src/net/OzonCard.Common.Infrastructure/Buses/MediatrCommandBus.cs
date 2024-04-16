using MediatR;
using OzonCard.Common.Core;

namespace OzonCard.Common.Infrastructure.Buses;

public class MediatrCommandBus(IMediator mediator) : ICommandBus
{
    /// <inheritdoc />
    public Task<TResult> Send<TResult>(ICommand<TResult> command, CancellationToken ct = default) =>
        mediator.Send(command, ct);

    public Task Send(ICommand command, CancellationToken ct = default) =>
        mediator.Send(command, ct);

}