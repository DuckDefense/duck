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
        Frozen
    }
    public class Stats {
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

        public Stats(int health, int attack, int defense, int specialattack, int specialdefense, int speed) {
            Health = health;
            Attack = attack;
            Defense = defense;
            SpecialAttack = specialattack;
            SpecialDefense = specialdefense;
            Speed = speed;
        }
    }

    public class StatModifier {
        public double AttackMod;
        public double DefenseMod;
        public double SpecialAttackMod;
        public double SpecialDefenseMod;
        public double SpeedMod;

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

        public Stats ApplyModifiers(Monster receiver) {
            var stats = receiver.Stats;
            stats.Attack *= (int) AttackMod;
            stats.Defense *= (int) DefenseMod;
            stats.SpecialAttack *= (int) SpecialAttackMod;
            stats.SpecialDefense *= (int) SpecialDefenseMod;
            stats.Speed *= (int) SpeedMod;
            return stats;
        }
    }
}
