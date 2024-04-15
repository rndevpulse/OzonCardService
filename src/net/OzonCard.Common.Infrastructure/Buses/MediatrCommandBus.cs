using MediatR;
using OzonCard.Common.Core;

namespace OzonCard.Common.Infrastructure.Buses;

public class MediatrCommandBus : ICommandBus
{
    private readonly IMediator _mediator;

    public MediatrCommandBus(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc />
    public Task<TResult> Send<TResult>(ICommand<TResult> command, CancellationToken ct = default) =>
        _mediator.Send(command, ct);
}