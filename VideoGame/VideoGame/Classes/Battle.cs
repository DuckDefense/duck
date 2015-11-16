using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoGame.Classes {
    public enum State {
        Won,
        Loss,
        Ran
    }

    public enum Selection {
        Attack,
        Item,
        Party,
        Run
    }

    class Battle {
        public Character User;
        public Character Opponent;
        public Monster CurrentUserMonster;
        public Monster CurrentOpponentMonster;
        public Selection Selection;
        /// <summary>
        /// Battle with a trainer
        /// </summary>
        public Battle(Character user, Character opponent) {
            User = user;
            Opponent = opponent;
            CurrentUserMonster = User.Monsters[0];
            CurrentOpponentMonster = Opponent.Monsters[0];
        }

        /// <summary>
        /// Battle with a wild monster
        /// </summary>
        public Battle(Character user, Monster opponent) {
            User = user;
            CurrentUserMonster = User.Monsters[0];
            CurrentOpponentMonster = opponent;
        }

        public void Start() {
            //Store stats so the battle won't alter the stats permanently
            CurrentUserMonster.PreviousStats = CurrentUserMonster.Stats;
            CurrentOpponentMonster.PreviousStats = CurrentOpponentMonster.Stats;

            //Get Ability effects here
            CurrentUserMonster.Ability.GetEffects(CurrentUserMonster, CurrentOpponentMonster);
            CurrentOpponentMonster.Ability.GetEffects(CurrentOpponentMonster, CurrentUserMonster);

            //Choose action here, wether its an attack, using an item or switching out a monster
            //TODO: Make this async
            LoopTurns();


            //Restore the stats when the battle is over, or when the monster has been switched out

            //Add experience here so the stats will still be updated if the monster levels up

        }

        public void LoopTurns() {

            var userSpeed = CurrentUserMonster.Stats.Speed;
            var opponentSpeed = CurrentOpponentMonster.Stats.Speed;

            switch (Selection) {
            case Selection.Attack:
                Selection = Selection.Attack;
                //Draw moves here
                if (userSpeed > opponentSpeed) {
                    //User goes before opponent
                }
                else {
                    //Opponent goes first
                }
                break;
            case Selection.Item:
                Selection = Selection.Item;
                break;
            case Selection.Party:
                Selection = Selection.Party;
                break;
            case Selection.Run:
                Selection = Selection.Run;
                break;
            }
        }

        public void Attack(Monster user, Monster opponent, int chosen) {
            //Execute chosen move here
            user.Moves[chosen].Execute(user, opponent);
            //Wait for the move to complete
            opponent.Moves[chosen].Execute(opponent, user);

        }
    }
}
