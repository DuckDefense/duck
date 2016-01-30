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
using Sandbox.Classes;

namespace VideoGame.Classes {

    public enum Direction {
        None,
        Up,
        Down,
        Right,
        Left
    }

    public enum NPCKind
    {
        Healer,
        Shop,
        Trainer
    }

    public class Character : IAnimatable, ITimer {
        public int Id;
        public float Interval { get; set; } = 200; // Interval at which the animation should update
        public float Timer { get; set; } = 0; // Timer that keeps getting updated until the Interval is reached

        public Vector2 Position; //Position of the character
        public Rectangle LeftTile, RightTile, UpperTile, LowerTile; //Tiles that surround the character
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
        public bool Talking;
        public Direction Direction;
        public NPCKind NpcKind;
        public string Name;
        public int Money;
        public Inventory Inventory;
        public List<Monster> Monsters;
        public Dictionary<int, Monster> KnownMonsters = new Dictionary<int, Monster>();
        //public Dictionary<int, Monster> CaughtMonster = new Dictionary<int, Monster>();
        public List<Monster> Box = new List<Monster>();

        public Conversation.Message BattleMessage;
        public Conversation.Message WinMessage;
        public Conversation.Message LoseMessage;
        public Conversation.Message IntroMessage;
        public Conversation.Message ByeMessage;

        public Area CurrentArea;
        public Area PreviousArea;
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
        Texture2D front, Texture2D back, Texture2D world, Vector2 position, bool controllable, bool database = false) {
            if (!database) Id = RandomId.GenerateUserId();
            Name = name;
            Money = money;
            Inventory = inventory;
            Monsters = monsters;
            foreach (Monster monster in Monsters) {
                monster.IsWild = false;
            }
            Controllable = controllable;
            FrontSprite = front;
            BackSprite = back;
            WorldSprite = world;
            Position = position;
        }

        /// <summary>
        /// Non Playable trainer
        /// </summary>
        /// <param name="name">Characters name</param>
        /// <param name="money">Amount of money the character has</param>
        /// <param name="inventory">Inventory of the character</param>
        /// <param name="monsters">Monsters that the character has</param>
        /// <param name="battleMessage">Lines the character should say when starting a battle with the player</param>
        /// <param name="winMessage">Lines the character should say when winning in a battle against the player</param>
        /// <param name="loseMessage">Lines the character should say when losing in a battle against the player</param>
        /// <param name="front">Sprite that is shown when you're fighting against this character</param>
        /// <param name="back">Sprite that is shown when you're fighting as this character</param>
        /// <param name="world">Sprite that is shown when you're walking around on the area</param>
        /// <param name="position">Position of the character</param>
        public Character(string name, int money, Inventory inventory, List<Monster> monsters, 
            List<string> battleMessage, List<string> winMessage, List<string> loseMessage,
            Texture2D front, Texture2D back, Texture2D world, Vector2 position) {
            Name = name;
            Money = money;
            Inventory = inventory;
            Monsters = monsters;
            foreach (Monster monster in Monsters) { monster.IsWild = false; }
            BattleMessage = new Conversation.Message(battleMessage, Color.Black, this);
            WinMessage = new Conversation.Message(winMessage, Color.Black, this); 
            LoseMessage = new Conversation.Message(loseMessage, Color.Black, this);
            Controllable = false;
            FrontSprite = front;
            BackSprite = back;
            WorldSprite = world;
            Position = position;
            NpcKind = NPCKind.Trainer;
            }

        /// <summary>
        /// Shop character
        /// </summary>
        /// <param name="name"></param>
        /// <param name="money"></param>
        /// <param name="inventory"></param>
        /// <param name="introMessage"></param>
        /// <param name="byeMessage"></param>
        /// <param name="front"></param>
        /// <param name="back"></param>
        /// <param name="world"></param>
        /// <param name="position"></param>
        public Character(string name, int money, Inventory inventory, List<string> introMessage, List<string> byeMessage, NPCKind npcKind, 
            Texture2D front, Texture2D back, Texture2D world, Vector2 position) {
            Name = name;
            Money = money;
            Inventory = inventory;
            Controllable = false;
            IntroMessage = new Conversation.Message(introMessage, Color.Black, this);
            ByeMessage = new Conversation.Message(byeMessage, Color.Black, this);
            FrontSprite = front;
            BackSprite = back;
            WorldSprite = world;
            Position = position;
            NpcKind = npcKind;
            }

        public Character(string name)
        {
            Name = name;
        }

        public void Update(GameTime time, KeyboardState cur, KeyboardState prev) {
            if (!Talking) {
                if (CountingDown) {
                    MovementTimer(time);
                }
                else {
                    GetDirection(cur, prev);
                    Movement(cur, time);
                }
            }
            SourceRectangle = new Rectangle(CurrentFrame.X * SpriteSize.X, CurrentFrame.Y * SpriteSize.Y, SpriteSize.X,
                    SpriteSize.Y);
            if (CurrentArea != null) Hitbox = new Rectangle((int)Position.X, (int)Position.Y, 32, 32);
            if (Direction == Direction.None) CurrentFrame.X = 0;
            else
                AnimateWorld(time);

        }

        public void MonsterUpdate(GameTime time) {
            foreach (var m in Monsters) {
                m.Update(time);
            }
        }

        public void Draw(SpriteBatch batch) {
            if (Debug) {
                batch.Draw(ContentLoader.Health, LineOfSightRectangle, Color.White);
                batch.Draw(ContentLoader.Button, Hitbox, Color.Red);
                //Tiles
                batch.Draw(ContentLoader.Button, LeftTile, Color.LemonChiffon);
                batch.Draw(ContentLoader.Button, RightTile, Color.Red);
                batch.Draw(ContentLoader.Button, UpperTile, Color.Turquoise);
                batch.Draw(ContentLoader.Button, LowerTile, Color.BurlyWood);
            }
            if (Talking) { DrawMessages(batch); }
            batch.Draw(WorldSprite, new Vector2(Position.X, Position.Y - 10), SourceRectangle, Color.White);
        }

        public void DrawMessages(SpriteBatch batch) {
            if (BattleMessage != null) {
                if (BattleMessage.Visible) BattleMessage.Draw(batch);
                if (WinMessage.Visible) WinMessage.Draw(batch);
                if (LoseMessage.Visible) LoseMessage.Draw(batch);
            }
            if (IntroMessage != null) {
                if (IntroMessage.Visible) IntroMessage.Draw(batch);
                if (ByeMessage.Visible) ByeMessage.Draw(batch);
            }
        }

        public void UpdateMessages(KeyboardState curKey, KeyboardState prevKey, Character player) {
            if (BattleMessage != null) {
                BattleMessage.Update(curKey, prevKey, player);
                WinMessage.Update(curKey, prevKey, player);
                LoseMessage.Update(curKey, prevKey, player);
            }
            if (IntroMessage != null) {
                IntroMessage.Update(curKey, prevKey, player);
                ByeMessage.Update(curKey, prevKey, player);
            }
        }

        public void GetDirection(KeyboardState cur, KeyboardState prev) {
            if (Controllable) {
                Direction = Direction.None;
                if (cur.IsKeyDown(Settings.moveUp) || cur.IsKeyDown(Keys.Up)) {
                    Direction = Direction.Up;
                    CurrentFrame.Y = 3;
                }
                if (cur.IsKeyDown(Settings.moveLeft) || cur.IsKeyDown(Keys.Left)) {
                    Direction = Direction.Left;
                    CurrentFrame.Y = 2;
                }
                if (cur.IsKeyDown(Settings.moveRight) || cur.IsKeyDown(Keys.Right)) {
                    Direction = Direction.Right;
                    CurrentFrame.Y = 1;
                }
                if (cur.IsKeyDown(Settings.moveDown) || cur.IsKeyDown(Keys.Down)) {
                    Direction = Direction.Down;
                    CurrentFrame.Y = 0;
                }
            }
        }

        public void SetLineOfSight(int tiles) {
            var size = (tiles) * 32;
            switch (Direction) {
            case Direction.None: break;
            case Direction.Up:
                LineOfSightRectangle = new Rectangle((int)Position.X + 12, (int)Position.Y - (size), 8, size-1); 
                break;
            case Direction.Down:
                LineOfSightRectangle = new Rectangle((int)Position.X + 12, (int)Position.Y + 32, (8), size-1);
                break;
            case Direction.Right:
                LineOfSightRectangle = new Rectangle((int)Position.X + 32, (int)Position.Y+12, size-1, (8));
                break;
            case Direction.Left:
                LineOfSightRectangle = new Rectangle((int)Position.X - (size) , (int)Position.Y + 12, size, (8));
                break;
            }
        }
        public void Movement(KeyboardState cur, GameTime time) {
            if (Controllable) {
                if (cur.IsKeyDown(Settings.moveUp) || cur.IsKeyDown(Keys.Up)) {
                    Direction = Direction.Up;
                    MovementTimer(time);
                }
                else if (cur.IsKeyDown(Settings.moveDown) || cur.IsKeyDown(Keys.Down)) {
                    Direction = Direction.Down;
                    MovementTimer(time);

                }
                else if (cur.IsKeyDown(Settings.moveLeft) || cur.IsKeyDown(Keys.Left)) {
                    Direction = Direction.Left;
                    MovementTimer(time);
                }
                else if (cur.IsKeyDown(Settings.moveRight) || cur.IsKeyDown(Keys.Right)) {
                    Direction = Direction.Right;
                    MovementTimer(time);
                }
            }
        }

        public void MovementTimer(GameTime time) {
            CountingDown = true;
            float move = (float)(32 * time.ElapsedGameTime.TotalSeconds * 4);
            Moved += move;
            switch (Direction) {
            case Direction.Up: if (Position.Y > 0) Position.Y -= move; break;
            case Direction.Down: if (Position.Y < (Settings.ResolutionHeight - 32)) Position.Y += move; break;
            case Direction.Right: if (Position.X < (Settings.ResolutionWidth - 32)) Position.X += move; break;
            case Direction.Left: if (Position.X > 0) Position.X -= move; break;
            }
            SetTilePositions();
            if (Moved >= 32) {
                CountingDown = false;
                Moved = 0;
                //CheckGrid();
            }
        }

        public bool LeftCollide(Rectangle collision) { return collision.Intersects(LeftTile); }
        public bool RightCollide(Rectangle collision) { return collision.Intersects(RightTile); }
        public bool UpperCollide(Rectangle collision) { return collision.Intersects(UpperTile); }
        public bool LowerCollide(Rectangle collision) { return collision.Intersects(LowerTile); }

        private void SetTilePositions() {
            LeftTile = new Rectangle((int)Position.X - 16, (int)Position.Y + 16, 4, 4);
            RightTile = new Rectangle((int)Position.X + (16 + SpriteSize.X) - 2, (int)Position.Y + 16, 4, 4);
            UpperTile = new Rectangle((int)Position.X + SpriteSize.X / 2, (int)Position.Y - (16), 4, 4);
            LowerTile = new Rectangle((int)Position.X + SpriteSize.X / 2, (int)Position.Y + (16 + SpriteSize.Y), 4, 4);
        }

        public void AnimateWorld(GameTime gametime) {
            Timer += (float)gametime.ElapsedGameTime.TotalMilliseconds;
            if (Timer > Interval) {
                CurrentFrame.X++;
                if (CurrentFrame.X > WorldSprite.Width / SpriteSize.X - 1) {
                    CurrentFrame.X = 0;
                }
                Timer = 0f;
            }
        }

        public void AnimateFront(GameTime gametime) {
            throw new NotImplementedException();
        }

        public void AnimateBack(GameTime gametime) {
            throw new NotImplementedException();
        }

        public void AnimateParty(GameTime gametime) {
            throw new NotImplementedException();
        }
    }
}
