using MediatR;
using OzonCard.Common.Core;

namespace OzonCard.Common.Infrastructure.Buses;

public class MediatrQueryBus : IQueryBus
{
    private readonly IMediator _mediator;

    public MediatrQueryBus(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc />
    public Task<TResult> Send<TResult>(IQuery<TResult> command, CancellationToken ct = default) => 
        _mediator.Send(command, ct);
}