using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace VideoGame.Classes {

    public enum Gender {
        NoGender,
        Male,
        Female
    }

    public class Monster {

        public bool IsDead => Stats.Health >= 0; //Returns true if health is 0 or below it

        public Texture2D FrontSprite; //Sprite that is shown when you're fighting this monster
        public Texture2D BackSprite; //Sprite that is shown when you've send out this monster
        public Texture2D PartySprite; //Sprite that is shown if you open the party
        //Maybe add Points for Sprite sizes
        //If we are going add monsters in the world we need to add a position and a collision box
        public int Experience;
        public int Level;
        public int Id;
        public string Name;
        public string Description;
        public Stats PreviousStats;
        public Stats Stats;
        public Type PrimaryType;
        public Type SecondaryType;
        public Item HeldItem; //Item the monster is currently holding
        public Ailment Ailment;
        public Gender Gender;
        public Ability Ability;
        public double CriticalHitChance = 5;
        public double CriticalHitMultiplier = 2;
        public List<Move> Moves;
        public List<Move> KnownMoves;


        /// <summary>
        /// Monster with one type
        /// </summary>
        /// <param name="id">ID of the monster</param>
        /// <param name="name">Name</param>
        /// <param name="description">Short description of the monster</param>
        /// <param name="maleChance">Chance for the monster to be male</param>
        /// <param name="helditem">Item the monster is carrying</param>
        /// <param name="stats">Stats the monster has</param>
        /// <param name="type">Type which changes how much damage certain moves do</param>
        /// <param name="moves">Attacks the monster knows</param>
        /// <param name="abilities">All possible abilities the monster can have</param>
        /// <param name="front">Texture that is shown when fighting against this monster</param>
        /// <param name="back">Texture that is shown when you've send out this monster</param>
        /// <param name="party">Texture that is shown in the party view</param>
        public Monster(int id, string name, string description, Type type, int maleChance, Item helditem, Stats stats, List<Move> moves, List<Ability> abilities,
            Texture2D front, Texture2D back, Texture2D party) {
            Id = id;
            Name = name;
            Description = description;
            PrimaryType = type;
            SecondaryType = Type.None;
            Gender = GetGender(maleChance);
            HeldItem = helditem;
            Stats = stats;
            Moves = moves;
            Ability = GetAbility(abilities);
            FrontSprite = front;
            BackSprite = back;
            PartySprite = party;
            Ailment = Ailment.Normal;
        }

        /// <summary>
        /// Monster with two types
        /// </summary>
        /// <param name="id">Id of the monster</param>
        /// <param name="name">Name</param>
        /// <param name="maleChance">Chance for the monster to be male</param>
        /// <param name="helditem">Item the monster is carrying</param>
        /// <param name="stats">Stats the monster has</param>
        /// <param name="description">Short description of the monster</param>
        /// <param name="primaryType">Primary type which changes how much damage certain moves do</param>
        /// <param name="secondaryType">Secondary type which changes how much damage certain moves do</param>
        /// <param name="moves">Attacks the monster knows</param>
        /// <param name="abilities">All possible abilities the monster can have</param>
        /// <param name="front">Texture that is shown when fighting against this monster</param>
        /// <param name="back">Texture that is shown when you've send out this monster</param>
        /// <param name="party">Texture that is shown in the party view</param>
        public Monster(int id, string name, string description, Type primaryType, Type secondaryType, int maleChance, Item helditem, Stats stats, List<Move> moves, List<Ability> abilities,
        Texture2D front, Texture2D back, Texture2D party) {
            Name = name;
            Description = description;
            PrimaryType = primaryType;
            SecondaryType = secondaryType;
            Gender = GetGender(maleChance);
            HeldItem = helditem;
            Stats = stats;
            Moves = moves;
            Ability = GetAbility(abilities);
            FrontSprite = front;
            BackSprite = back;
            PartySprite = party;
            Ailment = Ailment.Normal;
        }

        public void ReceiveExp(int amount) {
            //TODO: Add a experience calculation which will return level based on the amount of experience it has
        }

        public void LevelUp(int amount, int id) {
            GetMoves(id);
            //If we are limiting moves put a prompt here asking if the monster should learn this move
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
        public void GetMoves(int id) {
            KnownMoves.Clear();
            switch (id) {
            case 1:
                if (Level >= 1) { 
                    KnownMoves.Add(Move.Tackle());
                }
                break;
            case 10:
                if (Level >= 1) {
                    KnownMoves.Add(Move.Strangle());
                    KnownMoves.Add(Move.Glare());
                }
                if (Level >= 5) {
                    KnownMoves.Add(Move.Tackle());
                }
                if (Level >= 9) {
                    KnownMoves.Add(Move.Intimidate());
                }
                break;
            }
        }

        public static Monster Armler(int level, Item item = null) {
            if (item == null) item = new Item();
            List<Move> moves = new List<Move>();
            List<Ability> abilities = new List<Ability> {
                Ability.Buff(),
                Ability.Enraged()
            };
            //Calculate level so we can determine what moves it could have learned

            Stats stats = new Stats(45, 50, 71, 40, 60, 66, level);
            return new Monster(1, "Armler", "This shifty creature Likes to pretend that his pockets are its eyes", Type.Grass, 75, item, stats, moves, abilities,
                ContentLoader.GronkeyFront, ContentLoader.GronkeyBack, ContentLoader.GronkeyParty);
        }

        public static Monster Gronkey(int level, Item item = null) {
            if (item == null) item = new Item();
            List<Move> moves = new List<Move>();
            List<Ability> abilities = new List<Ability> {
                Ability.Enraged()
            };
            //Calculate level so we can determine what moves it could have learned

            Stats stats = new Stats(45, 66, 40, 40, 45, 85, level);
            return new Monster(10, "Gronkey", "This creature is absolutely vivid because someone shaved its face.", Type.Fight, 50, item, stats, moves, abilities,
                ContentLoader.GronkeyFront, ContentLoader.GronkeyBack, ContentLoader.GronkeyParty);
        }

    }
}
