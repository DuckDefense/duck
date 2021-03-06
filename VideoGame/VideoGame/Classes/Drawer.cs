﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sandbox.Classes;
using Sandbox.Classes.UI;

namespace VideoGame.Classes {
    public static class Drawer {
        public static SpriteBatch Batch;
        private static Button LastClickedButton;
        private static ContainerButton LastClickedContainer;
        public static bool DrawMedicine, DrawCapture;
        public static Button bMedicine, bCapture;
        public static List<ContainerButton> MoveButtons, ItemButtons, MedicineButtons, CaptureButtons, PartyButtons;
        public static List<Button> InventoryButtons;
        public static List<Conversation.BattleMessage> BattleMessages = new List<Conversation.BattleMessage>();

        private static Rectangle userpos, opponentpos;

        #region Battle

        public static Texture2D GetTextureFromMoveKind(Move m) {
            switch (m.Kind) {
            case Kind.Physical: return ContentLoader.PhysicalAttack;
            case Kind.Special: return ContentLoader.SpecialAttack;
            case Kind.NonDamage: return ContentLoader.NonDamageAttack;
            }
            return null;
        }

        public static Texture2D GetAilmentTexture(Ailment ailment) {
            switch (ailment) {
            case Ailment.Sleep: return ContentLoader.SLP;
            case Ailment.Poisoned: return ContentLoader.PSN;
            case Ailment.Burned: return ContentLoader.BRN;
            case Ailment.Dazzled: return ContentLoader.DZL;
            case Ailment.Frozen: return ContentLoader.FRZ;
            case Ailment.Frenzied: return ContentLoader.FZD;
            case Ailment.Fainted: return ContentLoader.FNT;
            }
            return null;
        }

        public static Texture2D GetTypeTexture(Type type) {
            switch (type) {
            case Type.Normal: return ContentLoader.NormalType;
            case Type.Fight: return ContentLoader.FightType;
            case Type.Fire: return ContentLoader.FireType;
            case Type.Water: return ContentLoader.WaterType;
            case Type.Grass: return ContentLoader.GrassType;
            case Type.Rock: return ContentLoader.RockType;
            case Type.Poison: return ContentLoader.PoisonType;
            case Type.Psych: return ContentLoader.PsychType;
            case Type.Flying: return ContentLoader.FlyingType;
            case Type.Ice: return ContentLoader.IceType;
            case Type.Ghost: return ContentLoader.GhostType;
            case Type.Sound: return ContentLoader.SoundType;
            }
            return ContentLoader.NormalType;
        }

        public static void DrawMoves(SpriteBatch batch, Character player) {
            ClearLists();
            var buttonPos = 0;
            float widthMultiplier = 1.5f;
            //Position.X + ((Position.Width - stringsize.X) / 2)
            Rectangle rec = new Rectangle(buttonPos - Convert.ToInt32(ContentLoader.Button.Width * widthMultiplier), ContentLoader.GrassyBackground.Height, ContentLoader.Button.Width, ContentLoader.Button.Height);
            foreach (var move in player.Monsters[0].Moves) {
                var stringsize = ContentLoader.Arial.MeasureString($"{move.Uses}/{move.MaxUses}");
                var b = new Button(new Rectangle(rec.X += Convert.ToInt32(ContentLoader.Button.Width * widthMultiplier), rec.Y + ContentLoader.Button.Height, Convert.ToInt32(rec.Width * widthMultiplier), rec.Height), ContentLoader.Button, $"{move.Name}", ContentLoader.Arial);
                var typeRec = new Rectangle(rec.X + ((b.Position.Width - ContentLoader.NormalType.Width) / 2), rec.Y + ContentLoader.Button.Height + (ContentLoader.NormalType.Height * 2), ContentLoader.NormalType.Width, ContentLoader.NormalType.Height);
                var usesRec = new Rectangle(rec.X + Convert.ToInt32((b.Position.Width - stringsize.X) / 2), rec.Y + (ContentLoader.Button.Height + (ContentLoader.NormalType.Height * 3)), ContentLoader.NormalType.Width, ContentLoader.NormalType.Height);
                b.Draw(batch);
                batch.Draw(GetTypeTexture(move.Type), typeRec, Color.White);
                batch.DrawString(ContentLoader.Arial, $"{move.Uses}/{move.MaxUses}", new Vector2(usesRec.X, usesRec.Y), Color.White);
                MoveButtons.Add(new ContainerButton(b, move));
            }
        }

        public static void DrawInventory(SpriteBatch batch, Character player) {
            ClearLists();
            var buttonPos = 0;
            Rectangle rec = new Rectangle(buttonPos - ContentLoader.Button.Width, ContentLoader.GrassyBackground.Height, ContentLoader.Button.Width, ContentLoader.Button.Height);
            bMedicine = new Button(new Rectangle(rec.X += ContentLoader.Button.Width, rec.Y + ContentLoader.Button.Height, rec.Width, rec.Height),
                ContentLoader.Button, "Medicine", ContentLoader.Arial);
            bCapture = new Button(new Rectangle(rec.X += ContentLoader.Button.Width, rec.Y + ContentLoader.Button.Height, rec.Width, rec.Height),
                ContentLoader.Button, "Capture", ContentLoader.Arial);

            bMedicine.Draw(batch);
            bCapture.Draw(batch);
            InventoryButtons.AddMany(bMedicine, bCapture);
            DrawItems(batch, player);
        }

        public static void DrawItems(SpriteBatch batch, Character player) {
            ClearLists();
            var buttonPos = 0;
            Rectangle rec = new Rectangle(buttonPos - ContentLoader.Button.Width, ContentLoader.GrassyBackground.Height, ContentLoader.Button.Width, ContentLoader.Button.Height);
            if (DrawMedicine) {
                foreach (var item in player.Inventory.Medicine.Where(x => x.Value.Amount > 0)) {
                    var b = new Button(new Rectangle(rec.X += ContentLoader.Button.Width, rec.Y + (ContentLoader.Button.Height * 2), rec.Width, rec.Height),
                            ContentLoader.Button);
                    b.Draw(batch);
                    MedicineButtons.Add(new ContainerButton(b, item.Value));
                    batch.Draw(item.Value.Sprite, new Vector2(b.Position.X + (item.Value.Sprite.Width / 2), b.Position.Y), Color.White);
                    batch.DrawString(ContentLoader.Arial, $"{item.Value.Amount}x",
                        new Vector2(b.Position.X, b.Position.Y + ContentLoader.Button.Height), Color.White);
                }
            }
            if (DrawCapture) {
                foreach (var item in player.Inventory.Captures.Where(x => x.Value.Amount > 0)) {
                    var b = new Button(new Rectangle(rec.X += ContentLoader.Button.Width, rec.Y + (ContentLoader.Button.Height * 2), rec.Width, rec.Height),
                        ContentLoader.Button) { Text = item.Value.Name };
                    b.Draw(batch);
                    CaptureButtons.Add(new ContainerButton(b, item.Value));
                    batch.Draw(item.Value.Sprite, new Vector2(b.Position.X + (item.Value.Sprite.Width / 2), b.Position.Y), Color.White);
                    batch.DrawString(ContentLoader.Arial, $"{item.Value.Amount}x",
                        new Vector2(b.Position.X, b.Position.Y + ContentLoader.Button.Height), Color.White);
                }
            }
        }

        public static void DrawParty(SpriteBatch batch, Character player) {
            ClearLists();
            var party = player.Monsters;
            var buttonPos = 0;
            Rectangle rec = new Rectangle(buttonPos - ContentLoader.Button.Width, ContentLoader.GrassyBackground.Height, ContentLoader.Button.Width, ContentLoader.Button.Height);

            foreach (var monster in party) {
                if (monster.UId != party[0].UId) {
                    if (!monster.IsDead) {
                        var rect = new Rectangle(rec.X += ContentLoader.Button.Width,
                            rec.Y + ContentLoader.Button.Height, rec.Width, rec.Height);
                        var b = new Button(rect, ContentLoader.Button);
                        b.Draw(batch);
                        batch.Draw(monster.PartySprite, new Vector2(rect.X + monster.PartySpriteSize.X - 4, rect.Y + 4),
                            monster.SourceRectangle, Color.White);
                        PartyButtons.Add(new ContainerButton(b, monster));
                    }
                }
            }
        }

        public static void DrawStatus(SpriteBatch batch, Monster user, Monster opponent) {
            var font = ContentLoader.Arial;
            float prevFullExp = user.Level * user.Level * 5;
            float fullExp = ((user.Level + 1) * (user.Level + 1) * 5);
            float difference = fullExp - prevFullExp;
            float experiencePerc = 100 - ((user.RemainingExp / difference) * 100);

            var userHealthPerc = (user.Stats.Health * 100) / user.MaxHealth;
            var userExpSize = new Vector2(experiencePerc, 2);
            var userHealthSize = new Vector2(userHealthPerc, 10);
            var userHealthRec = new Rectangle(userpos.X + 48, userpos.Y - 20, (int)userHealthSize.X, (int)userHealthSize.Y);
            var userExpRec = new Rectangle(userHealthRec.X, userHealthRec.Y + 31, (int)userExpSize.X, (int)userExpSize.Y);

            var opponentHealthPerc = (opponent.Stats.Health * 100) / opponent.MaxHealth;
            var currentOpponentHealthSize = new Vector2(opponentHealthPerc, 10);
            var opponentHealthRec = new Rectangle(opponentpos.X, opponentpos.Y - 12, (int)currentOpponentHealthSize.X, (int)currentOpponentHealthSize.Y);

            var userCol = Color.SpringGreen;
            var opponentCol = Color.SpringGreen;
            if (userHealthPerc <= 50) userCol = Color.Orange; if (userHealthPerc <= 20) userCol = Color.Red;
            if (opponentHealthPerc <= 50) opponentCol = Color.Orange; if (opponentHealthPerc <= 20) opponentCol = Color.Red;

            batch.Draw(ContentLoader.HealthBorderUser, new Vector2(userHealthRec.X - 33, userHealthRec.Y - 4), Color.Gray);
            batch.DrawString(ContentLoader.Arial, user.Name, new Vector2(userHealthRec.X - 31, userHealthRec.Y - 22), Color.Black);
            batch.DrawString(ContentLoader.Arial, $"Lv. {user.Level}", new Vector2(userHealthRec.X + (font.MeasureString($"{user.Name} Lv. {user.Level}").X) - 48, userHealthRec.Y - 22), Color.Black);
            if (user.Stats.Health < 0) user.Stats.Health = 0;
            batch.DrawString(ContentLoader.Arial, $"{user.Stats.Health}/{user.MaxHealth}", new Vector2(userHealthRec.X - 12, userHealthRec.Y + 12), Color.Black);
            if (user.Ailment != Ailment.Normal)
                batch.Draw(GetAilmentTexture(user.Ailment), new Vector2(userHealthRec.X - 31, userHealthRec.Y - 2), Color.White);
            batch.Draw(ContentLoader.Health, userHealthRec, userCol);
            batch.Draw(ContentLoader.Health, userExpRec, Color.DeepSkyBlue);

            batch.Draw(ContentLoader.HealthBorder, new Vector2(opponentHealthRec.X - 33, opponentHealthRec.Y - 4), Color.Gray);
            batch.DrawString(ContentLoader.Arial, opponent.Name, new Vector2(opponentpos.X - 31, opponentpos.Y - 33), Color.Black);
            batch.DrawString(ContentLoader.Arial, $"Lv. {opponent.Level}", new Vector2(opponentpos.X + (font.MeasureString($"{opponent.Name} Lv. {opponent.Level}").X) - 48, opponentpos.Y - 33), Color.Black);
            //batch.DrawString(ContentLoader.Arial, $"{opponent.Stats.Health}/{opponent.MaxHealth}", new Vector2(opponentpos.X, opponentpos.Y), Color.Blue);
            if (opponent.Ailment != Ailment.Normal)
                batch.Draw(GetAilmentTexture(opponent.Ailment), new Vector2(opponentHealthRec.X - 31, opponentHealthRec.Y - 2), Color.White);
            batch.Draw(ContentLoader.Health, opponentHealthRec, opponentCol);
        }

        public static void DrawMessage(SpriteBatch batch) {
            if (BattleMessages != null && BattleMessages.Count != 0)
                foreach (var mes in BattleMessages) {
                    mes.Draw(batch);
                }
        }

        public static void AddMessage(List<string> lines) {
            //BattleMessages = new List<Conversation.BattleMessage>();
            for (int i = 0; i < lines.Count; i++) if (string.IsNullOrEmpty(lines[i])) lines.RemoveAt(i);
            if (BattleMessages.Count != 0 && !BattleMessages[0].Said) {
                foreach (var line in lines) {
                    BattleMessages[0].Lines.Add(line);
                }
            }
            else {
                BattleMessages = new List<Conversation.BattleMessage>();
                var mes = new Conversation.BattleMessage(lines, Color.Black);
                BattleMessages.Add(mes);
            }
        }

        public static string SplitString(string s, Rectangle size) {
            var sSize = ContentLoader.Arial.MeasureString(s);
            if (sSize.X > size.Width) {
                if (s.Contains(",")) {
                    var index = s.LastIndexOf(",");
                    var splitS = s.Split(Convert.ToChar(","));
                    var splitSize = ContentLoader.Arial.MeasureString(splitS.ToString());
                    if (splitSize.X < size.Width) {
                        return s.Insert(index + 2, "\n");
                    }
                }
                else {
                    var difference = sSize.X % size.Width;
                    //Find a way to split the string so its size is low enough
                }
            }
            return s;
        }
        public static void DrawMonsterInfo(SpriteBatch batch, Monster m, Vector2 startPos = default(Vector2), bool knownMonsters = false) {
            Texture2D background = ContentLoader.MonsterViewer;
            var nameSize = ContentLoader.Arial.MeasureString(m.Ability.Name);
            Rectangle backgroundRectangle = new Rectangle((int)startPos.X, (int)startPos.Y, background.Width, background.Height);
            var frontPos = new Vector2(backgroundRectangle.X + 10, backgroundRectangle.Y + 24);
            var namePos = new Vector2(frontPos.X, backgroundRectangle.Y + 6);
            var descriptionPos = new Vector2(frontPos.X + 110, backgroundRectangle.Y + 28);
            var statsPos = new Vector2(frontPos.X, backgroundRectangle.Y + 130);
            var abilityNamePos = new Vector2(backgroundRectangle.X + 230, statsPos.Y);
            var abilityDescriptionPos = new Vector2(abilityNamePos.X, abilityNamePos.Y + 17);

            batch.Draw(background, backgroundRectangle, Color.White);
            batch.Draw(m.FrontSprite, frontPos, Color.White);
            batch.DrawString(ContentLoader.Arial, SplitString(m.Description, new Rectangle(0, 0, 112, 24)), descriptionPos, Color.Black);
            if (knownMonsters) {
                batch.DrawString(ContentLoader.Arial, m.Name, namePos, Color.Black);
                batch.DrawString(ContentLoader.Arial, m.Stats.PintBaseStats(), statsPos, Color.Black);
                if (m.PossibleAbilities.Count == 1) {
                    batch.DrawString(ContentLoader.Arial, $"{m.Ability.Name}", abilityNamePos, Color.Black);
                    batch.DrawString(ContentLoader.Arial, m.Ability.Description, abilityDescriptionPos, Color.Black);
                }
                else {
                    batch.DrawString(ContentLoader.Arial, $"{ m.PossibleAbilities[0].Name}", abilityNamePos, Color.Black);
                    batch.DrawString(ContentLoader.Arial, $"{ m.PossibleAbilities[0].Description}", abilityDescriptionPos, Color.Black);

                    var pos = new Vector2(abilityNamePos.X, abilityNamePos.Y + nameSize.Y + descriptionPos.Y);

                    batch.DrawString(ContentLoader.Arial, $"{ m.PossibleAbilities[1].Name}", pos, Color.Black);
                    batch.DrawString(ContentLoader.Arial, $"{ m.PossibleAbilities[1].Description}", new Vector2(pos.X, pos.Y + 17), Color.Black);
                }
            }
            else {
                batch.DrawString(ContentLoader.Arial, $"{m.Name} - Lv. {m.Level}", namePos, Color.Black);
                batch.DrawString(ContentLoader.Arial, m.Stats.PrintStats(m.MaxHealth), statsPos, Color.Black);
                batch.DrawString(ContentLoader.Arial, $"{m.Ability.Name}", abilityNamePos, Color.Black);
                batch.DrawString(ContentLoader.Arial, m.Ability.Description, abilityDescriptionPos, Color.Black);
            }
        }

        public static void DrawBattle(SpriteBatch batch, Monster user, Monster opponent) {
            Rectangle userMonsterPos = new Rectangle(
                ContentLoader.GrassyBackground.Width - (ContentLoader.GrassyBackground.Width),
                ContentLoader.GrassyBackground.Height - user.BackSprite.Height,
                user.BackSprite.Width,
                user.BackSprite.Height);
            Rectangle opponentMonsterPos = new Rectangle(
                ContentLoader.GrassyBackground.Width - (int)(opponent.FrontSprite.Width * 2),
                ContentLoader.GrassyBackground.Height - (int)(opponent.FrontSprite.Height * 2),
                opponent.FrontSprite.Width,
                opponent.FrontSprite.Height);

            userpos = userMonsterPos;
            opponentpos = opponentMonsterPos;

            batch.Draw(user.BackSprite, new Vector2(userMonsterPos.X, userMonsterPos.Y));
            batch.Draw(opponent.FrontSprite, new Vector2(opponentMonsterPos.X, opponentMonsterPos.Y));
            DrawStatus(batch, user, opponent);
        }

        public static void ClearLists() {
            InventoryButtons = new List<Button>();
            MoveButtons = new List<ContainerButton>();
            ItemButtons = new List<ContainerButton>();
            MedicineButtons = new List<ContainerButton>();
            CaptureButtons = new List<ContainerButton>();
            PartyButtons = new List<ContainerButton>();
        }

        public static void UpdateItemButtons(MouseState cur, MouseState prev) {
            if (bMedicine != null)
                if (bMedicine.IsClicked(cur, prev)) {
                    DrawMedicine = true;
                    DrawCapture = false;
                }
            if (bCapture != null)
                if (bCapture.IsClicked(cur, prev)) {
                    DrawCapture = true;
                    DrawMedicine = false;
                }
            if (MedicineButtons != null)
                foreach (var btn in MedicineButtons) {
                    btn.Update(cur, prev);
                    if (btn.Button.IsClicked(cur, prev)) LastClickedContainer = btn;
                }
            if (CaptureButtons != null)
                foreach (var btn in CaptureButtons) {
                    btn.Update(cur, prev);
                    if (btn.Button.IsClicked(cur, prev)) LastClickedContainer = btn;
                }
            if (InventoryButtons != null)
                foreach (var btn in InventoryButtons) {
                    btn.Update(cur, prev);
                    if (btn.IsClicked(cur, prev)) LastClickedButton = btn;
                }
        }

        public static void UpdateBattleButtons(MouseState cur, MouseState prev) {
            if (ItemButtons != null)
                foreach (var btn in ItemButtons) {
                    btn.Update(cur, prev);
                    if (btn.Button.IsClicked(cur, prev)) LastClickedContainer = btn;
                }
            if (MedicineButtons != null)
                foreach (var btn in MedicineButtons) {
                    btn.Update(cur, prev);
                    if (btn.Button.IsClicked(cur, prev)) LastClickedContainer = btn;
                }
            if (CaptureButtons != null)
                foreach (var btn in CaptureButtons) {
                    btn.Update(cur, prev);
                    if (btn.Button.IsClicked(cur, prev)) LastClickedContainer = btn;
                }
            if (InventoryButtons != null)
                foreach (var btn in InventoryButtons) {
                    btn.Update(cur, prev);
                    if (btn.IsClicked(cur, prev)) LastClickedButton = btn;
                }
            if (MoveButtons != null)
                foreach (var btn in MoveButtons) {
                    btn.Update(cur, prev);
                    if (btn.Button.IsClicked(cur, prev)) LastClickedContainer = btn;
                }
            if (PartyButtons != null)
                foreach (var btn in PartyButtons) {
                    btn.Update(cur, prev);
                    if (btn.Button.IsClicked(cur, prev)) LastClickedContainer = btn;
                }

            if (bMedicine != null)
                if (bMedicine.IsClicked(cur, prev)) {
                    DrawMedicine = true;
                    DrawCapture = false;
                }
            if (bCapture != null)
                if (bCapture.IsClicked(cur, prev)) {
                    DrawCapture = true;
                    DrawMedicine = false;
                }
        }

        public static void UpdateMessage(KeyboardState cur, KeyboardState prev, Character player) {
            if (BattleMessages != null && BattleMessages.Count != 0)
                foreach (var message in BattleMessages) {
                    message.Update(cur, prev, player);
                }
        }
        #endregion

        public static Button GetClickedButton() {
            return LastClickedButton;
        }
        public static ContainerButton GetClickedContainerButton() {
            return LastClickedContainer;
        }
    }
}
