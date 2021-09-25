using System;

namespace Zafar.MealAnalyzer.Core.Helpers
{
    public interface ICacheableMediatrQuery
    {
        public bool Cacheable { get; }
        public string CacheKey { get; }
        public TimeSpan? SlidingExpiration { get; }
    }
}
