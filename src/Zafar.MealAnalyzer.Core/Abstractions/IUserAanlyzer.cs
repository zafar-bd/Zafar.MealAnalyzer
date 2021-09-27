using System.Collections.Generic;
using Zafar.MealAnalyzer.Core.Models;

namespace Zafar.MealAnalyzer.Core.Abstractions
{
    public interface IUserAanlyzer
    {
        IEnumerable<MealCounterViewModel> Analyze(MealAnalyzerQueryDto dto);
    }
}
