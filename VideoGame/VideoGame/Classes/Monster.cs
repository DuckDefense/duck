using System;
using System.Collections.Generic;
using System.Linq;
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
        public Texture2D FrontSprite;   //Sprite that is shown when you're fighting this monster
        public Texture2D BackSprite;    //Sprite that is shown when you've send out this monster
        public Texture2D PartySprite;   //Sprite that is shown if you open the party
        //Maybe add Points for Sprite sizes
        //If we are going add monsters in the world we need to add a position and a collision box
        public int Experience;
        public int Level;
        public int Id;
        public string Name;
        public Stats PreviousStats;
        public Stats Stats;
        public Type PrimaryType;
        public Type SecondaryType;
        public Ailment Ailment;
        public Gender Gender;
        public List<Move> Moves;


        /// <summary>
        /// Monster with one type
        /// </summary>
        /// <param name="id">ID of the monster</param>
        /// <param name="name">Name</param>
        /// <param name="stats">Stats the monster has</param>
        /// <param name="type">Type which changes how much damage certain moves do</param>
        /// <param name="gender">Gender of the monster</param>
        /// <param name="moves">Attacks the monster knows</param>
        /// <param name="front">Texture that is shown when fighting against this monster</param>
        /// <param name="back">Texture that is shown when you've send out this monster</param>
        /// <param name="party">Texture that is shown in the party view</param>
        public Monster(int id, string name, Type type, Gender gender, Stats stats, List<Move> moves,
        Texture2D front, Texture2D back, Texture2D party) {
            Id = id;
            Name = name;
            PrimaryType = type;
            Gender = gender;
            Stats = stats;
            Moves = moves;
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
        /// <param name="stats">Stats the monster has</param>
        /// <param name="primaryType">Primary type which changes how much damage certain moves do</param>
        /// <param name="secondaryType">Secondary type which changes how much damage certain moves do</param>
        /// <param name="gender">Gender of the monster</param>
        /// <param name="moves">Attacks the monster knows</param>
        /// <param name="front">Texture that is shown when fighting against this monster</param>
        /// <param name="back">Texture that is shown when you've send out this monster</param>
        /// <param name="party">Texture that is shown in the party view</param>
        public Monster(int id, string name, Type primaryType, Type secondaryType, Gender gender, Stats stats, List<Move> moves,
        Texture2D front, Texture2D back, Texture2D party) {
            Name = name;
            PrimaryType = primaryType;
            SecondaryType = secondaryType;
            Gender = gender;
            Stats = stats;
            Moves = moves;
            FrontSprite = front;
            BackSprite = back;
            PartySprite = party;
            Ailment = Ailment.Normal;
        }

        public void ReceiveExp(int amount) {
            //TODO: Add a experience calculation which will return level based on the amount of experience it has
        }

        public void LevelUp(int amount, int id) {
            //TODO: Add a stat calculation to increase stats on level up

            //Also add learnable moves here. eg
            switch (id) {
            case 1:
                if (Level == 7)
                    Moves.Add(Move.Bubble());
                break;
            }
        }

        public static Monster Gronkey(ContentManager content, int level) {
            List<Move> moves = new List<Move>();
            //TODO: Add levels and stat scaling here
            Stats stats = new Stats(20, 12, 8, 5, 6, 10, level);
            return new Monster(1, "Gronkey", Type.Fight, Gender.Male, stats, moves,
                content.Load<Texture2D>(@"Sprites/Monsters/Front/Grumpy Monkey Front"),
                content.Load<Texture2D>(@"Sprites/Monsters/Back/Grumpy Monkey Back"),
                content.Load<Texture2D>(@"Sprites/Monsters/Party/Grumpy Monkey Party")
                );
        }
    }
}
