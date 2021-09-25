using MealAnalyzer.Core.DataModel;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Zafar.MealAnalyzer.Core.Abstractions;
using Zafar.MealAnalyzer.Core.Models;

namespace MealAnalyzer.Core.Abstractions
{
    public class JsonDataReader : IDataReader
    {
        private readonly IHostingEnvironment _env;
        public JsonDataReader(IHostingEnvironment env)
        {
            this._env = env;
        }

        private IEnumerable<CalendarViewModel> ReadJsonFiles()
        {
            string jsonDataPath = Path.Combine($"{_env.WebRootPath}", "Data");

            foreach (string file in Directory.GetFiles(jsonDataPath, "*.json"))
            {
                using StreamReader rd = new(file);
                string json = rd.ReadToEnd();
                var mealAggregate = JsonConvert.DeserializeObject<MealAggregateViewModel>(json);
                yield return mealAggregate.Calendar;
            }
        }

        public IEnumerable<MealViewModel> Read()
        {
            IEnumerable<CalendarViewModel> calendars = this.ReadJsonFiles();
            foreach (CalendarViewModel calendar in calendars)
            {
                foreach (KeyValuePair<string, DaysDetailViewModel> details in calendar.DaysWithDetails)
                {
                    MealViewModel meal = new()
                    {
                        Date = details.Value.Day.Date,
                        UserId = details.Value.Day.UserId,
                        MealCount = (uint)details.Value.Details.MealsWithDetails.Count
                    };
                    yield return meal;
                }
            }
        }
    }
}
