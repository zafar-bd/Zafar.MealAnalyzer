using System;
using System.Threading.Tasks;

namespace Zafar.MealAnalyzer.Core.Abstractions
{
    public interface ICacheHelper
    {
        Task SetDataAsync(string cacheKey, object value, TimeSpan duration);

        Task<T> GetDataAsync<T>(string cacheKey);
    }
}
