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

    public class Character : IAnimatable, ITimer
    {
        public float Interval { get; set; } = 200; // Interval at which the animation should update
        public float Timer { get; set; } = 0; // Timer that keeps getting updated until the Interval is reached

        public Vector2 Position; //Position of the character
        public Point SpriteSize = new Point(32, 32);//Height and Width of the sprite
        public Point CurrentFrame = new Point(0, 0);//Frame that is being drawn by the SourceRectangle

        public Rectangle SourceRectangle; //Rectangle needed for animating

        //Collisions
        public Rectangle LineOfSightRectangle;
        public Rectangle Hitbox;

        public Texture2D FrontSprite; //Sprite that is shown when you're fighting against this character
        public Texture2D BackSprite; //Sprite that is shown when you're fighting as this character
        public Texture2D WorldSprite; //Sprite that is shown when you're walking around on the area

        public bool CountingDown;
        public bool Defeated;
        public bool Debug;
        public bool Controllable;
        public Direction Direction;
        public string Name;
        public int Money;
        public Inventory Inventory;
        public List<Monster> Monsters;
        public List<Monster> Box = new List<Monster>();
        public Area CurrentArea;
        public AI AI;
        public float MovementSpeed = 2f;
        public float Moved;


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
        }

        //TODO: Add animation function from DuckDefense

        public void Update(GameTime time, KeyboardState cur, KeyboardState prev)
        {
            if (CountingDown)
            {
                MovementTimer(time);
            }
            else
            {
                GetDirection(cur, prev);
                
                Movement(cur, time);
            }
            SourceRectangle = new Rectangle(CurrentFrame.X*SpriteSize.X, CurrentFrame.Y*SpriteSize.Y, SpriteSize.X, SpriteSize.Y);
            if (CurrentArea != null) Hitbox = new Rectangle((int) Position.X, (int) Position.Y, 32, 32);    
            AnimateWorld(time);
        }

        public void MonsterUpdate(GameTime time) {
            foreach (var m in Monsters) {
                m.Update(time);
            }
        }

        public void Draw(SpriteBatch batch)
        {
            if (Debug)
            {
                batch.Draw(ContentLoader.Button, null, LineOfSightRectangle, null, null, 0f, Vector2.Zero, Color.White);
                batch.Draw(ContentLoader.Button, Hitbox, Color.Red);
            }
            batch.Draw(WorldSprite, Position, SourceRectangle, Color.White);
        }

        public void GetDirection(KeyboardState cur, KeyboardState prev)
        {
            if (Controllable)
            {
                Direction = Direction.None;
                if (cur.IsKeyDown(Settings.moveUp) || cur.IsKeyDown(Keys.Up))
                {
                    Direction = Direction.Up;
                    CurrentFrame.Y = 3;
                }
                if (cur.IsKeyDown(Settings.moveLeft) || cur.IsKeyDown(Keys.Left))
                {
                    Direction = Direction.Left;
                    CurrentFrame.Y = 2;
                }
                if (cur.IsKeyDown(Settings.moveRight) || cur.IsKeyDown(Keys.Right))
                {
                    Direction = Direction.Right;
                    CurrentFrame.Y = 1;
                }
                if (cur.IsKeyDown(Settings.moveDown) || cur.IsKeyDown(Keys.Down))
                {
                    Direction = Direction.Down;
                    CurrentFrame.Y = 0;
                }
            }
        }

        public void SetLineOfSight(int tiles)
        {
            var size = (tiles + 1) * 16;
            switch (Direction)
            {
                case Direction.None:
                    break;
                case Direction.Up:
                    LineOfSightRectangle = new Rectangle((int)Position.X, (int)Position.Y - (size - (WorldSprite.Height / 3)), (WorldSprite.Width / 3), size);
                    break;
                case Direction.Down:
                    LineOfSightRectangle = new Rectangle((int)Position.X, (int)Position.Y, (WorldSprite.Width / 3), size);
                    break;
                case Direction.Right:
                    LineOfSightRectangle = new Rectangle((int)Position.X, (int)Position.Y, size, (WorldSprite.Height) / 4);
                    break;
                case Direction.Left:
                    LineOfSightRectangle = new Rectangle((int)Position.X - (size - (WorldSprite.Width / 3)), (int)Position.Y, size, (WorldSprite.Height / 4));
                    break;
            }
        }
        public void Movement(KeyboardState cur, GameTime time)
        {


            if (Controllable)
            {
                if (cur.IsKeyDown(Settings.moveUp) || cur.IsKeyDown(Keys.Up))
                {
                    Direction = Direction.Up;
                    MovementTimer(time);
                }
                if (cur.IsKeyDown(Settings.moveDown) || cur.IsKeyDown(Keys.Down))
                {
                    Direction = Direction.Down;
                    MovementTimer(time);

                }
                if (cur.IsKeyDown(Settings.moveLeft) || cur.IsKeyDown(Keys.Left))
                {
                    Direction = Direction.Left;
                    MovementTimer(time);
                }
                if (cur.IsKeyDown(Settings.moveRight) || cur.IsKeyDown(Keys.Right))
                {
                    Direction = Direction.Right;
                    MovementTimer(time);
                }
            }

            
            //move here
            //

        }

        public void MovementTimer(GameTime time)
        {
            CountingDown = true;
            float move = (float) (32 * time.ElapsedGameTime.TotalSeconds * 2) ;
            Moved += move;
            switch (Direction)
            {
                case Direction.Up:
                    Position.Y -= move;
                    break;
                case Direction.Down:
                    Position.Y += move;
                    break;
                case Direction.Right:
                    Position.X += move;
                    break;
                case Direction.Left:
                    Position.X -= move;
                    break;
            }
            //if (Timer > Interval - 100)
            if(Moved >= 32)
            {
                CountingDown = false;
                Direction = Direction.None;
                Timer = 0f;
                Moved = 0;
            }
        }

        public void MoveToPoint(Vector2 move, float delay)
        {

            var distanceX = (move.X - Position.X) / delay;
            var distanceY = (move.Y - Position.Y) / delay;
            //var moveX = distanceX/delay;
            //var moveY = distanceY/delay;

            Hitbox = new Rectangle((int)move.X,
                (int)move.Y,
                16, 16);
            if (!Hitbox.Contains(Position))
            {
                Position.X += distanceX * 2;
                Position.Y += distanceY * 2;
            }
        }

        public void AnimateWorld(GameTime gametime)
        {
            if (Direction == Direction.None) CurrentFrame.X = 0;
            else
            {
                Timer += (float)gametime.ElapsedGameTime.TotalMilliseconds;
                if (Timer > Interval)
                {
                    CurrentFrame.X++;
                    if (CurrentFrame.X > WorldSprite.Width / SpriteSize.X - 1)
                    {
                        CurrentFrame.X = 1;
                    }
                    Timer = 0f;
                }
            }
        }

        public void AnimateFront(GameTime gametime)
        {
            throw new NotImplementedException();
        }

        public void AnimateBack(GameTime gametime)
        {
            throw new NotImplementedException();
        }

        public void AnimateParty(GameTime gametime)
        {
            throw new NotImplementedException();
        }
    }
}
