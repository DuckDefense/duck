using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        /// <param name="name">Name</param>
        /// <param name="stats">Stats the monster has</param>
        /// <param name="type">Type which changes how much damage certain moves do</param>
        /// <param name="gender">Gender of the monster</param>
        /// <param name="ailment">Ailment the monster has</param>
        /// <param name="moves">Attacks the monster knows</param>
        /// <param name="front">Texture that is shown when fighting against this monster</param>
        /// <param name="back">Texture that is shown when you've send out this monster</param>
        /// <param name="party">Texture that is shown in the party view</param>
        public Monster(string name, Stats stats, Type type, Gender gender, Ailment ailment, List<Move> moves,
        Texture2D front, Texture2D back, Texture2D party) {
            Name = name;
            Stats = stats;
            PrimaryType = type;
            Gender = gender;
            Ailment = ailment;
            Moves = moves;
            FrontTexture = front;
            BackTexture = back;
            PartyTexture = party;
        }

        /// <summary>
        /// Monster with two types
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="stats">Stats the monster has</param>
        /// <param name="primaryType">Primary type which changes how much damage certain moves do</param>
        /// <param name="secondaryType">Secondary type which changes how much damage certain moves do</param>
        /// <param name="gender">Gender of the monster</param>
        /// <param name="ailment">Ailment the monster has</param>
        /// <param name="moves">Attacks the monster knows</param>
        /// <param name="front">Texture that is shown when fighting against this monster</param>
        /// <param name="back">Texture that is shown when you've send out this monster</param>
        /// <param name="party">Texture that is shown in the party view</param>
        public Monster(string name, Stats stats, Type primaryType, Type secondaryType, Gender gender, Ailment ailment, List<Move> moves,
        Texture2D front, Texture2D back, Texture2D party) {
            Name = name;
            Stats = stats;
            PrimaryType = primaryType;
            SecondaryType = secondaryType;
            Gender = gender;
            Ailment = ailment;
            Moves = moves;
            FrontTexture = front;
            BackTexture = back;
            PartyTexture = party;
        }
        
        public void ReceiveExp(int amount) {
            //TODO: Add a experience calculation which will return level based on the amount of experience it has
        }

        public void LevelUp(int amount) {
            //TODO: Add a stat calculation to increase stats on level up
        }
    }
}
