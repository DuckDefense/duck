using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VideoGame.Classes {
    public class AI {
        private Character character;
        public Rectangle Hitbox;
        private int tiles;
        public string Message;

        public AI(Character character, int sightSize, string message) {
            Message = message;
            this.character = character;
            tiles = sightSize;
            RandomizeDirection();
        }

        public void Update(GameTime gameTime, Character player, ref bool allowedToWalk, ref Battle battle) {
            character.SetLineOfSight(tiles);
            Hitbox = new Rectangle((int)character.Position.X - (character.SpriteSize.X / 2), (int)character.Position.Y - (character.SpriteSize.Y / 2),
              character.SpriteSize.X * 2, character.SpriteSize.Y * 2);
            if (character.LineOfSightRectangle.Contains(player.Position)) {
                MoveToPoint(player.Position, 100, player, ref player.Controllable, ref battle);
            }
        }

        public void RandomizeDirection() {
            //Add a delay
            Array values = Enum.GetValues(typeof(Direction));
            Random random = new Random();
            Direction randomDir = (Direction)values.GetValue(random.Next(values.Length));
            if (randomDir == Direction.None) randomDir = Direction.Down;
            switch (randomDir) {
            case Direction.None: randomDir = Direction.Down; character.CurrentFrame.Y = 0; break;
            case Direction.Up: character.CurrentFrame.Y = 3; break;
            case Direction.Down: character.CurrentFrame.Y = 0; break;
            case Direction.Right: character.CurrentFrame.Y = 1; break;
            case Direction.Left: character.CurrentFrame.Y = 2; break;
            }
            character.Direction = randomDir;
        }

        public void MoveToPoint(Vector2 move, float delay, Character player, ref bool allowedToWalk, ref Battle battle) {
            if (!character.Defeated) {
                var distanceX = move.X - character.Position.X;
                var distanceY = move.Y - character.Position.Y;
                var moveX = distanceX/delay;
                var moveY = distanceY/delay;

                Hitbox = new Rectangle((int)character.Position.X - (character.SpriteSize.X / 2), (int)character.Position.Y - (character.SpriteSize.Y / 2),
             character.SpriteSize.X * 2, character.SpriteSize.Y * 2);
                if (!Hitbox.Contains(player.Position)) {
                    allowedToWalk = false;
                    character.Position.X += moveX * 2;
                    character.Position.Y += moveY * 2;
                }
                else {
                    //Hou dat lekkere gesprek
                    battle = new Battle(player, character);
                    allowedToWalk = true;
                }
            }
        }
    }

    public static class BattleAI
    {
        private static Move strongestAttack = Move.Glare();
        public static void EnemyAttack(Battle b, Monster user, Monster receiver)
        {
            strongestAttack = Move.Glare();
            //user.GetMoves();
            foreach (var m in user.KnownMoves)
            {
                m.Damage = m.GetDamage(user.Stats.SpecialAttack, receiver.Stats.SpecialDefense,
                           m.GetDamageModifier(receiver), 1);
                if (m.Damage > strongestAttack.Damage)
                {
                    strongestAttack = m;
                }
            }
            b.Attack(user, receiver, strongestAttack);
        }
    }
}
