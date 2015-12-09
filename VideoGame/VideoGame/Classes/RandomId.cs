using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VideoGame.Classes
{
    public static class RandomId
    {
        public static Random random = new Random();
        public static List<int> UsedNumbers = new List<int>(); 

        public static int GenerateRandomUId()
        {
            var num = random.Next(0, Int32.MaxValue);
            if (UsedNumbers.Contains(num))
            {
                do
                {
                    num = random.Next(0, Int32.MaxValue);
                } while (UsedNumbers.Contains(num));
            }
            UsedNumbers.Add(num);
            return num;
        }
    }
}
