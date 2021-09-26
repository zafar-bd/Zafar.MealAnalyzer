using System.Collections.Generic;
using Zafar.MealAnalyzer.Core.Models;

namespace Zafar.MealAnalyzer.Core.Helpers
{
    public class MealComparer : IEqualityComparer<MealCounterViewModel>
    {
        public bool Equals(MealCounterViewModel x, MealCounterViewModel y)
        {
            //First check if both object reference are equal then return true
            if (object.ReferenceEquals(x, y))
            {
                return true;
            }

            //If either one of the object refernce is null, return false
            if (object.ReferenceEquals(x, null) || object.ReferenceEquals(y, null))
            {
                return false;
            }

            //Comparing all the properties one by one
            return x.UserId == y.UserId;
        }

        public int GetHashCode(MealCounterViewModel obj)
        {
            return obj.UserId.GetHashCode();
        }
    }
}
