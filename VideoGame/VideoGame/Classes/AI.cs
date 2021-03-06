﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sandbox.Classes.UI;

namespace VideoGame.Classes
{
    public class AI
    {
        private bool Chase;
        private Character character;
        public Rectangle Hitbox;
        private int tiles;
        public string Message;
        private bool shopAdded;

        public AI(Character character, int sightSize)
        {
            this.character = character;
            tiles = sightSize;
            RandomizeDirection();
        }

        public void Update(Character player, ref Battle battle, ref List<Shop> shops, MouseState cur, MouseState prev)
        {
            var playerRect = new Rectangle((int)player.Position.X, (int)player.Position.Y, 32, 32);
            character.SetLineOfSight(tiles);
            Hitbox = new Rectangle((int)character.Position.X - (character.SpriteSize.X / 2), (int)character.Position.Y - (character.SpriteSize.Y / 2),
              character.SpriteSize.X * 2, character.SpriteSize.Y * 2);
            if (character.NpcKind == NPCKind.Trainer)
            {
                if (character.LineOfSightRectangle.Intersects(playerRect))
                {
                    Chase = true;
                }
                if (Chase)
                    MoveToPoint(player.Position, 100, player, ref player.Controllable, ref battle, ref shops);
            }
            else
            {
                if (character.LineOfSightRectangle.Intersects(playerRect))
                {
                    GetMessages(player, ref battle, ref player.Controllable, ref shops);
                }
                else
                {
                    ResetMessages();
                }
            }
        }

        public void RandomizeDirection()
        {
            //Add a delay
            Array values = Enum.GetValues(typeof(Direction));
            Random random = new Random();
            Direction randomDir = (Direction)values.GetValue(random.Next(values.Length));
            if (randomDir == Direction.None) randomDir = Direction.Down;
            switch (randomDir)
            {
                case Direction.None: randomDir = Direction.Down; character.CurrentFrame.Y = 0; break;
                case Direction.Up: character.CurrentFrame.Y = 3; break;
                case Direction.Down: character.CurrentFrame.Y = 0; break;
                case Direction.Right: character.CurrentFrame.Y = 1; break;
                case Direction.Left: character.CurrentFrame.Y = 2; break;
            }
            character.Direction = randomDir;
        }

        public void MoveToPoint(Vector2 move, float delay, Character player, ref bool allowedToWalk, ref Battle battle, ref List<Shop> shops)
        {
            if (!character.Defeated)
            {
                var distanceX = move.X - character.Position.X;
                var distanceY = move.Y - character.Position.Y;
                var moveX = distanceX / delay;
                var moveY = distanceY / delay;

                Hitbox = new Rectangle((int)character.Position.X - (character.SpriteSize.X / 2),
                    (int)character.Position.Y - (character.SpriteSize.Y / 2),
                    character.SpriteSize.X * 2, character.SpriteSize.Y * 2);
                if (!Hitbox.Contains(player.Position))
                {
                    allowedToWalk = false;
                    if (character.Direction == Direction.Right || character.Direction == Direction.Left)
                        character.Position.X += moveX * 2;
                    if (character.Direction == Direction.Down || character.Direction == Direction.Up)
                        character.Position.Y += moveY * 2;
                }
                else
                {
                    GetMessages(player, ref battle, ref allowedToWalk, ref shops);
                }
            }
            else Chase = false;
        }

        public void GetMessages(Character player, ref Battle battle, ref bool allowedToWalk, ref List<Shop> shops)
        {
            switch (character.NpcKind)
            {
                case NPCKind.Trainer:
                    if (!character.BattleMessage.Said)
                    {
                        character.BattleMessage.Visible = true;
                        character.Talking = true;
                    }
                    if (!character.Talking)
                    {
                        battle = new Battle(player, character);
                        allowedToWalk = true;
                    }
                    break;
                case NPCKind.Healer:
                    if (!character.IntroMessage.Said)
                    {
                        character.IntroMessage.Visible = true;
                        character.Talking = true;
                    }
                    if (!character.Talking)
                    {
                        if (character.ByeMessage.Said == false)
                        {
                            HealMonsters(player);
                            character.ByeMessage.Visible = true;
                        }
                        else
                        {
                            character.Talking = false;
                        }
                    }
                    break;
                case NPCKind.Shop:
                    if (!character.IntroMessage.Said)
                    {
                        character.IntroMessage.Visible = true;
                        character.Talking = true;
                    }
                    if (!character.Talking)
                    {
                        if (character.ByeMessage.Said == false)
                        {
                            if (!shopAdded)
                            {
                                shops.Add( new Shop("Best shop around TM", character, player));
                                shopAdded = true;
                            }
                        }
                        else
                        {
                            character.Talking = false;
                        }
                    }
                    break;
            }
        }

        public void HealMonsters(Character player)
        {
            foreach (var m in player.Monsters)
            {
                m.Stats.Health = m.MaxHealth;
                m.Ailment = Ailment.Normal;
                foreach (var use in m.Moves)
                {
                    use.Uses = use.MaxUses;
                }
            }
        }

        private void ResetMessages()
        {
            if (character.BattleMessage != null)
            {
                character.BattleMessage.Said = false;
                character.WinMessage.Said = false;
                character.LoseMessage.Said = false;
            }
            if (character.IntroMessage != null)
            {
                character.IntroMessage.Said = false;
                character.ByeMessage.Said = false;
            }
        }
    }

    public static class BattleAI
    {
        private static Move strongestAttack = Move.Glare();

        public static void MakeDecision(Battle b, Move m, Monster user, Monster receiver, Character player)
        {
            Random random = new Random();

            switch (random.Next(1, 5))
            {
                case 1:
                    EnemyAttack(b, user, receiver);
                    break;
                case 2:
                    UseBuff(b, user, receiver);
                    break;
                case 3:
                    if (user.Ailment != Ailment.Normal)
                    {
                        if (player != null)
                        {
                            foreach (var value in player.Inventory.Medicine.Values)
                            {
                                if (Medicine.CuresAilment(user.Ailment))
                                {
                                    UseMedicine(b, player, user);
                                }
                            }
                        }
                    }
                    else
                    {
                        EnemyAttack(b, user, receiver);
                    }
                    break;
                case 4:
                    if (user.Stats.Health < user.MaxHealth / 100 * 20)
                    {
                        if (player != null)
                        {
                            if (player.Inventory.Medicine.Count != 0)
                            {
                                UseMedicine(b, player, user);
                            }
                            else
                            {
                                EnemyAttack(b, user, receiver);
                            }
                        }
                        else
                        {
                            EnemyAttack(b, user, receiver);
                        }
                    }
                    else
                    {
                        EnemyAttack(b, user, receiver);
                    }
                    break;
                case 5:
                    if (user.Ailment != Ailment.Normal)
                    {
                        foreach (var value in player.Inventory.Medicine.Values)
                        {
                            if (Medicine.CuresAilment(user.Ailment))
                            {
                                UseMedicine(b, player, user);
                            }
                        }
                    }

                    if (user.Stats.Health < user.MaxHealth / 100 * 20)
                    {
                        if (player != null)
                        {
                            if (player.Inventory.Medicine.Count != 0)
                            {
                                UseMedicine(b, player, user);
                            }
                            else
                            {
                                if (m.Damage != null)
                                {
                                    if (m.Damage > user.Stats.Health)
                                    {
                                        EnemyAttack(b, user, receiver);
                                    }
                                    else
                                    {
                                        UseBuff(b, user, receiver);
                                    }
                                }
                                else
                                {
                                    UseBuff(b, user, receiver);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (m != null)
                        {
                            if (m.Damage != 0)
                            {
                                if (m.Damage > user.Stats.Health)
                                {
                                    EnemyAttack(b, user, receiver);
                                }
                                else
                                {
                                    UseBuff(b, user, receiver);
                                }
                            }
                            else
                            {
                                UseBuff(b, user, receiver);
                            }
                        }
                        else
                        {
                            UseBuff(b, user, receiver);
                        }
                    }
                    break;
            }


        }
        public static void EnemyAttack(Battle b, Monster user, Monster receiver)
        {
            strongestAttack = Move.Glare();
            //user.GetMoves();
            foreach (var m in user.Moves.Where(x => x.Uses != 0))
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

        public static void UseBuff(Battle b, Monster user, Monster receiver)
        {
            List<Move> moveList = new List<Move>();
            foreach (var move in user.Moves)
            {
                if (move.Kind == Kind.NonDamage)
                {
                    moveList.Add(move);
                }
            }
            if (moveList.Count != 0)
            {
                Random r = new Random();

                b.Attack(user, receiver, moveList[r.Next(0, moveList.Count)]);
            }
        }

        public static void UseMedicine(Battle b, Character player, Monster user)
        {
            List<Medicine> medicineList = new List<Medicine>();

            foreach (var medicine in player.Inventory.Medicine.Values)
            {
                if (medicine.HealAmount != 0)
                {
                    medicineList.Add(medicine);
                }

            }
            if (medicineList.Count != 0)
            {
                Random r = new Random();
                var medicine = medicineList[r.Next(0, medicineList.Count)];
                Drawer.AddMessage(new List<string> { $"{medicine.Name} used on {user.Name}." });
                medicine.Use(user, player);
            }
        }
    }
}
