using MediatR;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Behaviors
{
    public class ResilienceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private static readonly AsyncRetryPolicy RetryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(3, retryAttempt =>
                TimeSpan.FromMilliseconds(200 * retryAttempt));

        private static readonly AsyncCircuitBreakerPolicy CircuitBreakerPolicy = Policy
            .Handle<Exception>()
            .CircuitBreakerAsync(
                exceptionsAllowedBeforeBreaking: 5,
                durationOfBreak: TimeSpan.FromSeconds(30)
            );

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            return await RetryPolicy.ExecuteAsync(() =>
                CircuitBreakerPolicy.ExecuteAsync(() =>
                    next(cancellationToken)));
        }
    }
}
