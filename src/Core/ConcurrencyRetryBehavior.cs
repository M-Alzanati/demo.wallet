using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application
{
    public class ConcurrencyRetryBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly int _maxRetries;
        private readonly TimeSpan _delayBetweenRetries;

        public ConcurrencyRetryBehavior(int maxRetries = 3, TimeSpan? delayBetweenRetries = null)
        {
            _maxRetries = maxRetries;
            _delayBetweenRetries = delayBetweenRetries ?? TimeSpan.FromMilliseconds(200);
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var retryCount = 0;

            while (true)
            {
                try
                {
                    return await next(cancellationToken);
                }
                catch (Exception ex)
                {
                    retryCount++;

                    if (retryCount > _maxRetries)
                        throw;

                    await Task.Delay(_delayBetweenRetries, cancellationToken);
                }
            }
        }
    }
}
