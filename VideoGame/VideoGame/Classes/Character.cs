using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace VideoGame.Classes {

    public enum Direction
    {
        None,
        Up,
        Down,
        Right,
        Left
    }
    class Character {
        public float Interval { get; set; } // Interval at which the animation should update
        private float Timer { get; set; } // Timer that keeps getting updated until the Interval is reached

        private Vector2 position = new Vector2(0,0);
        public Vector2 Position { get {return position;}
            set { position = value; }
        } //Position of the character
        public Point SpriteSize { get; set; } //Height and Width of the sprite
        public Point CurrentFrame { get; set; } //Frame that is being drawn by the SourceRectangle
        public Rectangle PositionRectangle { get; set; } //Rectangle used as collision
        public Rectangle SourceRectangle; //Rectangle needed for animating

        public Texture2D FrontSprite; //Sprite that is shown when you're fighting against this character
        public Texture2D BackSprite; //Sprite that is shown when you're fighting as this character
        public Texture2D WorldSprite; //Sprite that is shown when you're walking around on the area

        public bool Controllable;
        public Direction Direction;
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
        Texture2D front, Texture2D back, Texture2D world, Vector2 position, bool controllable = false) {
            Name = name;
            Money = money;
            Inventory = inventory;
            Monsters = monsters;
            Controllable = controllable;
            FrontSprite = front;
            BackSprite = back;
            WorldSprite = world;
            Position = position;

            SpriteSize = new Point(WorldSprite.Width, WorldSprite.Height);
            PositionRectangle = new Rectangle((int)Position.X, (int)Position.Y, SpriteSize.X, SpriteSize.Y);
            SourceRectangle = new Rectangle();
        }

        //TODO: Add animation function from DuckDefense
        
        public void Update(GameTime time) {
            if (Controllable)
            {
                Movement(time);
            }
           
            // Add timer here
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Begin();
            batch.Draw(FrontSprite, Position, Color.White);
            batch.End();
        }
        public void Movement(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.W) || Keyboard.GetState().IsKeyDown(Keys.Up)) Direction = Direction.Up;
            else if (Keyboard.GetState().IsKeyDown(Keys.S) || Keyboard.GetState().IsKeyDown(Keys.Down)) Direction = Direction.Down;
            else if (Keyboard.GetState().IsKeyDown(Keys.A) || Keyboard.GetState().IsKeyDown(Keys.Left)) Direction = Direction.Left;
            else if (Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.Right))
                Direction = Direction.Right;
            else Direction = Direction.None;
            switch (Direction)
            {
                case Direction.Up:
                    position.Y -= 2;
                    break;
                case Direction.Down:
                    position.Y += 2;
                    break;
                case Direction.Right:
                    position.X += 2;
                    break;
                case Direction.Left:
                    position.X -= 2;
                    break;
            }
        }

    }
}
