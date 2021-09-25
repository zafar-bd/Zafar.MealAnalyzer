using System.Collections.Generic;

namespace MealAnalyzer.Core.DataModel
{
    public class DetailViewModel
    {
        public Dictionary<string, MealDetailViewModel> MealsWithDetails { get; set; }
    }
}
