using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VideoGame.Classes {
    public enum Ailment {
        Normal,
        Sleep,
        Poisoned,
        Paralyzed,
        Burned,
        Dazzled,
        Frozen,
        Blessed,
        Frenzied,
        Fainted
    }
    
    public class Stats {
        //Randomized stats which will be multiplied with Base and Level
        public int RandHealth { get; }
        public int RandAttack { get; }
        public int RandDefense { get; }
        public int RandSpecialAttack { get; }
        public int RandSpecialDefense { get; }
        public int RandSpeed { get; }

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
        /// new Base stat and automatically calculate all stats
        /// </summary>
        public Stats(int health, int attack, int defense, int specialattack, int specialdefense, int speed, int level) {
            BaseHealth = health;
            BaseAttack = attack;
            BaseDefense = defense;
            BaseSpecialAttack = specialattack;
            BaseSpecialDefense = specialdefense;
            BaseSpeed = speed;

            //TODO: Change the random to crypto random
            Random rand = new Random();
            RandHealth = rand.Next(0, 31);
            RandAttack = rand.Next(0, 31);
            RandDefense = rand.Next(0, 31);
            RandSpecialAttack = rand.Next(0, 31);
            RandSpecialDefense = rand.Next(0, 31);
            RandSpeed = rand.Next(0, 31);

            CalculateStats(level);
        }
        private void CalculateStats(int level) {
            //Shamelessly stolen from Pokemon, without EVs
            Health = (((BaseHealth * RandHealth) * 2) * level / 100) + 5;
            Attack = (((BaseAttack * RandAttack) * 2) * level / 100) + 5;
            Defense = (((BaseDefense * RandDefense) * 2) * level / 100) + 5;
            SpecialAttack = (((BaseSpecialAttack * RandSpecialAttack) * 2) * level / 100) + 5;
            SpecialDefense = (((BaseSpecialDefense * RandSpecialDefense) * 2) * level / 100) + 5;
            Speed = (((BaseSpeed * RandSpeed * 2) * level) / 100) + 5;
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
            stats.Attack *= (int)AttackMod;
            stats.Defense *= (int)DefenseMod;
            stats.SpecialAttack *= (int)SpecialAttackMod;
            stats.SpecialDefense *= (int)SpecialDefenseMod;
            stats.Speed *= (int)SpeedMod;
            return stats;
        }
    }
}
