using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using OpenTK.Graphics.ES20;

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
            Texture2D front, Texture2D back, Texture2D party) {
            Id = id;
            UId = RandomId.GenerateRandomUId();
            Level = level;
            experience = level * level * 5;
            RemainingExp = ((level +1)*(level + 1)*5) - experience;
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
        Texture2D front, Texture2D back, Texture2D party) {
            Id = id;
            UId = RandomId.GenerateRandomUId();
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

            var expGain = (a * l * y) ;
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
            for (int i = level; i < 100; i++)
            {
                var lvl = i*i*5;
                RemainingExp = lvl - Experience;
                if (lvl < Experience)
                {
                    level++;
                }
                else break;
            }
            if (level - 1 > Level)
            {
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

        //TODO: Find a way to do this nicer and cleaner
        public void GetMoves() {
            Moves = new List<Move>();
            switch (Id) {
            case 1:
                if (Level >= 1) {
                    Moves.Add(Move.Tackle());
                }
                break;
            case 10:
                if (Level >= 1) {
                    Moves.Add(Move.Strangle());
                    Moves.Add(Move.Glare());
                    Moves.Add(Move.InstantKill());
                }
                if (Level >= 5) { Moves.Add(Move.Tackle()); }
                if (Level >= 9) { Moves.Add(Move.Intimidate()); }
                if (Level >= 11) { Moves.Add(Move.Headbutt()); }
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
        #region Monsters

        public static Monster Armler(int level, Item item = null) {
            if (item == null) item = new Item();
            List<Ability> abilities = new List<Ability> {
                Ability.Buff(),
                Ability.Enraged()
            };
            //Calculate level so we can determine what moves it could have learned

            Stats stats = new Stats(45, 50, 71, 40, 60, 66, level);
            return new Monster(1, level, "Armler", "This shifty creature Likes to pretend that its pockets are its eyes",
                Type.Grass, 75, 5, item, stats, abilities,
                ContentLoader.ArmlerFront, ContentLoader.ArmlerBack, ContentLoader.ArmlerParty);
        }
        public static Monster Pantsler(int level, Item item = null) {
            if (item == null) item = new Item();
            List<Ability> abilities = new List<Ability> {
                Ability.Buff(),
                Ability.Enraged()
            };
            //Calculate level so we can determine what moves it could have learned

            Stats stats = new Stats(60, 64, 90, 50, 75, 56, level);
            return new Monster(2, level, "Pantsler", "While desperately trying to escape from the clutches of its \npants this monster is still able to battle",
                Type.Grass, Type.Fight, 75, 5, item, stats, abilities,
                ContentLoader.PantslerFront, ContentLoader.PantslerBack, ContentLoader.PantslerParty);
        }
        public static Monster Prestler(int level, Item item = null) {
            if (item == null) item = new Item();
            List<Ability> abilities = new List<Ability> {
                Ability.StrongFist()
            };
            //Calculate level so we can determine what moves it could have learned

            Stats stats = new Stats(60, 121, 60, 50, 60, 85, level);
            return new Monster(3, level, "Prestler", "These pants are the ultimate fighter, having consumed its prior wearer",
                Type.Grass, Type.Fight, 75, 5, item, stats, abilities,
                ContentLoader.PrestlerFront, ContentLoader.PrestlerBack, ContentLoader.PrestlerParty);
        }
        public static Monster Mimird(int level, Item item = null) {
            if (item == null) item = new Item();
            List<Ability> abilities = new List<Ability> {
                Ability.Swift(),
                Ability.Evasive()
            };
            //Calculate level so we can determine what moves it could have learned

            Stats stats = new Stats(40, 43, 49, 73, 50, 71, level);
            return new Monster(4, level, "Mimird", "This fish has crafted its own beak! Ain't that endearing",
                Type.Water, 75, 5, item, stats, abilities,
                ContentLoader.MimirdFront, ContentLoader.MimirdBack, ContentLoader.MimirdParty);
        }
        public static Monster Njortor(int level, Item item = null) {
            if (item == null) item = new Item();
            List<Ability> abilities = new List<Ability> {
                Ability.Swift(),
                Ability.Evasive()
            };
            //Calculate level so we can determine what moves it could have learned

            Stats stats = new Stats(55, 43, 52, 83, 59, 109, level);
            return new Monster(5, level, "Njortor", "This mammal has created its own helicopter and attached it to its back",
                Type.Water, Type.Flying, 75, 5, item, stats, abilities,
                ContentLoader.NjortorFront, ContentLoader.NjortorBack, ContentLoader.NjortorParty);
        }
        public static Monster Dvallalin(int level, Item item = null) {
            if (item == null) item = new Item();
            List<Ability> abilities = new List<Ability> {
                Ability.Swift(),
                Ability.Relaxed()
            };
            //Calculate level so we can determine what moves it could have learned

            Stats stats = new Stats(95, 30, 39, 93, 65, 112, level);
            return new Monster(6, level, "Dvallalin", "This whale, being weighed down by its troubles, \nmade a blimp to carry his burdens",
                Type.Water, Type.Flying, 75, 5, item, stats, abilities,
                ContentLoader.DvallalinFront, ContentLoader.DvallalinBack, ContentLoader.DvallalinParty);
        }
        
        // 7 to 9 fire starter

        public static Monster Gronkey(int level, Item item = null) {
            if (item == null) item = new Item();
            List<Ability> abilities = new List<Ability> {
                Ability.Enraged()
            };
            //Calculate level so we can determine what moves it could have learned

            Stats stats = new Stats(45, 66, 40, 40, 45, 85, level);
            return new Monster(10, level, "Gronkey", "This creature is absolutely vivid because someone shaved its face.",
                Type.Fight, 50, 50, item, stats, abilities,
                ContentLoader.GronkeyFront, ContentLoader.GronkeyBack, ContentLoader.GronkeyParty);
        }
        public static Monster Gladkey(int level, Item item = null) {
            if (item == null) item = new Item();
            List<Ability> abilities = new List<Ability> {
                Ability.Relaxed(),
                Ability.Unmovable()
            };
            //Calculate level so we can determine what moves it could have learned

            Stats stats = new Stats(160, 110, 75, 60, 90, 5, level);
            return new Monster(11, level, "Gladkey", "This creature is on cloud 9 after its mustache grew back",
                Type.Fight, 50, 50, item, stats, abilities,
                ContentLoader.GladkeyFront, ContentLoader.GladkeyBack, ContentLoader.GladkeyParty);
        }
        public static Monster Brass(int level, Item item = null) {
            if (item == null) item = new Item();
            List<Ability> abilities = new List<Ability> {
                Ability.Ordinary(),
                Ability.Unmovable()
            };
            //Calculate level so we can determine what moves it could have learned

            Stats stats = new Stats(78, 15, 90, 10, 80, 5, level);
            return new Monster(12, level, "Brass", "This brick is pretty useless, all it can do is lie and wait\nuntil is undeniably faints.",
                Type.Rock, 75, 50, item, stats, abilities,
                ContentLoader.BrassFront, ContentLoader.BrassBack, ContentLoader.BrassParty);
        }
        public static Monster Bonsantai(int level, Item item = null) {
            if (item == null) item = new Item();
            List<Ability> abilities = new List<Ability> {
                Ability.StrongFist()
            };
            //Calculate level so we can determine what moves it could have learned

            Stats stats = new Stats(88, 115, 65, 10, 75, 75, level);
            return new Monster(13, level, "Bonsantai", "After years of training and getting teased, \nthis brick learned the ways of the earth",
                Type.Rock, Type.Grass, 75, 50, item, stats, abilities,
                ContentLoader.BonsantaiFront, ContentLoader.BonsantaiBack, ContentLoader.BonsantaiParty);
        }
        public static Monster Huffstein(int level, Item item = null) {
            if (item == null) item = new Item();
            List<Ability> abilities = new List<Ability> {
                Ability.Fuzzy(),
                Ability.ToxicBody()
            };
            //Calculate level so we can determine what moves it could have learned

            Stats stats = new Stats(60, 42, 83, 76, 65, 55, level);
            return new Monster(14, level, "Huffstein", "Being exposed to smog for so long, it has started to orbit around its body",
                Type.Poison, Type.Rock, 50, 50, item, stats, abilities,
                ContentLoader.HuffsteinFront, ContentLoader.HuffsteinBack, ContentLoader.HuffsteinParty);
        }
        public static Monster Fester(int level, Item item = null) {
            if (item == null) item = new Item();
            List<Ability> abilities = new List<Ability> {
                Ability.AfterImage(),
                Ability.Deaf()
            };
            //Calculate level so we can determine what moves it could have learned

            Stats stats = new Stats(68, 40, 40, 79, 65, 128, level);
            return new Monster(15, level, "Fester", "Being haunted in life, he now haunts his enemies in the afterlife",
                Type.Poison, Type.Ghost, 50, 50, item, stats, abilities,
                ContentLoader.FesterFront, ContentLoader.HuffsteinBack, ContentLoader.HuffsteinParty);
        }
        public static Monster Joiantler(int level, Item item = null) {
            if (item == null) item = new Item();
            List<Ability> abilities = new List<Ability> {
                Ability.Enjoyer(),
                Ability.Relaxed()
            };
            //Calculate level so we can determine what moves it could have learned

            Stats stats = new Stats(100, 85, 100, 78, 97, 110, level);
            return new Monster(16, level, "Joiantler", "Having found its hat nothing can ruin its day",
                Type.Normal, Type.Grass, 50, 50, item, stats, abilities,
                ContentLoader.JoiantlerFront, ContentLoader.JoiantlerBack, ContentLoader.JoiantlerParty);
        }
        public static Monster Rasion(int level, Item item = null) {
            if (item == null) item = new Item();
            List<Ability> abilities = new List<Ability> {
                Ability.Squishy(),
                Ability.Silly()
            };
            //Calculate level so we can determine what moves it could have learned

            Stats stats = new Stats(58, 68, 40, 50, 48, 87, level);
            return new Monster(17, level, "Rasion", "This creature has been completely consumed by poison, \nlosing its conscience",
                Type.Poison, Type.Normal, 50, 50, item, stats, abilities,
                ContentLoader.RasionFront, ContentLoader.RasionBack, ContentLoader.RasionParty);
        }
        #endregion
    }
}
