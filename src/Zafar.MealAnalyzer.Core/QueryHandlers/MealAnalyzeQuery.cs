using MediatR;
using System;
using Zafar.MealAnalyzer.Core.Helpers;
using Zafar.MealAnalyzer.Core.Models;

namespace Zafar.MealAnalyzer.Core.QueryHandlers
{
    public class MealAnalyzeQuery : MealAnalyzerQueryDto, IRequest<string>, ICacheableMediatrQuery
    {
        public bool Cacheable { get; set; }
        public string CacheKey { get; set; }
        public TimeSpan? SlidingExpiration { get; set; }
    }
}
