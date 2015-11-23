﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;

namespace VideoGame.Classes {

    public enum Direction {
        None,
        Up,
        Down,
        Right,
        Left
    }
    class Character {
        public float Interval { get; set; } // Interval at which the animation should update
        private float Timer { get; set; } // Timer that keeps getting updated until the Interval is reached

        private Vector2 position;
        public Vector2 Position { get { return position; } set { position = value; } } //Position of the character

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
        public Area CurrentArea;
        public Camera2D Camera;

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
        /// <param name="camera"></param>
        public Character(string name, int money, Inventory inventory, List<Monster> monsters,
        Texture2D front, Texture2D back, Texture2D world, Vector2 position, Camera2D camera) {
            Name = name;
            Money = money;
            Inventory = inventory;
            Monsters = monsters;
            Controllable = true;
            FrontSprite = front;
            BackSprite = back;
            WorldSprite = world;
            Position = position;
            Camera = camera;

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
        Texture2D front, Texture2D back, Texture2D world, Vector2 position) {
            Name = name;
            Money = money;
            Inventory = inventory;
            Monsters = monsters;
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

        public void EnterArea(ScalingViewportAdapter viewport) {
            Camera = new Camera2D(viewport) {
                Zoom = 0.5f,
                Position = new Vector2(CurrentArea.Map.WidthInPixels / 4f,
                CurrentArea.Map.HeightInPixels / 4f)
            };
        }

        public void Update(GameTime time, KeyboardState cur, KeyboardState prev) {
            if (Controllable) {
                Movement(time, cur, prev);
            }

            // Add timer here
        }

        public void Draw(SpriteBatch batch) {
            batch.Draw(WorldSprite, Position, Color.White);
        }

        public void Movement(GameTime gameTime, KeyboardState cur, KeyboardState prev) {
            if (Keyboard.GetState().IsKeyDown(Keys.W) || Keyboard.GetState().IsKeyDown(Keys.Up))
                Direction = Direction.Up;
            else if (Keyboard.GetState().IsKeyDown(Keys.S) || Keyboard.GetState().IsKeyDown(Keys.Down))
                Direction = Direction.Down;
            else if (Keyboard.GetState().IsKeyDown(Keys.A) || Keyboard.GetState().IsKeyDown(Keys.Left))
                Direction = Direction.Left;
            else if (Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.Right))
                Direction = Direction.Right;
            else Direction = Direction.None;

            //TODO: Add grid movement
            switch (Direction) {
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
            //Camera.Move(Position);
        }
    }
}
