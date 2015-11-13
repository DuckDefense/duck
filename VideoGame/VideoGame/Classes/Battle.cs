using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VideoGame.Classes {
    public enum State {
        Won,
        Loss,
        Ran
    }
    class Battle {
        public List<Monster> UserMonsters;
        public List<Monster> OpponentMonsters = new List<Monster>();
        public Monster User;
        public Monster Opponent;

        /// <summary>
        /// Battle with a trainer
        /// </summary>
        /// <param name="userMonsters">First monster in users party</param>
        /// <param name="opponentMonsters">First monster in opponents party</param>
        public Battle(List<Monster> userMonsters, List<Monster> opponentMonsters) {
            User = userMonsters[0];
            Opponent = opponentMonsters[0];
            UserMonsters = userMonsters;
            OpponentMonsters = opponentMonsters;
        }

        /// <summary>
        /// Battle with a wild monster
        /// </summary>
        /// <param name="userMonsters">First monster in party</param>
        /// <param name="wildMonster"></param>
        public Battle(List<Monster> userMonsters, Monster wildMonster) {
            User = userMonsters[0];
            UserMonsters = userMonsters;
            Opponent = wildMonster;
        }

        #region BattleOptions NotDone

        public void Attack() {
            //Show all known moves of the monster
        }

        public void Items() {
            //Show all items in the inventory
        }

        public void Party() {
            //Show all monsters in inventory

            //If monster is switched out
            User.Stats = User.PreviousStats;
            //Select new monster here
            
            
            User.PreviousStats = User.Stats;
        }

        public void Run() {
            //Run away from the monster, chance to run away is decided if users monsters speed is higher than the opponents speed
        } 
        #endregion

        public void StartBattle(Monster user, Monster receiver) {
            //Store stats so the battle won't alter the stats permanently
            user.PreviousStats = user.Stats;
            receiver.PreviousStats = receiver.Stats;
            

            //Restore the stats when the battle is over, or when the monster has been switched out
            user.Stats = user.PreviousStats;
            receiver.Stats = receiver.PreviousStats;

            //Add experience here so the stats will still be updated if the monster levels up
            
        }
    }
}
