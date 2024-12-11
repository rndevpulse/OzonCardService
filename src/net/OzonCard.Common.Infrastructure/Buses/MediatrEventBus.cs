

using MediatR;
using OzonCard.Common.Core;

namespace OzonCard.Common.Infrastructure.Buses
{
    public class MediatrEventBus : IEventBus
    {
        private readonly IMediator _mediator;

        public MediatrEventBus(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Publish(IEvent e, CancellationToken ct = default)
        {
            await _mediator.Publish(e, ct);
        }
    }
}