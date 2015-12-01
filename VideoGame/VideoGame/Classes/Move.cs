﻿using System;
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
        public int Uses;
        public Type Type;
        public StatModifier HitModifier;
        public StatModifier MissModifier;

        public Move(string name, string description, int damage, int accuracy, int uses, Kind kind, Type type) {
            Name = name;
            Description = description;
            Damage = damage;
            Accuracy = accuracy;
            Uses = uses;
            Kind = kind;
            Type = type;
        }
        public Move(string name, string description, int damage, int accuracy, int uses, StatModifier hitModifier, StatModifier missModifier, Kind kind, Type type) {
            Name = name;
            Description = description;
            Damage = damage;
            Accuracy = accuracy;
            Uses = uses;
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
            double critMultiplier = 1;
            //Check if types are very effective or not very effective
            GetDamageModifier(receiver);
            //Check if the move hit
            var chanceToHit = Accuracy + ((user.Stats.Speed - receiver.Stats.Speed )/ 3);
            Random rand = new Random();
            //If random number is below chanceToHit the move did hit
            if (rand.Next(0, 100) <= chanceToHit)
            {

                //if random number is below the CriticalHitChance, the move did a critical
                if (rand.Next(0, 100) <= user.CriticalHitChance) critMultiplier = user.CriticalHitMultiplier;

                if (Kind == Kind.Physical)
                {
                    var phys = GetDamage(user.Stats.Attack, receiver.Stats.Defense, GetDamageModifier(receiver),
                        critMultiplier);
                    receiver.Stats.Health -= phys;
                        //Replace this with a lerp (reducing it little by little) so it looks nicer
                }
                else if (Kind == Kind.Special)
                {
                    var spec = GetDamage(user.Stats.SpecialAttack, receiver.Stats.SpecialDefense,
                        GetDamageModifier(receiver), critMultiplier);
                    receiver.Stats.Health -= spec;
                }
                if (HitModifier != null)
                {
                   //TODO: Find out if this will return the same stats if the modifier is empty
                    user.Stats = HitModifier.ApplyModifiers(user);
                    receiver.Stats = HitModifier.ApplyModifiers(receiver); 
                }
                
            }
            //User missed
            else
            {
                if (MissModifier != null)
                {
                    user.Stats = MissModifier.ApplyModifiers(user);
                    receiver.Stats = MissModifier.ApplyModifiers(receiver);
                }
            }
            Uses -= 1;
            // Check if either pokemon died
            if (user.Stats.Health >= 0) user.Ailment = Ailment.Fainted;
            if (receiver.Stats.Health >= 0) receiver.Ailment = Ailment.Fainted;
        }


        private int GetDamage(int offensive, int defensive, int modifier, double crit) {
            return Convert.ToInt32(((Damage * (offensive / defensive)) * modifier) * crit);
        }

        private int GetDamageModifier(Monster receiver) {
            double modifier = 1;
            var prim = receiver.PrimaryType;
            var secon = receiver.SecondaryType;
            switch (Type) {
            case Type.Normal:
                break;
            case Type.Fight:
                if (prim == Type.Normal || secon == Type.Normal) { modifier *= 2; }
                break;
            case Type.Fire:
                if (prim == Type.Grass || secon == Type.Grass) { modifier *= 2; }
                if (prim == Type.Water || secon == Type.Water) { modifier *= .5; }
                break;
            case Type.Water:
                if (prim == Type.Fire || secon == Type.Fire) { modifier *= 2; }
                if (prim == Type.Grass || secon == Type.Grass) { modifier *= .5; }
                break;
            case Type.Grass:
                if (prim == Type.Water || secon == Type.Water) { modifier *= 2; }
                if (prim == Type.Fire || secon == Type.Fire) { modifier *= .5; }
                break;
            }
            return (int)modifier;
        }

        #region Preset Moves

        #region Physical
        public static Move Tackle() {
            return new Move("Tackle", "A fullbody tackle",
                60, 100, 40, Kind.Physical, Type.Normal);
        }
        public static Move Headbutt() {
            return new Move("Headbutt", "A headbutt",
                70, 100, 25, Kind.Physical, Type.Normal);
        }
        public static Move Strangle() {
            return new Move("Strangle", "The monster strangles the foe",
                40, 100, 20, Kind.Physical, Type.Normal);
        }
        #endregion

        #region Special
        public static Move Bubble() {
            return new Move("Bubble", "The monster spits bubbles",
                40, 100, 30, Kind.Special, Type.Water);
        }

        #endregion

        #region NonDamage
        public static Move Glare() {
            //TODO: Test if this will keep reducing stats if used more than once
            var statMod = new StatModifier(0, 0, 0, 0, .75);
            return new Move("Glare", "The monster gives a cold glare and slightly lowers opponents speed",
                0, 70, 25, statMod, statMod, Kind.NonDamage, Type.Normal);
        }
        public static Move Intimidate() {
            var hit = new StatModifier(0, 0.75, 0, 0, 0);
            var miss = new StatModifier(0, 0, 0.75, 0, 0);
            return new Move("Intimidate", "The monster shows the opponent just how intimidating it can be," +
                                          "which will slightly lower the opponents attack," +
                                          "if it misses will lower the users defense",
                0, 75, 20, hit, miss, Kind.NonDamage, Type.Normal);
        }
        #endregion
        #endregion
    }
}
