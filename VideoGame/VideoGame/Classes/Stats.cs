﻿using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VideoGame.Classes {
    public enum Ailment {
        Normal = 0,
        Sleep,
        Poisoned,
        Burned,
        Dazzled,
        Frozen,
        Frenzied,
        Fainted
    }

    public class Stats {
        //Randomized stats which will be multiplied with Base and Level
        public int RandAttack { get; set; }
        public int RandDefense { get; set; }
        public int RandSpecialAttack { get; set; }
        public int RandSpecialDefense { get; set; }
        public int RandSpeed { get; set; }

        //Base stats, which will be multiplied to level
        public int BaseHealth { get; }
        public int BaseAttack { get; }
        public int BaseDefense { get; }
        public int BaseSpecialAttack { get; }
        public int BaseSpecialDefense { get; }
        public int BaseSpeed { get; }

        //Shown stats
        public int Health;
        public int Attack;
        public int Defense;
        public int SpecialAttack;
        public int SpecialDefense;
        public int Speed;

        /// <summary>
        /// New stats with everything on 0
        /// </summary>
        public Stats() { }

        /// <summary>
        /// Stats with randomize disabled
        /// </summary>
        /// <param name="stats">Current stats</param>
        /// <param name="level">Level</param>
        /// <param name="randAttack"></param>
        /// <param name="randDefense"></param>
        /// <param name="randSpecialAttack"></param>
        /// <param name="randSpecialDefense"></param>
        /// <param name="randSpeed"></param>
        public Stats(Stats stats, int level, int randAttack, int randDefense, int randSpecialAttack, int randSpecialDefense, int randSpeed) {
            BaseHealth = stats.BaseHealth;
            BaseAttack = stats.BaseAttack;
            BaseDefense = stats.BaseDefense;
            BaseSpecialAttack = stats.BaseSpecialAttack;
            BaseSpecialDefense = stats.BaseSpecialDefense;
            BaseSpeed = stats.BaseSpeed;

            RandAttack = randAttack;
            RandDefense = randDefense;
            RandSpecialAttack = randSpecialAttack;
            RandSpecialDefense = randSpecialDefense;
            RandSpeed = randSpeed;

            CalculateStats(level);
        }

        /// <summary>
        /// new Base stat and automatically calculate all stats
        /// </summary>
        public Stats(int health, int attack, int defense, int specialattack, int specialdefense, int speed, int level, bool randomize = true) {
            BaseHealth = health;
            BaseAttack = attack;
            BaseDefense = defense;
            BaseSpecialAttack = specialattack;
            BaseSpecialDefense = specialdefense;
            BaseSpeed = speed;

            if (randomize) {
                var rand = new CryptoRandom();
                RandAttack = rand.Next(1, 31);
                RandDefense = rand.Next(1, 31);
                RandSpecialAttack = rand.Next(1, 31);
                RandSpecialDefense = rand.Next(1, 31);
                RandSpeed = rand.Next(1, 31);
            }
            CalculateStats(level);
        }
        public void CalculateStats(int level) {
            Health = ((((BaseHealth * 20) * 2) * level) / 250) + 5;
            Attack = ((((BaseAttack * RandAttack) * 2) * level) / 250) + 5;
            Defense = ((((BaseDefense * RandDefense) * 2) * level) / 250) + 5;
            SpecialAttack = ((((BaseSpecialAttack * RandSpecialAttack) * 2) * level) / 250) + 5;
            SpecialDefense = ((((BaseSpecialDefense * RandSpecialDefense) * 2) * level) / 250) + 5;
            Speed = ((((BaseSpeed * RandSpeed) * 2) * level) / 250) + 5;
        }

        public void LevelUp(int level, ref int maxhealth)
        {
            CalculateStats(level);
            maxhealth = Health;
        }

        public string PrintStats(int maxHealth) {
            return $"Health: {Health}/{maxHealth}\n" +
                   $"Attack: {Attack}\n" +
                   $"Defense: {Defense}\n" +
                   $"SpecialAttack: {SpecialAttack}\n" +
                   $"SpecialDefense: {SpecialDefense}\n" +
                   $"Speed: {Speed}";
        }

        public string PintBaseStats() {
            return $"Base Health: {BaseHealth}\n" +
                   $"Base Attack: {BaseAttack}\n" +
                   $"Base Defense: {BaseDefense}\n" +
                   $"Base SpecialAttack: {BaseSpecialAttack}\n" +
                   $"Base SpecialDefense: {BaseSpecialDefense}\n" +
                   $"Base Speed: {BaseSpeed}";
        }
    }

    public class StatModifier {
        public double AttackMod = 1;
        public double DefenseMod = 1;
        public double SpecialAttackMod = 1;
        public double SpecialDefenseMod = 1;
        public double SpeedMod = 1;

        public StatModifier() { }

        /// <summary>
        /// Add a modifier for stats
        /// </summary>
        /// <param name="attackMod">Amount of which the Attack stat will be multiplied with (eg. .90 will return 90% of the original)</param>
        /// <param name="defenseMod">Amount of which the Defense stat will be multiplied with</param>
        /// <param name="specialAttackMod">Amount of which the SpecialAttack stat will be multiplied with</param>
        /// <param name="specialDefenseMod">Amount of which the Defense stat will be multiplied with</param>
        /// <param name="speedMod">Amount of which the Speed stat will be multiplied with</param>
        public StatModifier(double attackMod, double defenseMod, double specialAttackMod, double specialDefenseMod, double speedMod) {
            AttackMod = attackMod;
            DefenseMod = defenseMod;
            SpecialAttackMod = specialAttackMod;
            SpecialDefenseMod = specialDefenseMod;
            SpeedMod = speedMod;
        }

        public Stats ApplyModifiers(Monster monster) {
            var stats = monster.Stats;
            monster.RestorePreviousStats();
            CheckMinimum(monster.Stats, monster.PreviousStats);
            stats.Attack = Convert.ToInt32(stats.Attack * AttackMod);
            stats.Defense = Convert.ToInt32(stats.Defense * DefenseMod);
            stats.SpecialAttack = Convert.ToInt32(stats.SpecialAttack * SpecialAttackMod);
            stats.SpecialDefense = Convert.ToInt32(stats.SpecialDefense * SpecialDefenseMod);
            stats.Speed = Convert.ToInt32(stats.Speed * SpeedMod);
            //Actually set the right stats to PreviousStats
            return stats;
        }

        private void CheckMinimum(Stats curStats, Stats prevStats) {
            double attDif = prevStats.Attack - curStats.Attack;
            double defDif = prevStats.Defense - curStats.Defense;
            double specAttDif = prevStats.SpecialAttack - curStats.SpecialAttack;
            double specDefDif = prevStats.SpecialDefense - curStats.SpecialDefense;
            double spdDif = prevStats.Speed - curStats.Speed;
            //Check if stats are below 40% of the original
            if (attDif != 0) if ((attDif / prevStats.Attack) >= 0.75) { curStats.Attack = Convert.ToInt32(prevStats.Attack * 0.25); AttackMod = 1; }
            if (defDif != 0) if ((defDif / prevStats.Defense) >= 0.75) { curStats.Defense = Convert.ToInt32(prevStats.Defense * 0.25); DefenseMod = 1; }
            if (specAttDif != 0) if ((specAttDif / prevStats.SpecialAttack) >= 0.75) { curStats.SpecialAttack = Convert.ToInt32(prevStats.SpecialAttack * 0.25); SpecialAttackMod = 1; }
            if (specAttDif != 0) if ((specDefDif / prevStats.SpecialDefense) >= 0.75) { curStats.SpecialDefense = Convert.ToInt32(prevStats.SpecialDefense * 0.25); SpecialDefenseMod = 1; }
            if (spdDif != 0) if ((spdDif / prevStats.Speed) >= 0.75) { curStats.Speed = Convert.ToInt32(prevStats.Speed * 0.25); SpeedMod = 1; }
        }
    }
}
