using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.AccessControl;
using System.Text;

namespace VideoGame.Classes {
    public class ModApplyer {
        public StatModifier Modifier;
        public bool ApplyToUser;
        public bool ApplyToOpponent;

        public ModApplyer(StatModifier mod, bool applyToUser, bool applyToOpponent) {
            Modifier = mod;
            ApplyToUser = applyToUser;
            ApplyToOpponent = applyToOpponent;
        }
    }

    public enum Kind {
        Physical,
        Special,
        NonDamage
    }

    public class Move {

        public string Name;
        public string Description;
        public int BaseDamage;
        public int Damage;
        public Kind Kind;
        public int Accuracy;
        public int Uses;
        public int MaxUses;
        public Type Type;
        public double AilmentChance;
        public Ailment Ailment;
        private ModApplyer HitModifier;
        private ModApplyer MissModifier;
        public bool Missed = false;

        public Move(string name, string description, int damage, int accuracy, int uses, Kind kind, Type type) {
            Name = name;
            Description = description;
            BaseDamage = damage;
            Accuracy = accuracy;
            Uses = uses;
            MaxUses = uses;
            Kind = kind;
            Type = type;
        }
        public Move(string name, string description, int damage, int accuracy, int uses, ModApplyer hitModifier, ModApplyer missModifier, Kind kind, Type type) {
            Name = name;
            Description = description;
            BaseDamage = damage;
            Accuracy = accuracy;
            Uses = uses;
            MaxUses = uses;
            HitModifier = hitModifier;
            MissModifier = missModifier;
            Kind = kind;
            Type = type;
        }

        public Move(string name, string description, int damage, int accuracy, int uses, Ailment ailment, double ailmentChance, Kind kind, Type type) {
            Name = name;
            Description = description;
            BaseDamage = damage;
            Accuracy = accuracy;
            Uses = uses;
            MaxUses = uses;
            Ailment = ailment;
            AilmentChance = ailmentChance;
            Kind = kind;
            Type = type;
        }

        /// <summary>
        /// Execute a move
        /// </summary>
        /// <param name="user">Monster that is using this move</param>
        /// <param name="receiver">Monster that is receiving this move</param>
        public void Execute(Monster user, Monster receiver) {
            //TODO: Actually remove uses here
            Uses -= 1;
            double critMultiplier = 1;
            //Check if the move hit
            var chanceToHit = Accuracy + ((user.Stats.Speed - receiver.Stats.Speed) / 3);
            if (chanceToHit < Accuracy / 3) chanceToHit = Accuracy / 3;
            Random rand = new Random();
            //If random number is below chanceToHit the move did hit
            if (rand.Next(0, 100) <= chanceToHit) {
                Missed = false;
                //if random number is below the CriticalHitChance, the move did a critical
                if (rand.Next(0, 100) <= user.CriticalHitChance) critMultiplier = user.CriticalHitMultiplier;

                if (Kind == Kind.Physical) {
                    Damage = GetDamage(user.Stats.Attack, receiver.Stats.Defense, GetDamageModifier(receiver),
                        critMultiplier);
                    receiver.Stats.Health -= Damage;
                    //Replace this with a lerp (reducing it little by little) so it looks nicer
                }
                else if (Kind == Kind.Special) {
                    Damage = GetDamage(user.Stats.SpecialAttack, receiver.Stats.SpecialDefense,
                        GetDamageModifier(receiver), critMultiplier);
                    receiver.Stats.Health -= Damage;
                }
                if (HitModifier != null) {
                    if (HitModifier.ApplyToOpponent) receiver.Stats = HitModifier.Modifier.ApplyModifiers(receiver);
                    if (HitModifier.ApplyToUser) user.Stats = HitModifier.Modifier.ApplyModifiers(user);
                }

            }
            //User missed
            else {
                Drawer.AddMessage(new List<string> { "But it missed.." });
                Missed = true;
                if (MissModifier != null) {
                    if (MissModifier.ApplyToOpponent)
                        receiver.Stats = MissModifier.Modifier.ApplyModifiers(receiver);
                    if (MissModifier.ApplyToUser) user.Stats = MissModifier.Modifier.ApplyModifiers(user);
                    //receiver.Stats = MissModifier.ApplyModifiers(receiver);
                }
            }
            // Check if either monster died
            if (user.Stats.Health <= 0) user.Ailment = Ailment.Fainted;
            if (receiver.Stats.Health <= 0) receiver.Ailment = Ailment.Fainted;
        }


        public int GetDamage(int offensive, int defensive, double modifier, double crit) {
            if (modifier == 0) return 0;
            var d = Convert.ToInt32(((BaseDamage * ((double)offensive / (double)defensive)) * modifier) * crit);
            if (d < 1)
                return 1;
            return d;
        }

        public double GetDamageModifier(Monster receiver) {
            double modifier = 1;
            var prim = receiver.PrimaryType;
            var secon = receiver.SecondaryType;
            switch (Type) {
            case Type.Normal:
                if (prim == Type.Ghost || secon == Type.Ghost) { modifier *= 0; }
                if (prim == Type.Sound || secon == Type.Sound) { modifier *= 2; }
                break;
            case Type.Fight:
                if (prim == Type.Normal || secon == Type.Normal) { modifier *= 2; }
                if (prim == Type.Rock || secon == Type.Rock) { modifier *= 2; }
                if (prim == Type.Ghost || secon == Type.Ghost) { modifier *= 0; }
                break;
            case Type.Fire:
                if (prim == Type.Grass || secon == Type.Grass) { modifier *= 2; }
                if (prim == Type.Water || secon == Type.Water) { modifier *= .5; }
                if (prim == Type.Rock || secon == Type.Rock) { modifier *= .5; }
                if (prim == Type.Ice || secon == Type.Ice) { modifier *= 2; }
                break;
            case Type.Water:
                if (prim == Type.Fire || secon == Type.Fire) { modifier *= 2; }
                if (prim == Type.Grass || secon == Type.Grass) { modifier *= .5; }
                if (prim == Type.Rock || secon == Type.Rock) { modifier *= 2; }
                break;
            case Type.Grass:
                if (prim == Type.Water || secon == Type.Water) { modifier *= 2; }
                if (prim == Type.Fire || secon == Type.Fire) { modifier *= .5; }
                if (prim == Type.Rock || secon == Type.Rock) { modifier *= 2; }
                if (prim == Type.Poison || secon == Type.Poison) { modifier *= .5; }
                break;
            case Type.Rock:
                if (prim == Type.Fire || secon == Type.Fire) { modifier *= 2; }
                if (prim == Type.Ice || secon == Type.Ice) { modifier *= 2; }
                break;
            case Type.Poison:
                if (prim == Type.Psych || secon == Type.Psych) { modifier *= 2; }
                if (prim == Type.Grass || secon == Type.Grass) { modifier *= 2; }
                break;
            case Type.Psych:
                if (prim == Type.Ghost || secon == Type.Ghost) { modifier *= 2; }
                break;
            case Type.Flying:
                if (prim == Type.Grass || secon == Type.Grass) { modifier *= 2; }
                if (prim == Type.Ice || secon == Type.Ice) { modifier *= 2; }
                if (prim == Type.Fight || secon == Type.Fight) { modifier *= 2; }
                break;
            case Type.Ice:
                if (prim == Type.Water || secon == Type.Water) { modifier *= .5; }
                if (prim == Type.Fire || secon == Type.Fire) { modifier *= 2; }
                if (prim == Type.Grass || secon == Type.Grass) { modifier *= 2; }
                if (prim == Type.Rock || secon == Type.Rock) { modifier *= 2; }
                if (prim == Type.Poison || secon == Type.Poison) { modifier *= 2; }
                if (prim == Type.Flying || secon == Type.Flying) { modifier *= 2; }
                if (prim == Type.Sound || secon == Type.Sound) { modifier *= .5; }
                break;
            case Type.Ghost:
                if (prim == Type.Normal || secon == Type.Normal) { modifier *= 0; }
                if (prim == Type.Psych || secon == Type.Psych) { modifier *= 2; }
                if (prim == Type.Ghost || secon == Type.Ghost) { modifier *= 2; }
                break;
            case Type.Sound:
                if (prim == Type.Normal || secon == Type.Normal) { modifier *= .5; }
                if (prim == Type.Water || secon == Type.Water) { modifier *= .5; }
                if (prim == Type.Psych || secon == Type.Psych) { modifier *= 2; }
                if (prim == Type.Rock || secon == Type.Rock) { modifier *= .5; }
                if (prim == Type.Ice || secon == Type.Ice) { modifier *= 2; }
                if (prim == Type.Ghost || secon == Type.Ghost) { modifier *= 2; }
                break;
            default:
                throw new ArgumentOutOfRangeException();
            }
            return modifier;
        }

        #region Preset Moves

        #region Normal
        //Physical
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
        //Special

        //NonDamage
        public static Move Glare() {
            var statMod = new ModApplyer(new StatModifier(1, 1, 1, 1, .75), false, true);
            return new Move("Glare", "The monster gives a cold glare",
                0, 70, 25, statMod, null, Kind.NonDamage, Type.Normal);
        }
        public static Move Intimidate() {
            var hit = new ModApplyer(new StatModifier(1, 0.75, 1, 1, 1), false, true);
            var miss = new ModApplyer(new StatModifier(1, 1, 0.75, 1, 1), true, false);
            return new Move("Intimidate", "The monster shows the opponent just how intimidating it is",
                0, 75, 20, hit, miss, Kind.NonDamage, Type.Normal);
        }
        #endregion
        #region Fight
        //Physical
        public static Move MultiPunch() {
            return new Move("Multipunch", "punches the foe at high speed",
                70, 90, 15, Kind.Physical, Type.Fight);
        }
        #endregion
        #region Fire
        //Physical

        //Special
        public static Move Meteor() {
            return new Move("Meteor", "Cast down a meteor upon the foe",
                120, 55, 5, Kind.Special, Type.Fire);
        }
        public static Move Implode() {
            return new Move("Implode", "Implodes the foe",
                70, 85, 20, Ailment.Burned, 20, Kind.Special, Type.Fire);
        }

        //NonDamage
        public static Move Flare() {
            return new Move("Flare", "Fires a flare to the foe",
                0, 85, 25, Ailment.Burned, 75, Kind.NonDamage, Type.Fire);
        }
        #endregion
        #region Water
        //Physical
        public static Move WetSlap() {
            return new Move("Wet Slap", "A wet slap right in the kisser",
                50, 85, 30, Kind.Physical, Type.Water);
        }

        //Special
        public static Move Bubble() {
            return new Move("Bubble", "The monster spits bubbles",
                40, 100, 30, Kind.Special, Type.Water);
        }
        //NonDamage
        public static Move Douse() {
            return new Move("Douse", "Slow down the foe by dousing it",
                0, 100, 20, 
                new ModApplyer(new StatModifier(1, 1, 1, 1, 0.8), false, true), 
                new ModApplyer(new StatModifier(1, 1, 1, 1, 1), true, false),
                Kind.NonDamage, Type.Water);
        }

        #endregion
        #region Grass
        //Physical

        //Special
        public static Move LeafCut()
        {
            return new Move("LeafCut", "Cuts the target with a sharp leaf",
                60, 80, 20, Kind.Physical, Type.Grass);
        }

        public static Move NatureCalling()
        {
            return new Move("Nature Calling", "Calls upon nature to harm the foe",
                60, 100, 25, Kind.Special, Type.Grass);
        }

        public static Move TreeHammer()
        {
            return new Move("Tree hammer", "Tears a tree out of the earth, and smacks the foe with it",
                120, 95, 5, Kind.Physical, Type.Grass);
        }

        //NonDamage
            #endregion
            #region Rock
        public static Move RockThrow() {
            return new Move("Rock throw", "Throw a rock at the foe",
                65, 80, 20, Kind.Special, Type.Rock);
        }
        #endregion
        #region Ice
        public static Move Freeze() {
            return new Move("Freeze", "Freezes the foe",
                60, 80, 15, Ailment.Frozen, 20, Kind.Special, Type.Ice);
        }
        public static Move Icicle() {
            return new Move("Icicle", "Fires icicles at the foe",
                75, 70, 10, Kind.Special, Type.Ice);
        }
        #endregion
        #region Poison
        public static Move PoisonDart() {
            return new Move("Poison dart", "Shoots a poison dart at the target",
                40, 70, 60, Ailment.Poisoned, 60, Kind.Physical, Type.Normal);
        }
        #endregion
        #region Ghost
        public static Move Torment() {
            return new Move("Torment", "Torments the foe",
                70, 75, 15, Kind.Special, Type.Ghost);
        }
        public static Move SoulHunt() {
            return new Move("Soul hunt", "Hunt for the foes soul",
                120, 40, 10, Kind.Special, Type.Ghost);
        }
        #endregion
        #region Psych
        public static Move MindTrick() {
            return new Move("MindTrick", "Tricks the foes mind causing him to hurt itself",
                80, 60, 15, Kind.Special, Type.Psych);
        }
        public static Move MindClose() {
            return new Move("Mind close", "Closes the foes mind",
                20, 75, 5, Ailment.Sleep, 80, Kind.Special, Type.Fire);
        }
        #endregion
        #region Flying
        public static Move Tornado() {
            return new Move("Tornado", "Twists the foe in a tornado",
                50, 90, 15, Kind.Special, Type.Flying);
        }
        #endregion
        #region Sound
        public static Move Scream() {
            return new Move("Scream", "Screams loudly at the foe",
                60, 80, 20, Kind.Special, Type.Sound);
        }
        public static Move HighPitch() {
            return new Move("High pitch", "Make a high pitch sound, deafening the foe",
                75, 80, 15, Kind.Special, Type.Sound);
        }

        //NonDamage
        public static Move DazzlingTune() {
            return new Move("Dazzling Tune", "The monster plays a dazzling tune",
                0, 100, 15, Ailment.Dazzled, 75, Kind.NonDamage, Type.Sound);
        }public static Move SleepyTune() {
            return new Move("Sleepy Tune", "The monster plays a sleep inducing tune",
                0, 100, 15, Ailment.Sleep, 75, Kind.NonDamage, Type.Sound);
        }public static Move AngryTune() {
            return new Move("Angry Tune", "The monster plays a aggresive tune",
                0, 100, 15, Ailment.Frenzied, 75, Kind.NonDamage, Type.Sound);
        }

        #endregion
        #endregion
    }
}
