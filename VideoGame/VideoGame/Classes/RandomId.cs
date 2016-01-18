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
            var list = DatabaseConnector.GetUids();
            var num = random.Next(2, int.MaxValue);
            while (list.Contains(num)) {
                num = random.Next(2, int.MaxValue);
            }
            return num;
        }

        public static int GenerateUserId() {
            var list = DatabaseConnector.GetPlayerIds();
            var num = random.Next(2, 999999);
            while (list.Contains(num)) {
                num = random.Next(2, 999999);
            }
            return num;
        }

        public static int GenerateLinkId() {
            var list = DatabaseConnector.GetLinkIds();
            var num = random.Next(2, int.MaxValue);
            while (list.Contains(num)) {
                num = random.Next(2, int.MaxValue);
            }
            return num;
        }

        public static int GenerateStatsId() {
            var list = DatabaseConnector.GetStatsIds();
            var num = random.Next(2, 999999);
            while (list.Contains(num)) {
                num = random.Next(2, 999999);
            }
            return num;
        }
    }
}
