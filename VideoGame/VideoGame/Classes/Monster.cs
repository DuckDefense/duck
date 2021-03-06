﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using OpenTK.Graphics.ES20;
using Sandbox.Classes;

namespace VideoGame.Classes {

    public enum Gender {
        NoGender,
        Male,
        Female
    }

    public class Monster : IAnimatable {
        public Rectangle SourceRectangle;
        public Point PartySpriteSize = new Point(24, 24);
        private Point CurrentFrame = new Point(0, 0);
        private float Interval = 500f;
        private float Timer = 0f;
        public int MaxHealth;

        public bool IsDead => Stats.Health <= 0; //Returns true if health is 0 or below it
        public bool DeadCount = false;
        public bool IsWild;
        public bool Fought;

        public Texture2D FrontSprite; //Sprite that is shown when you're fighting this monster
        public Texture2D BackSprite; //Sprite that is shown when you've send out this monster
        public Texture2D PartySprite; //Sprite that is shown if you open the party
                                      //If we are going add monsters in the world we need to add a position and a collision box
        public int RemainingExp;
        private int experience;
        public int Experience
        {
            get { return experience; }
            set { experience = value; }
        }

        //TODO: Fix this
        public int Level { get; set; }

        public int Id;
        public int UId;
        public int CaptureChance;
        public string Name;
        public string Description;
        public Stats PreviousStats;
        public int StatId;
        public Stats Stats;
        public Type PrimaryType;
        public Type SecondaryType;
        public Item HeldItem; //Item the monster is currently holding
        public Ailment Ailment;
        public Gender Gender;
        public List<Ability> PossibleAbilities = new List<Ability>(2);
        public Ability Ability;
        public double CriticalHitChance = 5;
        public double CriticalHitMultiplier = 2;
        public List<Move> Moves;

        public Monster() { }

        /// <summary>
        /// Monster with one type
        /// </summary>
        /// <param name="id">ID of the monster</param>
        /// <param name="level">Level</param>
        /// <param name="name">Name</param>
        /// <param name="description">Short description of the monster</param>
        /// <param name="maleChance">Chance for the monster to be male</param>
        /// <param name="captureChance">Chance to capture this monster</param>
        /// <param name="helditem">Item the monster is carrying</param>
        /// <param name="stats">Stats the monster has</param>
        /// <param name="type">Type which changes how much damage certain moves do</param>
        /// <param name="abilities">All possible abilities the monster can have</param>
        /// <param name="front">Texture that is shown when fighting against this monster</param>
        /// <param name="back">Texture that is shown when you've send out this monster</param>
        /// <param name="party">Texture that is shown in the party view</param>
        public Monster(int id, int level, string name, string description, Type type, int maleChance, int captureChance, Item helditem, Stats stats, List<Ability> abilities,
            Texture2D front, Texture2D back, Texture2D party, bool database = false) {
            Id = id;
            if (!database) {
                StatId = RandomId.GenerateStatsId();
                UId = RandomId.GenerateRandomUId();
            }
            Level = level;
            experience = level * level * 5;
            RemainingExp = ((level + 1) * (level + 1) * 5) - experience;
            Name = name;
            Description = description;
            PrimaryType = type;
            SecondaryType = Type.None;
            Gender = GetGender(maleChance);
            CaptureChance = captureChance;
            HeldItem = helditem;
            Stats = stats;
            PossibleAbilities = abilities;
            Ability = GetAbility(abilities);
            FrontSprite = front;
            BackSprite = back;
            PartySprite = party;
            Ailment = Ailment.Normal;
            GetMoves();
            MaxHealth = Stats.Health;
        }

        /// <summary>
        /// Monster with two types
        /// </summary>
        /// <param name="id">Id of the monster</param>
        /// <param name="level">Level</param>
        /// <param name="name">Name</param>
        /// <param name="maleChance">Chance for the monster to be male</param>
        /// <param name="captureChance">Chance to capture this monster</param>
        /// <param name="helditem">Item the monster is carrying</param>
        /// <param name="stats">Stats the monster has</param>
        /// <param name="description">Short description of the monster</param>
        /// <param name="primaryType">Primary type which changes how much damage certain moves do</param>
        /// <param name="secondaryType">Secondary type which changes how much damage certain moves do</param>
        /// <param name="abilities">All possible abilities the monster can have</param>
        /// <param name="front">Texture that is shown when fighting against this monster</param>
        /// <param name="back">Texture that is shown when you've send out this monster</param>
        /// <param name="party">Texture that is shown in the party view</param>
        public Monster(int id, int level, string name, string description, Type primaryType, Type secondaryType, int maleChance, int captureChance, Item helditem, Stats stats, List<Ability> abilities,
        Texture2D front, Texture2D back, Texture2D party, bool database = false) {
            Id = id;
            if (!database) {
                StatId = RandomId.GenerateStatsId();
                UId = RandomId.GenerateRandomUId();
            }
            Level = level;
            experience = level * level * 5;
            RemainingExp = ((level + 1) * (level + 1) * 5) - experience;
            Name = name;
            Description = description;
            PrimaryType = primaryType;
            SecondaryType = secondaryType;
            Gender = GetGender(maleChance);
            CaptureChance = captureChance;
            HeldItem = helditem;
            Stats = stats;
            PossibleAbilities = abilities;
            Ability = GetAbility(abilities);
            FrontSprite = front;
            BackSprite = back;
            PartySprite = party;
            Ailment = Ailment.Normal;
            Moves = new List<Move>();
            GetMoves();
            //TODO: Check if this updates with levelup
            MaxHealth = Stats.Health;
        }

        public void ReceiveExp(Monster opponent) {
            double a = opponent.IsWild ? 1 : 1.5f;
            double l = opponent.Level;
            double y = Level >= l ? 1 : 1.2f;

            var expGain = (a * l * y);
            // / 3

            experience += Convert.ToInt32(expGain);
            CheckLevelUp();
            //TODO: Add a experience calculation which will return level based on the amount of experience it has
        }
        public void RestorePreviousStats() {
            var stats = Stats;
            PreviousStats = new Stats(stats, Level, stats.RandAttack, stats.RandDefense, stats.RandSpecialAttack, stats.RandSpecialDefense, stats.RandSpeed);
        }

        public void CheckLevelUp() {
            //var level = Experience / Level / 5;
            int level = Level;
            for (int i = level; i < 100; i++) {
                var lvl = i * i * 5;
                RemainingExp = lvl - Experience;
                if (lvl < Experience) {
                    level++;
                }
                else break;
            }
            if (level - 1 > Level) {
                Level = level - 1;
                GetMoves();
                Stats.LevelUp(Level, ref MaxHealth);
            }
        }

        private static Gender GetGender(int maleChance) {
            Random rand = new Random();
            //If rand is smaller than MaleChance return Male, if it's bigger return female
            return rand.Next(0, maleChance) <= maleChance ? Gender.Male : Gender.Female;
        }

        public Ability GetAbility(List<Ability> abilities) {
            Random rand = new Random();
            return abilities[rand.Next(0, abilities.Count)];
        }

        public bool CanEvolve() {
            switch (Id) {
            case 1: if (Level >= 18) return true; break;
            case 2: if (Level >= 32) return true; break;

            case 4: if (Level >= 16) return true; break;
            case 5: if (Level >= 36) return true; break;

            case 7: if (Level >= 14) return true; break;
            case 8: if (Level >= 38) return true; break;

            case 10: if (Level >= 28) return true; break;

            case 12: if (Level >= 18) return true; break;
                case 19:
                    if (Level >= 25) return true;
                    break;
            }
            return false;
        }

        public Monster GetEvolution() {
            switch (Id) {
            case 1: return ReturnEvolution(2);
            case 2: return ReturnEvolution(3);

            case 4: return ReturnEvolution(5);
            case 5: return ReturnEvolution(6);

            //TODO: Update with fire starters
            case 7: return ReturnEvolution(8);
            case 8: return ReturnEvolution(9);

            case 10: return ReturnEvolution(11);

            case 12: return ReturnEvolution(13);
                case 19: return ReturnEvolution(20);
            }
            return this;
        }

        public void GetMoves() {
            Moves = new List<Move>();
            switch (Id) {
            case 1:
                if (Level >= 1) {
                    Moves.Add(Move.Tackle());
                }
                if (Level >= 8) Moves.Add(Move.LeafCut());
                if (Level >= 12) Moves.Add(Move.RockThrow());
                    break;
            case 2:
                Moves.AddMany(Move.Tackle(), Move.LeafCut(), Move.RockThrow());
                    if (Level >= 27) Moves.Add(Move.Strangle());
                    break;
            case 3:
                Moves.AddMany(Move.Tackle(), Move.LeafCut(), Move.RockThrow(), Move.Strangle());
                    if (Level >= 40) Moves.Add(Move.Intimidate());
                    if (Level >= 51) Moves.Add(Move.MultiPunch());
                    if (Level >= 60) Moves.Add(Move.PoisonDart());
                    if (Level >= 65) Moves.Add(Move.TreeHammer());
                    break;
            case 4:
                if (Level >= 1) Moves.Add(Move.Tackle());
                    if (Level >= 6) Moves.Add(Move.WetSlap());
                    if (Level >= 9) Moves.Add(Move.Bubble());
                if (Level >= 11) Moves.Add(Move.Peck());
                    break;
            case 5:
                Moves.AddMany(Move.Tackle(), Move.WetSlap(),Move.Bubble(),Move.Peck());
                    if (Level >= 20) Moves.Add(Move.Scream());
                    if (Level >= 30) Moves.Add(Move.Douse());
                    break;
            case 6:
                Moves.AddMany(Move.Tackle(),Move.WetSlap(), Move.Bubble(), Move.Peck(), Move.Scream(), Move.Douse());
                    if (Level >= 47) Moves.Add(Move.Icicle());
                    if (Level >= 53) Moves.Add(Move.Tornado());
                    break;
                case 7:
                    if (Level >= 1) Moves.Add(Move.Tackle());
                    if (Level >= 11) Moves.Add(Move.Flare());
                    break;
                case 8:
                    Moves.AddMany(Move.Tackle(), Move.Flare());
                    if (Level >= 16) Moves.Add(Move.FireClaw());
                    if (Level >= 20) Moves.Add(Move.RockThrow());
                    break;
                case 9:
                    Moves.AddMany(Move.Tackle(), Move.Flare(), Move.FireClaw(), Move.RockThrow());
                    if (Level >= 43) Moves.Add(Move.Tornado());
                    if (Level >= 50) Moves.Add(Move.TailSmash());
                    if (Level >= 63) Moves.Add(Move.Meteor());
                    break;
            case 10:
                if (Level >= 1) {
                    Moves.Add(Move.Strangle());
                    Moves.Add(Move.Glare());
                }
                if (Level >= 5) { Moves.Add(Move.Tackle()); }
                if (Level >= 9) { Moves.Add(Move.Intimidate()); }
                if (Level >= 11) { Moves.Add(Move.Headbutt()); }
                if (Level >= 18) Moves.Add(Move.MultiPunch());
                break;
            case 11:
                Moves.AddMany(Move.Strangle(), Move.Glare(), Move.Tackle(), Move.Intimidate(), Move.Headbutt(), Move.MultiPunch());
                break;
                case 12:
                    if (Level >= 1) { Moves.Add(Move.Tackle()); }
                    break;
                case 13:
                    if (Level >= 1) { Moves.Add(Move.Tackle()); }
                    if (Level >= 20) { Moves.Add(Move.NatureCalling()); }
                    if (Level >= 21) { Moves.Add(Move.Intimidate()); }
                    if (Level >= 53) Moves.Add(Move.TreeHammer());
                    break;
                case 14:
                    if (Level >= 1) { Moves.Add(Move.Tackle()); }
                    if (Level >= 9) { Moves.Add(Move.Smog()); }
                    if (Level >= 20) { Moves.Add(Move.PoisonDart()); }
                    break;
                case 15:
                    if (Level >= 1) { Moves.Add(Move.Tackle()); }
                    if (Level >= 20) { Moves.Add(Move.Torment()); }
                    if (Level >= 43) { Moves.Add(Move.SoulHunt()); }
                    break;
                case 16:
                    if (Level >= 1) { Moves.Add(Move.Headbutt()); }
                    if (Level >= 5) { Moves.Add(Move.TreeHammer()); }
                    if (Level >= 20) { Moves.Add(Move.NatureCalling()); }
                    break;
                case 17:
                    if (Level >= 1) { Moves.Add(Move.Tackle()); }
                    if (Level >= 20) { Moves.Add(Move.Smog()); }
                    break;
                case 18:
                    if (Level >= 1) { Moves.Add(Move.Headbutt()); }
                    if (Level >= 11) { Moves.Add(Move.Scream()); }
                    if (Level >= 17) { Moves.Add(Move.SleepyTune()); }
                    if (Level >= 21) { Moves.Add(Move.AngryTune()); }
                    if (Level >= 34) { Moves.Add(Move.MindTrick()); }
                    break;
                case 19:
                    if (Level >= 1) { Moves.Add(Move.Tackle()); }
                    if (Level >= 8) { Moves.Add(Move.Headbutt()); }
                    if (Level >= 15) { Moves.Add(Move.Flare()); }
                    break;
                case 20:
                    Moves.AddMany(Move.Tackle(), Move.Headbutt(), Move.Flare());
                    if (Level >= 29) { Moves.Add(Move.FlameThrower()); }
                    break;
                case 21:
                    if (Level >= 1) { Moves.Add(Move.Scream()); }
                    if (Level >= 12) { Moves.Add(Move.SleepyTune()); }
                    if (Level >= 13) { Moves.Add(Move.HighPitch()); }
                    if (Level >= 15) { Moves.Add(Move.AngryTune()); }
                    if (Level >= 21) { Moves.Add(Move.DazzlingTune()); }
                    break;
                case 22:
                    if (Level >= 1) { Moves.Add(Move.Headbutt()); }
                    if (Level >= 15) { Moves.Add(Move.LeafCut()); }
                    if (Level >= 21) { Moves.Add(Move.MindClose()); }
                    break;
                case 23:
                    if (Level >= 1) { Moves.Add(Move.Headbutt()); }
                    if (Level >= 9) { Moves.Add(Move.Scream()); }
                    if (Level >= 21) { Moves.Add(Move.HighPitch()); }
                    if (Level >= 31) { Moves.Add(Move.MindClose()); }
                    break;

            }
        }

        public void Update(GameTime gametime) {
            SourceRectangle = new Rectangle(CurrentFrame.X * PartySpriteSize.X, CurrentFrame.Y * PartySpriteSize.Y, PartySpriteSize.X, PartySpriteSize.Y);
            AnimateParty(gametime);
        }
        public void AnimateWorld(GameTime gametime) {
            throw new NotImplementedException();
        }
        public void AnimateFront(GameTime gametime) {
            throw new NotImplementedException();
        }
        public void AnimateBack(GameTime gametime) {
            throw new NotImplementedException();
        }
        public void AnimateParty(GameTime gametime) {
            Timer += (float)gametime.ElapsedGameTime.TotalMilliseconds;
            if (Timer > Interval) {
                CurrentFrame.X++;
                if (CurrentFrame.X > 1) {
                    CurrentFrame.X = 0;
                }
                Timer = 0f;
            }
        }

        private Monster ReturnEvolution(int id) {
            var mon = DatabaseConnector.GetMonster(id, Level);
            var stats = Stats;
            var heldItem = HeldItem;
            mon.Stats = stats;
            mon.Stats.CalculateStats(Level);
            mon.HeldItem = heldItem;
            return mon;
        }
    }
}
