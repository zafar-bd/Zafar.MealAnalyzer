using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Zafar.MealAnalyzer.Core.Abstractions;
using Zafar.MealAnalyzer.Core.Helpers;
using Zafar.MealAnalyzer.Core.Models;

namespace Zafar.MealAnalyzer.Core.QueryHandlers
{
    public class MealAnalyzeQueryHandler : IRequestHandler<MealAnalyzeQuery, string>
    {
        private readonly UserAnalyzeServiceResolver _analyzeServiceResover;
        public MealAnalyzeQueryHandler(UserAnalyzeServiceResolver analyzeServiceResover)
        {
            this._analyzeServiceResover = analyzeServiceResover;
        }
        public async Task<string> Handle(MealAnalyzeQuery request, CancellationToken cancellationToken)
        {
            IUserAanlyzer mealAnalyzer = _analyzeServiceResover(request.UserType);
            IEnumerable<MealCounterViewModel> analyzedMeals = mealAnalyzer.Analyze(request);
            string userId = string.Join(',', analyzedMeals
                                                 .OrderByDescending(o => o.MealCount)
                                                 .ThenBy(o => o.UserId)
                                                 .Select(x => x.UserId));
            await Task.CompletedTask;
            return userId;
        }
    }
}
