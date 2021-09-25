using System;

namespace Zafar.MealAnalyzer.Core.Models
{
    public class MealAnalyzerQueryDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public UserType UserType { get; set; }
        public bool Cachable { get; set; } = true;
    }

    public enum UserType : uint
    {
        /// <summary>
        /// Active at least 5 meals
        /// </summary>
        ACTIVE = 5,
        /// <summary>
        /// greater than 10 meals
        /// </summary>
        SUPERACTIVE = 10,
        BORED
    }

}
