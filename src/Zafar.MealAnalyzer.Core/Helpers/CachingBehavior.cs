using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Zafar.MealAnalyzer.Core.Abstractions;

namespace Zafar.MealAnalyzer.Core.Helpers
{
    public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly ICacheHelper _cacheHelper;

        public CachingBehavior(
            ILogger<TResponse> logger,
            IConfiguration configuration,
            ICacheHelper cacheHelper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration;
            _cacheHelper = cacheHelper;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (request is ICacheableMediatrQuery cacheablesMediatorQuery)
            {
                TResponse response;

                if (!cacheablesMediatorQuery.Cacheable)
                    return await next();

                async Task<TResponse> GetResponseAndAddToCache()
                {
                    response = await next();
                    var slidingExpiration = cacheablesMediatorQuery.SlidingExpiration == null
                        ? TimeSpan.FromSeconds(uint.Parse(_configuration["DefaultCacheSlidingExpiration"]))
                        : cacheablesMediatorQuery.SlidingExpiration;
                    await _cacheHelper.SetDataAsync(cacheablesMediatorQuery.CacheKey, response, (TimeSpan)slidingExpiration);
                    return response;
                }

                response = await _cacheHelper.GetDataAsync<TResponse>(cacheablesMediatorQuery.CacheKey);
                if (response != null)
                {
                    _logger.LogInformation($"Fetched from Cache -> '{cacheablesMediatorQuery.CacheKey}'.");
                }
                else
                {
                    response = await GetResponseAndAddToCache();
                    _logger.LogInformation($"Added to Cache -> '{cacheablesMediatorQuery.CacheKey}'.");
                }

                return response;
            }
            else
            {
                return await next();
            }
        }
    }
}