using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VideoGame.Classes {
    public enum AbilityId {
        Buff,
        Fuzzy,
        Enraged,
    }

    public class Ability {
        public AbilityId Id;
        public string Name;
        public string Description;

        #region Specifics

        public double ChanceToNeglectAttack;

        #endregion

        private Ability(AbilityId id, string name, string description) {
            Id = id;
            Name = name;
            Description = description;
        }

        public void GetEffects(Monster user, Monster opponent) {
            Random rand = new Random();

            switch (user.Ability.Id) {
            case AbilityId.Buff:
                user.Stats = new StatModifier(1, 1.25, 1, 1, 1).ApplyModifiers(user);
                break;
            case AbilityId.Fuzzy:
                ChanceToNeglectAttack = 7.5;
                break;
            case AbilityId.Enraged:
                user.Stats = new StatModifier(1.25, 1, 1, 1, 1).ApplyModifiers(user);
                break;
            }
        }
        #region Preset Abilities

        public static Ability Buff() {
            return new Ability(AbilityId.Buff, "Buff", "The muscle amount in this monster is very dense, increasing this monsters defense slightly");
        }
        public static Ability Fuzzy() {
            return new Ability(AbilityId.Fuzzy, "Fuzzy", "The fur on this monster is very springy, has a small chance to neglect an attack");
        }
        public static Ability Enraged() {
            return new Ability(AbilityId.Enraged, "Enraged", "this monster is overcome with rage which makes his attacks hit harder, but reduces accuracy");
        }

        #endregion
    }
}
