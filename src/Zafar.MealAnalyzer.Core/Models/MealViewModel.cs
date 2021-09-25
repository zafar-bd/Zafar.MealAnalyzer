using System;

namespace Zafar.MealAnalyzer.Core.Models
{
    public class MealViewModel
    {
        public DateTime Date { get; set; }
        public uint UserId { get; set; }
        public uint MealCount { get; set; }
    }
}
