using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.AccessControl;
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
        public StatModifier HitModifier;
        public StatModifier MissModifier;
        private double damageMod = 1;

        public Move(string name, string description, int damage, int accuracy, Kind kind, Type type) {
            Name = name;
            Description = description;
            Damage = damage;
            Accuracy = accuracy;
            Kind = kind;
            Type = type;
        }
        public Move(string name, string description, int damage, int accuracy, StatModifier hitModifier, StatModifier missModifier, Kind kind, Type type) {
            Name = name;
            Description = description;
            Damage = damage;
            Accuracy = accuracy;
            HitModifier = hitModifier;
            MissModifier = missModifier;
            Kind = kind;
            Type = type;
        }

        /// <summary>
        /// Execute a move
        /// </summary>
        /// <param name="user">Monster that is using this move</param>
        /// <param name="receiver">Monster that is receiving this move</param>
        public void Execute(Monster user, Monster receiver) {
            //Check if types are very effective or not very effective
            GetDamageModifier(receiver);
            //Check if the move hit
            var chanceToHit = Accuracy - (user.Stats.Speed - receiver.Stats.Speed);
            Random rand = new Random();
            //If random number is below chanceToHit the move did hit
            if (rand.Next(0, 100) <= chanceToHit) {
                if (Kind == Kind.Physical) {
                    var phys = Convert.ToInt32((Damage*(user.Stats.Attack/receiver.Stats.Defense))*damageMod);
                    receiver.Stats.Health -= phys;
                }
                else if (Kind == Kind.Special) {
                    var spec =
                        Convert.ToInt32((Damage*(user.Stats.SpecialAttack/receiver.Stats.SpecialDefense))*damageMod);
                    receiver.Stats.Health -= spec;
                }
                //TODO: Find out if this will return the same stats if the modifier is empty
                user.Stats = HitModifier.ApplyModifiers(user);
                receiver.Stats = HitModifier.ApplyModifiers(receiver);
            }
            else {
                user.Stats = MissModifier.ApplyModifiers(user);
                receiver.Stats = MissModifier.ApplyModifiers(receiver);
            }
            damageMod = 1;
        }

        public void GetDamageModifier(Monster receiver) {
            var prim = receiver.PrimaryType;
            var secon = receiver.SecondaryType;
            switch (Type) {
            case Type.Normal:
                damageMod = 1;
                break;
            case Type.Fight:
                    if (prim == Type.Normal || secon == Type.Normal) {
                        damageMod *= 2;
                    }
                break;
            case Type.Fire:
                if (prim == Type.Water|| secon == Type.Water) {
                    damageMod *= 2;
                }
                break;
            case Type.Water:
                if (prim == Type.Fire|| secon == Type.Fire) {
                    damageMod *= 2;
                }
                break;
            case Type.Grass:
                if (prim == Type.Water || secon == Type.Water) {
                    damageMod *= 2;
                }
                break;
            }
        }

        #region Preset Moves

        #region Physical
        public static Move Tackle() {
            return new Move("Tackle", "A fullbody tackle",
                60, 100, Kind.Physical, Type.Normal);
        }
        public static Move Headbutt() {
            return new Move("Headbutt", "A headbutt",
                70, 100, Kind.Physical, Type.Normal);
        }
        public static Move Strangle() {
            return new Move("Strangle", "The monster strangles the foe",
                40, 100, Kind.Physical, Type.Normal);
        }
        #endregion

        #region Special
        public static Move Bubble() {
            return new Move("Bubble", "The monster spits bubbles",
                40, 100, Kind.Special, Type.Water);
        }

        #endregion

        #region NonDamage
        public static Move Glare() {
            //TODO: Test if this will keep reducing stats if used more than once
            var statMod = new StatModifier(0, 0, 0, 0, .9);
            return new Move("Glare", "The monster gives a cold glare and slightly lowers opponents speed",
                0, 70, statMod, statMod, Kind.NonDamage, Type.Normal);
        }
        public static Move Intimidate() {
            var hit = new StatModifier(0, 0.9, 0, 0, 0);
            var miss = new StatModifier(0, 0, 0.9, 0, 0);
            return new Move("Intimidate", "The monster shows the opponent just how intimidating it can be," +
                                          "which will slightly lower the opponents attack," +
                                          "if it misses will lower the users defense",
                0, 75, hit, miss, Kind.NonDamage, Type.Normal);
        }
        #endregion
        #endregion
    }
}
