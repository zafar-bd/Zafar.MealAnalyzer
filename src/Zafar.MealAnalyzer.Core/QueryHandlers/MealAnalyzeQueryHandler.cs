using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Zafar.MealAnalyzer.Core.Abstractions;

namespace Zafar.MealAnalyzer.Core.QueryHandlers
{
    public class MealAnalyzeQueryHandler : IRequestHandler<MealAnalyzeQuery, string>
    {
        private readonly IMealAnalyzer _mealAnalyzer;

        public MealAnalyzeQueryHandler(IMealAnalyzer mealAnalyzer)
        {
            this._mealAnalyzer = mealAnalyzer;
        }

        public async Task<string> Handle(MealAnalyzeQuery request, CancellationToken cancellationToken)
        {
            var userId = _mealAnalyzer.GetAnalyzedUserId(request);
            await Task.CompletedTask;
            return userId;
        }
    }
}
