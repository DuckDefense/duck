using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace VideoGame.Classes {

    public enum Kind {
        Physical,
        Special,
        NonDamage
    }

    public class Move {

        public string Name;
        public string Description;
        public int Damage;
        public Kind Kind;
        public int Accuracy;
        public Type Type;

        public Move(string name, string description, int damage, int accuracy, Kind kind, Type type) {
            Name = name;
            Description = description;
            Damage = damage;
            Accuracy = accuracy;
            Kind = kind;
            Type = type;
        }

        /// <summary>
        /// Execute a move
        /// </summary>
        /// <param name="user">Monster that is using this move</param>
        /// <param name="receiver">Monster that is receiving this move</param>
        /// <param name="userMod">Stat modifier for the users stats</param>
        /// <param name="receiverMod">Stat modifier for the receivers stats</param>
        public void Execute(Monster user, Monster receiver, StatModifier userMod, StatModifier receiverMod) {
            if (Kind == Kind.Physical) {
                var phys = Damage*(user.Stats.Attack/receiver.Stats.Defense);
                receiver.Stats.Health -= phys;
            }
            else if (Kind == Kind.Special) {
                var spec = Damage*(user.Stats.Attack/receiver.Stats.Defense);
                receiver.Stats.Health -= spec;
            }
            //TODO: Find out if this will return the same stats if the modifier is empty
            user.Stats = userMod.ApplyModifiers(user);
            receiver.Stats = receiverMod.ApplyModifiers(receiver);
        }

        #region Preset Moves

        #region Physical
        public static Move Tackle() {
            return new Move("Tackle", "A fullbody tackle",
                60, 100, Kind.Physical, Type.Normal);
        }
        #endregion

        #region Special
        public static Move Bubble() {
            return new Move("Bubble", "A fullbody tackle",
                60, 100, Kind.Physical, Type.Water);
        }

        #endregion

        #region NonDamage
        public static Move Glare() {
            return new Move("Glare", "The monster gives a cold glare and slightly lowers opponents speed",
                0, 70, Kind.NonDamage, Type.Normal);
        }
        #endregion
        #endregion
    }
}
