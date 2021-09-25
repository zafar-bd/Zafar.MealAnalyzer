using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Zafar.MealAnalyzer.Core.Abstractions;
using Zafar.MealAnalyzer.Core.Models;

namespace Zafar.MealAnalyzer.WebApi.Controllers
{
    [Route("api/v1/meals")]
    [ApiController]
    public class MealsController : ControllerBase
    {
        private readonly IMealAnalyzer _dataReader;

        public MealsController(IMealAnalyzer dataReader)
        {
            this._dataReader = dataReader;
        }
        /// <summary>
        /// Get Analyzed meals
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] MealAnalyzerQueryDto dto)
        {
            var data = _dataReader.GetAnalyzedUserId(dto);
            return Ok(data);
        }
    }
}
