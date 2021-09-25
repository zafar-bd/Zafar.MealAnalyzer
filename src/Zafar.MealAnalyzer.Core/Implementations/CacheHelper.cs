using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Zafar.MealAnalyzer.Core.Abstractions;

namespace Zafar.MealAnalyzer.Core.Implementations
{
    public class CacheHelper : ICacheHelper
    {
        private readonly IDistributedCache _distributedCache;

        public CacheHelper(IDistributedCache distributedCache)
        {
            this._distributedCache = distributedCache;
        }

        public async Task SetDataAsync(string cacheKey, object value, TimeSpan duration)
        {
            var data = JsonConvert.SerializeObject(value);
            await _distributedCache.SetStringAsync(cacheKey, data,
                  new DistributedCacheEntryOptions
                  {
                      SlidingExpiration = duration,
                      AbsoluteExpirationRelativeToNow = duration
                  });
        }

        public async Task<T> GetDataAsync<T>(string cacheKey)
        {
            var ipInCache = await _distributedCache.GetStringAsync(cacheKey);
            if (ipInCache is not null)
            {
                var data = JsonConvert.DeserializeObject<T>(ipInCache);
                return data;
            }
            return default;
        }

    }
}
