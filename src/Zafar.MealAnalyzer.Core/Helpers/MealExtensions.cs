using System.Collections.Generic;
using System.Linq;
using Zafar.MealAnalyzer.Core.Models;

namespace Zafar.MealAnalyzer.Core.Helpers
{
    public static class MealExtensions
    {
        public static IEnumerable<MealCounterViewModel> ApplyPrimaryFilter(this IEnumerable<MealViewModel> meals, MealAnalyzerQueryDto dto)
        => meals.Where(a => a.Date.Date >= dto.StartDate.Date
                         && a.Date.Date <= dto.EndDate.Date)
                .GroupBy(a => a.UserId)
                .Select(a => new MealCounterViewModel
                {
                    UserId = a.Key,
                    MealCount = (uint)a.Sum(b => b.MealCount)
                });

        public static IEnumerable<MealCounterViewModel> ApplyActiveFilter(this IEnumerable<MealCounterViewModel> meals)
        => meals.Where(a => a.MealCount >= (uint)UserType.ACTIVE
                         && a.MealCount <= (uint)UserType.SUPERACTIVE);

        public static IEnumerable<MealCounterViewModel> ApplySuperActiveFilter(this IEnumerable<MealCounterViewModel> meals)
         => meals.Where(a => a.MealCount >= (uint)UserType.SUPERACTIVE);

        public static IEnumerable<MealCounterViewModel> ApplyBoredFilter(this IEnumerable<MealViewModel> meals, MealAnalyzerQueryDto dto)
        {
            IEnumerable<MealCounterViewModel> filteredMeals = meals.ApplyPrimaryFilter(dto);

            var activeThresholdMeals = filteredMeals
                             .Where(a => a.MealCount >= (uint)UserType.ACTIVE)
                             .ToList();

            var activeUsersInThePrecedingPeriod = meals
             .Where(a => a.Date.Date < dto.StartDate.Date)
             .GroupBy(a => a.UserId)
             .Select(a => new MealCounterViewModel
             {
                 UserId = a.Key,
                 MealCount = (uint)a.Sum(b => b.MealCount)
             })
             .Where(a => a.MealCount >= (uint)UserType.ACTIVE)
             .ToList();

            return activeUsersInThePrecedingPeriod.Except(activeThresholdMeals).ToList();
        }

        public static IEnumerable<MealCounterViewModel> ApplyActiveThreshold(this IEnumerable<MealViewModel> meals, MealAnalyzerQueryDto dto)
        {
            IEnumerable<MealCounterViewModel> filteredMeals = meals.ApplyPrimaryFilter(dto);

            return dto.UserType switch
            {
                UserType.ACTIVE => filteredMeals.ApplyActiveFilter(),
                UserType.SUPERACTIVE => filteredMeals.ApplySuperActiveFilter(),
                UserType.BORED => new List<MealCounterViewModel>(),
                _ => new List<MealCounterViewModel>(),
            };
        }
    }
}
