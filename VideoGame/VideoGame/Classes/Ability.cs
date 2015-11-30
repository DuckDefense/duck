using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VideoGame.Classes {
    public enum AbilityId {
        Buff,
        Fuzzy,
        Enraged,
        Ordinary,
        Unmovable,
        ToxicBody
    }

    public class Ability {
        public AbilityId Id;
        public string Name;
        public string Description;

        #region Specifics

        public double ChanceToNeglectAttack;
        //Status
        public double ChanceToInflictSleep;
        public double ChanceToInflictPoison;
        public double ChanceToInflictParalyzed;
        public double ChanceToInflictBurned;
        public double ChanceToInflictDazzled;
        public double ChanceToInflictFrozen;
        public double ChanceToInflictBlessed;
        public double ChanceToInflictFrenzied;
        public bool GoesLast;
        public bool GoesFirst;

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
            case AbilityId.Ordinary:
                break;
            case AbilityId.Unmovable:
                GoesLast = true;
                break;
            case AbilityId.ToxicBody:
                ChanceToInflictPoison = 10;
                break;
            }
        }
        #region Preset Abilities

        public static Ability Buff() {
            return new Ability(AbilityId.Buff, "Buff", "The muscle amount in this monster is very dense, increasing this monsters defense slightly.");
        }
        public static Ability Fuzzy() {
            return new Ability(AbilityId.Fuzzy, "Fuzzy", "The fur on this monster is very springy, has a small chance to neglect an attack.");
        }
        public static Ability Enraged() {
            return new Ability(AbilityId.Enraged, "Enraged", "this monster is overcome with rage which makes his attacks hit harder, but reduces accuracy.");
        }
        public static Ability Ordinary() {
            return new Ability(AbilityId.Ordinary, "Ordinary", "This monster is just like all the others, and is not unique in any way.");
        }
        public static Ability Unmovable() {
            return new Ability(AbilityId.Unmovable, "Unmovable", "Being unmovable is not helping this creature at all, and thus will always move last.");
        }
        public static Ability ToxicBody() {
            return new Ability(AbilityId.ToxicBody, "Toxic Body", "If this monster gets hit by a physical attack there is a 10% chance to get poisoned.");
        }

        #endregion
    }
}
