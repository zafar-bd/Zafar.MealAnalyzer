using System.Collections.Generic;
using Zafar.MealAnalyzer.Core.Models;

namespace Zafar.MealAnalyzer.Core.Abstractions
{
    public interface IDataReader
    {
        IEnumerable<MealViewModel> Read();
    }
}
