using System.Collections.Generic;
using Zafar.MealAnalyzer.Core.Models;

namespace Zafar.MealAnalyzer.Core.Abstractions
{
    public interface IMealAnalyzer
    {
        public IEnumerable<MealCounterViewModel> GetAnalyzedMeals(MealAnalyzerQueryDto dto);
        public string GetAnalyzedUserId(MealAnalyzerQueryDto dto);
    }
}
