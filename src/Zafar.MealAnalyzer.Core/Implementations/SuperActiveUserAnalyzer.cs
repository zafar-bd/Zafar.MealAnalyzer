using System.Collections.Generic;
using Zafar.MealAnalyzer.Core.Abstractions;
using Zafar.MealAnalyzer.Core.Helpers;
using Zafar.MealAnalyzer.Core.Models;

namespace Zafar.MealAnalyzer.Core.Implementations
{
    public class SuperActiveUserAnalyzer : IUserAanlyzer
    {
        private readonly IDataReader _dataReader;
        public SuperActiveUserAnalyzer(IDataReader dataReader)
        {
            this._dataReader = dataReader;
        }
        public IEnumerable<MealCounterViewModel> Analyze(MealAnalyzerQueryDto dto)
        {
            IEnumerable<MealViewModel> meals = _dataReader.Read();
            IEnumerable<MealCounterViewModel> superActiveUsers = meals
                .ApplyPrimaryFilter(dto)
                .ApplySuperActiveFilter();

            return superActiveUsers;
        }
    }
}
