using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;

namespace VideoGame.Classes
{

    public enum Direction
    {
        None,
        Up,
        Down,
        Right,
        Left
    }

    public class Character
    {
        public float Interval { get; set; } // Interval at which the animation should update
        private float Timer { get; set; } // Timer that keeps getting updated until the Interval is reached

        private Vector2 position;
        public Vector2 Position { get { return position; } set { position = value; } } //Position of the character

        public Point SpriteSize { get; set; } //Height and Width of the sprite
        public Point CurrentFrame { get; set; } //Frame that is being drawn by the SourceRectangle
        public Rectangle PositionRectangle { get; set; } //Rectangle used as collision
        public Rectangle SourceRectangle; //Rectangle needed for animating
        public Rectangle LineOfSightRectangle;

        public Texture2D FrontSprite; //Sprite that is shown when you're fighting against this character
        public Texture2D BackSprite; //Sprite that is shown when you're fighting as this character
        public Texture2D WorldSprite; //Sprite that is shown when you're walking around on the area

        public bool Debug;
        public bool Controllable;
        public Direction Direction;
        public string Name;
        public int Money;
        public Inventory Inventory;
        public List<Monster> Monsters;
        public Monster CurrentMonster;
        public Area CurrentArea;

        /// <summary>
        /// New moveable character
        /// </summary>
        /// <param name="name">Characters name</param>
        /// <param name="money">Amount of money the character has</param>
        /// <param name="inventory">Inventory of the character</param>
        /// <param name="monsters">Monsters that the character has</param>
        /// <param name="front">Sprite that is shown when you're fighting against this character</param>
        /// <param name="back">Sprite that is shown when you're fighting as this character</param>
        /// <param name="world">Sprite that is shown when you're walking around on the area</param>
        /// <param name="position">Position of the character</param>
        /// <param name="controllable"></param>
        public Character(string name, int money, Inventory inventory, List<Monster> monsters,
        Texture2D front, Texture2D back, Texture2D world, Vector2 position, bool controllable)
        {
            Name = name;
            Money = money;
            Inventory = inventory;
            Monsters = monsters;
            foreach (Monster monster in Monsters)
            {
                monster.IsWild = false;
            }
            Controllable = controllable;
            FrontSprite = front;
            BackSprite = back;
            WorldSprite = world;
            Position = position;

            SpriteSize = new Point(WorldSprite.Width, WorldSprite.Height);
            PositionRectangle = new Rectangle((int)Position.X, (int)Position.Y, SpriteSize.X, SpriteSize.Y);
            SourceRectangle = new Rectangle();
        }

        /// <summary>
        /// New Non Playable Character
        /// </summary>
        /// <param name="name">Characters name</param>
        /// <param name="money">Amount of money the character has</param>
        /// <param name="inventory">Inventory of the character</param>
        /// <param name="monsters">Monsters that the character has</param>
        /// <param name="front">Sprite that is shown when you're fighting against this character</param>
        /// <param name="back">Sprite that is shown when you're fighting as this character</param>
        /// <param name="world">Sprite that is shown when you're walking around on the area</param>
        /// <param name="position">Position of the character</param>
        public Character(string name, int money, Inventory inventory, List<Monster> monsters,
        Texture2D front, Texture2D back, Texture2D world, Vector2 position)
        {
            Name = name;
            Money = money;
            Inventory = inventory;
            Monsters = monsters;
            foreach (Monster monster in Monsters)
            {
                monster.IsWild = false;
            }
            Controllable = false;
            FrontSprite = front;
            BackSprite = back;
            WorldSprite = world;
            Position = position;

            SpriteSize = new Point(WorldSprite.Width, WorldSprite.Height);
            PositionRectangle = new Rectangle((int)Position.X, (int)Position.Y, SpriteSize.X, SpriteSize.Y);
            SourceRectangle = new Rectangle();
        }

        //TODO: Add animation function from DuckDefense

        public void Update(GameTime time, KeyboardState cur, KeyboardState prev)
        {
            foreach (var m in Monsters) {
                m.Update(time);
            }
            //Add update and sourcerectangle here
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(WorldSprite, Position, Color.White);
            if (Debug)
                batch.Draw(ContentLoader.Button, null, LineOfSightRectangle, null, null, 0f, Vector2.Zero, Color.White);
        }

        public void SetLineOfSight(int tiles)
        {
            var size = (tiles + 1) * 16;
            switch (Direction)
            {
                case Direction.None:
                    break;
                case Direction.Up:
                    LineOfSightRectangle = new Rectangle((int)Position.X, (int)Position.Y - (size - WorldSprite.Height), WorldSprite.Width, size);
                    break;
                case Direction.Down:
                    LineOfSightRectangle = new Rectangle((int)Position.X, (int)Position.Y, WorldSprite.Width, size);
                    break;
                case Direction.Right:
                    LineOfSightRectangle = new Rectangle((int)Position.X, (int)Position.Y, size, WorldSprite.Height);
                    break;
                case Direction.Left:
                    LineOfSightRectangle = new Rectangle((int)Position.X - (size - WorldSprite.Width), (int)Position.Y, size, WorldSprite.Height);
                    break;
            }

        }
    }
}
