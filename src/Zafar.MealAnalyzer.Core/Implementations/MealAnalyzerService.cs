using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zafar.MealAnalyzer.Core.Abstractions;
using Zafar.MealAnalyzer.Core.Helpers;
using Zafar.MealAnalyzer.Core.Models;

namespace Zafar.MealAnalyzer.Core.Implementations
{
    public class MealAnalyzerService : IMealAnalyzer
    {
        private readonly IDataReader _dataReader;
        public MealAnalyzerService(IDataReader dataReader)
        {
            this._dataReader = dataReader;
        }

        public IEnumerable<MealCounterViewModel> GetAnalyzedMeals(MealAnalyzerQueryDto dto)
        {
            IEnumerable<MealViewModel> meals = _dataReader.Read();

            return dto.UserType switch
            {
                UserType.ACTIVE => GetActiveUsers(dto, meals),
                UserType.SUPERACTIVE => GetActiveUsers(dto, meals),
                UserType.BORED => meals.ApplyBoredFilter(dto),
                _ => default,
            };
        }

        public string GetAnalyzedUserId(MealAnalyzerQueryDto dto)
        {
            var analyzedMeals = this.GetAnalyzedMeals(dto);
            return string.Join(',', analyzedMeals.Select(x => x.UserId));
        }

        private static IEnumerable<MealCounterViewModel> GetActiveUsers(MealAnalyzerQueryDto dto, IEnumerable<MealViewModel> meals)
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
