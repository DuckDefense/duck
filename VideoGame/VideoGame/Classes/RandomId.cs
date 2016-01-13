using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sandbox.Classes;

namespace VideoGame.Classes {
    public static class RandomId {
        public static Random random = new Random();
        public static List<int> UsedNumbers = new List<int>();

        public static int GenerateRandomUId() {
            var num = random.Next(0, int.MaxValue);
            if (UsedNumbers.Contains(num)) {
                do {
                    num = random.Next(0, int.MaxValue);
                } while (UsedNumbers.Contains(num));
            }
            UsedNumbers.Add(num);
            return num;
        }

        public static int GenerateUserId() {
            //var list = DatabaseConnector.GetPlayerIds();
            var num = random.Next(0, 999999);
            //while (list.Contains(num)) {
            //    num = random.Next(0, 999999);
            //}
            return num;
        }

        public static int GenerateStatsId() {
            //var list = DatabaseConnector.GetStatsIds();
            var num = random.Next(0, 999999);
            //while (list.Contains(num)) {
            //    num = random.Next(999999);
            //}
            return num;
        }
    }
}
