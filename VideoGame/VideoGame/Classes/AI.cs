using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VideoGame.Classes
{
    public class AI
    {
        private Character character;
        public Rectangle Hitbox;
        private int tiles;
        public string Message;

        public AI(Character character, int sightSize, string message)
        {
            Message = message;
            this.character = character;
            tiles = sightSize;
            RandomizeDirection();
        }

        public void Update(GameTime gameTime, Character player, ref bool allowedToWalk, ref Battle battle)
        {
            character.SetLineOfSight(tiles);
            character.PositionRectangle = new Rectangle((int)character.Position.X, (int)character.Position.Y, character.SpriteSize.X, character.SpriteSize.Y);


            Hitbox = new Rectangle((int)character.Position.X - (character.SpriteSize.X /2), (int)character.Position.Y - (character.SpriteSize.Y / 2),
              character.SpriteSize.X * 2, character.SpriteSize.Y * 2);
            if (character.LineOfSightRectangle.Contains(player.PositionRectangle))
            {
                MoveToPoint(player.Position, 100, player, ref allowedToWalk, ref battle);
            }
        }

        public void RandomizeDirection()
        {
            //Add a delay
            Array values = Enum.GetValues(typeof(Direction));
            Random random = new Random();
            Direction randomDir = (Direction)values.GetValue(random.Next(values.Length));
            if (randomDir == Direction.None) randomDir = Direction.Down;
            character.Direction = randomDir;
        }

        public void MoveToPoint(Vector2 move, float delay, Character player, ref bool allowedToWalk, ref Battle battle)
        {
            var distanceX = move.X - character.Position.X;
            var distanceY = move.Y - character.Position.Y;
            var moveX = distanceX / delay;
            var moveY = distanceY / delay;

            Hitbox = new Rectangle((int)character.Position.X - (character.SpriteSize.X / 2), (int)character.Position.Y - (character.SpriteSize.Y / 2),
               character.SpriteSize.X * 2, character.SpriteSize.Y * 2);
            if (!Hitbox.Contains(player.PositionRectangle))
            {
                allowedToWalk = false;
                character.Position.X += moveX;
                character.Position.Y += moveY;
            }
            else
            {
                //Hou dat lekkere gesprek
                battle = new Battle(player, character);
                allowedToWalk = true;
            }
        }
    }
}
