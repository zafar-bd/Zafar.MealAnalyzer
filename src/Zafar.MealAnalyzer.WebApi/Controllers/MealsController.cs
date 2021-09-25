using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Zafar.MealAnalyzer.Core.Abstractions;
using Zafar.MealAnalyzer.Core.Models;
using Zafar.MealAnalyzer.Core.QueryHandlers;

namespace Zafar.MealAnalyzer.WebApi.Controllers
{
    [Route("api/v1/meals")]
    [ApiController]
    public class MealsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public MealsController(IMediator dataReader)
        {
            this._mediator = dataReader;
        }
        /// <summary>
        /// Get Analyzed meals
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] MealAnalyzerQueryDto dto)
        {
            var userId = await _mediator.Send(new MealAnalyzeQuery
            {
                Cacheable = true,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                UserType = dto.UserType,
                CacheKey = $"{dto.StartDate}-{dto.EndDate}-{dto.UserType}"
            });

            return Ok(userId);
        }
    }
}
