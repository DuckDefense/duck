using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VideoGame.Classes {
    class Character {
        public float Interval { get; set; } // Interval at which the animation should update
        private float Timer = { get; set; } // Timer that keeps getting updated until the Interval is reached
        
        public Vector2 Position { get; set; } //Position of the character
        public Point SpriteSize { get; set; } //Height and Width of the sprite
        public Point CurrentFrame { get; set; } //Frame that is being drawn by the SourceRectangle
        public Rectangle PositionRectangle { get; set; } //Rectangle used as collision
        public Rectangle SourceRectangle; //Rectangle needed for animating
        
        public Texture2D FrontSprite; //Sprite that is shown when you're fighting against this character
        public Texture2D BackSprite; //Sprite that is shown when you're fighting as this character
        public Texture2D WorldSprite; //Sprite that is shown when you're walking around on the area
        
        public bool Controllable;
        public string Name;
        public int Money;
        public Inventory Inventory;
        public List<Monster> Monsters;

        /// <summary>
        /// New character
        /// </summary>
        /// <param name="name">Characters name</param>
        /// <param name="money">Amount of money the character has</param>
        /// <param name="inventory">Inventory of the character</param>
        /// <param name="monsters">Monsters that the character has</param>
        /// <param name="front">Sprite that is shown when you're fighting against this character</param>
        /// <param name="back">Sprite that is shown when you're fighting as this character</param>
        /// <param name="world">Sprite that is shown when you're walking around on the area</param>
        /// <param name="position">Position of the character</param>
        /// <param name="controllable">Is this a playable character</param>
        public Character(string name, int money, Inventory inventory, List<Monster> monsters, 
        Texture2D front, Texture2D back, Texture3D world, Vector2 position, bool controllable = false) {
            Name = name;
            Money = money;
            Inventory = inventory;
            Monsters = monsters;
            Controllable = controllable;
            FrontSprite = font;
            BackSprite = back;
            WorldSprite = world;
            Position = position;
            
            SpriteSize = new Point(WorldSprite.Width, WorldSprite.Height);
            PositionRectangle = new Rectangle(Position.X, Position.Y, SpriteSize.Width, SpriteSize.Height);
            SourceRectangle = new Rectangle();
        }
    }
}
