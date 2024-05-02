using System.Text;
using MediatR;
using Microsoft.Extensions.Logging;
using OzonCard.Common.Core;

namespace OzonCard.Common.Infrastructure.Piplines
{
    public class TransactionPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ITransactionManager _transactions;
        private readonly ILogger<TransactionPipeline<TRequest, TResponse>> _logger;

        public TransactionPipeline(ITransactionManager transactions, ILogger<TransactionPipeline<TRequest, TResponse>> logger)
        {
            _transactions = transactions;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            try
            {
                if (request is ICommand<object>)
                {
                    var trx = _transactions.StartTransaction();
                    var result = await next();
                    if (trx)
                        await _transactions.CommitAsync(cancellationToken);
                    return result;
                }
                return await next();
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "Error while executing transaction scope");
                _transactions.HasError(e);
                throw;
            }
        }

    }
}