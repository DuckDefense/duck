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
        ToxicBody,
        Swift,
        Relaxed,
        StrongFist,
        AfterImage,
        Evasive,
        Enjoyer,
        Squishy,
        Confident,
        Warm,
        OnFire,
        Silly,
        Deaf,
        Musician
    }

    public class Ability {
        public AbilityId Id;
        public string Name;
        public string Description;

        #region Specifics

        //Status
        public double ChanceToEvade;
        public double ChanceToNeglectAttack;
        public double ChanceToSkipTurn;
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
        public bool ImmuneToSound;

        public double MoneyModifier;

        #endregion

        private Ability(AbilityId id, string name, string description) {
            Id = id;
            Name = name;
            Description = description;
        }

        public static Ability GetAbilityFromString(string s) {
            switch (s) {
            case "Buff": return Buff();
            case "Fuzzy": return Fuzzy();
            case "Enraged": return Enraged();
            case "Ordinary": return Ordinary();
            case "Unmovable": return Unmovable();
            case "ToxicBody": return ToxicBody();
            case "Swift": return Swift();
            case "Relaxed": return Relaxed();
            case "StrongFist": return StrongFist();
            case "AfterImage": return AfterImage();
            case "Evasive": return Evasive();
            case "Enjoyer": return Enjoyer();
            case "Squishy": return Squishy();
            case "Confident": return Confident();
            case "Warm": return Warm();
            case "OnFire": return OnFire();
            case "Silly": return Silly();
            case "Deaf": return Deaf();
            case "Musician": return Musician();
            }
            return null;
        }

        public void GetEffects(Monster user, Monster opponent) {
            Random rand = new Random();

            switch (user.Ability.Id) {
            case AbilityId.Buff: user.Stats = new StatModifier(1, 1.25, 1, 1, 1).ApplyModifiers(user); break;
            case AbilityId.Fuzzy: ChanceToNeglectAttack = 7.5; break;
            case AbilityId.Enraged: user.Stats = new StatModifier(1.25, 1, 1, 1, 1).ApplyModifiers(user); break;
            case AbilityId.Ordinary: break;
            case AbilityId.Unmovable: GoesLast = true; break;
            case AbilityId.ToxicBody: ChanceToInflictPoison = 10; break;
            case AbilityId.Swift: user.Stats = new StatModifier(1, 1, 1, 1, 1.25).ApplyModifiers(user); break;
            case AbilityId.Relaxed: user.MaxHealth += user.MaxHealth / 95; break;
            case AbilityId.StrongFist: user.Stats = new StatModifier(1.50, 0.75, 1, 1, 1).ApplyModifiers(user); break;
            case AbilityId.AfterImage: ChanceToEvade = 3; user.Stats = new StatModifier(1, 1, 1, 1, 1.20).ApplyModifiers(user); break;
            case AbilityId.Evasive: ChanceToEvade = 5; break;
            case AbilityId.Enjoyer: user.Stats = new StatModifier(1.1, 1.1, 1.1, 1.1, 1.1).ApplyModifiers(user); break;
            case AbilityId.Squishy: user.Stats = new StatModifier(1, .80, 1, 1, 1.30).ApplyModifiers(user); break;
            case AbilityId.Confident: foreach (var m in user.Moves) m.Accuracy += 25;  break;
            case AbilityId.Warm: ChanceToInflictSleep = 7.5f; break;
            case AbilityId.OnFire: ChanceToInflictBurned = 15f; break;
            case AbilityId.Silly: ChanceToSkipTurn = 10f; break;
            case AbilityId.Deaf: ImmuneToSound = true; break;
            case AbilityId.Musician: MoneyModifier = 20f; break;
            }
            //TODO: Actually check for the chances
        }
        #region Preset Abilities

        public static Ability Buff() {
            return new Ability(AbilityId.Buff, "Buff", "The muscle amount in this monster is very dense, \nincreasing this monsters defense slightly.");
        }
        public static Ability Fuzzy() {
            return new Ability(AbilityId.Fuzzy, "Fuzzy", "The fur on this monster is very springy, \nhas a small chance to neglect an attack.");
        }
        public static Ability Enraged() {
            return new Ability(AbilityId.Enraged, "Enraged", "This monster is overcome with rage which makes \nits attacks hit harder, but reduces accuracy.");
        }
        public static Ability Ordinary() {
            return new Ability(AbilityId.Ordinary, "Ordinary", "This monster is just like all the others, \nand is not unique in any way.");
        }
        public static Ability Unmovable() {
            return new Ability(AbilityId.Unmovable, "Unmovable", "Being unmovable is not helping this creature at all, \nand thus will always move last.");
        }
        public static Ability ToxicBody() {
            return new Ability(AbilityId.ToxicBody, "Toxic Body", "If this monster gets hit by a physical attack \nthere is a 10% chance to get poisoned.");
        }
        public static Ability Swift() {
            return new Ability(AbilityId.Swift, "Swift", "This monster is generally faster than others");
        }
        public static Ability Relaxed() {
            return new Ability(AbilityId.Relaxed, "Relaxed", "This creature likes to relax, \nwhich slightly increases its health");
        }
        public static Ability StrongFist() {
            return new Ability(AbilityId.StrongFist, "Strong Fist", "These guns ain't just for show");
        }
        public static Ability AfterImage() {
            return new Ability(AbilityId.AfterImage, "After Image", "Being fast has its advantages, \nwhich will sometimes leave afterimages");
        }
        public static Ability Evasive() {
            return new Ability(AbilityId.Evasive, "Evasive", "This monster has a small chance to evade attacks");
        }
        public static Ability Enjoyer() {
            return new Ability(AbilityId.Enjoyer, "Enjoyer", "Finding a way to enjoy its life \nthis monster has found himself a lot happier");
        }
        public static Ability Squishy() {
            return new Ability(AbilityId.Squishy, "Squishy", "The soft skin has not been kind on this monster, \nits defense is slightly lowered");
        }
        public static Ability Confident() {
            return new Ability(AbilityId.Confident, "Confident", "This confident creature has no trouble hitting its moves");
        }
        public static Ability Warm() {
            return new Ability(AbilityId.Warm, "Warm", "This monster is pleasantly warm, \ncausing the enemy to occasionally fall asleep");
        }
        public static Ability OnFire() {
            return new Ability(AbilityId.OnFire, "On Fire", "Considering a part of its body is on fire, \nattack it may cause you to catch on fire");
        }
        public static Ability Silly() {
            return new Ability(AbilityId.Silly, "Silly", "This creature is silly, \nwhich will sometimes forget to attack");
        }
        public static Ability Deaf() {
            return new Ability(AbilityId.Deaf, "Deaf", "Being deaf makes this monster immune to \nSound-Types attack");
        }
        public static Ability Musician() {
            return new Ability(AbilityId.Musician, "Musician", "Its experience in music shows its worth, \ngiving the user more money when it defeats an opponent");
        }
        #endregion
    }
}
