using System;
using System.Collections.Generic;
using System.Linq;
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
                UserType.ACTIVE => meals.ApplyActiveThreshold(dto),
                UserType.SUPERACTIVE => meals.ApplyActiveThreshold(dto),
                UserType.BORED => meals.ApplyBoredFilter(dto),
                _ => throw new ArgumentException("Invalid User Type"),
            };
        }

        public string GetAnalyzedUserId(MealAnalyzerQueryDto dto)
        {
            var analyzedMeals = this.GetAnalyzedMeals(dto);
            return string.Join(',', analyzedMeals.Select(x => x.UserId));
        }
    }
}
