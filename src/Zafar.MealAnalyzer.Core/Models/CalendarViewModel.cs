using System.Collections.Generic;

namespace MealAnalyzer.Core.DataModel
{
    public class CalendarViewModel
    {
        public Dictionary<string, DaysDetailViewModel> DaysWithDetails { get; set; }
    }
}
