using System;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Infrastructure.Behaviors
{
    public class ConcurrencyRetryBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private const int MaxRetryCount = 3;

        public ConcurrencyRetryBehavior()
        {
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            for (var retry = 0; retry < MaxRetryCount; retry++)
            {
                try
                {
                    return await next(cancellationToken);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    Trace.TraceError(ex.Message, "Concurrency conflict detected. Retrying {RetryCount}...", retry + 1);

                    if (retry == MaxRetryCount - 1)
                        throw;

                    await Task.Delay(50, cancellationToken);
                }
            }

            throw new InvalidOperationException("Failed to process due to concurrency issues.");
        }
    }
}
