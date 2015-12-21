using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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

        private static Rectangle userpos, opponentpos;

        #region Battle

        public static void Initialize(GraphicsDeviceManager graphics) {
            Batch = new SpriteBatch(graphics.GraphicsDevice);
        }

        public static void DrawMoves(SpriteBatch batch, Character player) {
            ClearLists();
            var buttonPos = 0;
            player.Monsters[0].GetMoves();
            Rectangle rec = new Rectangle(buttonPos - ContentLoader.Button.Width, ContentLoader.GrassyBackground.Height, ContentLoader.Button.Width, ContentLoader.Button.Height);
            foreach (var move in player.Monsters[0].Moves) {
                var b = new Button(new Rectangle(rec.X += ContentLoader.Button.Width, rec.Y + ContentLoader.Button.Height, rec.Width, rec.Height), ContentLoader.Button, $"{move.Name}", ContentLoader.Arial);
                b.Draw(batch);
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
                foreach (var item in player.Inventory.Medicine) {
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
                foreach (var item in player.Inventory.Captures) {
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

        public static void DrawHealth(SpriteBatch batch, Monster user, Monster opponent) {
            var userHealthPerc = (user.Stats.Health * 100) / user.MaxHealth;
            var currentUserHealthSize = new Vector2(userHealthPerc, 5);
            var userHealthRec = new Rectangle(userpos.X + 32, userpos.Y, (int)currentUserHealthSize.X, (int)currentUserHealthSize.Y);

            var opponentHealthPerc = (opponent.Stats.Health * 100) / opponent.MaxHealth;
            var currentOpponentHealthSize = new Vector2(opponentHealthPerc, 5);
            var opponentHealthRec = new Rectangle(opponentpos.X - 32, opponentpos.Y + 32, (int)currentOpponentHealthSize.X, (int)currentOpponentHealthSize.Y);

            batch.DrawString(ContentLoader.Arial, user.Name, new Vector2(userpos.X + 32, userpos.Y + 32), Color.Black);
            batch.DrawString(ContentLoader.Arial, $"{user.Stats.Health}/{user.MaxHealth}", new Vector2(userpos.X + 32, userpos.Y - 32), Color.Red);
            batch.DrawString(ContentLoader.Arial, $"{opponent.Stats.Health}/{opponent.MaxHealth}", new Vector2(opponentpos.X, opponentpos.Y), Color.Blue);
            batch.DrawString(ContentLoader.Arial, $"{userHealthPerc}", new Vector2(userpos.X, userpos.Y), Color.Orange);
            batch.DrawString(ContentLoader.Arial, $"{opponentHealthPerc}", new Vector2(opponentHealthRec.X, opponentHealthRec.Y), Color.Orange);
            batch.Draw(ContentLoader.Button, userHealthRec, Color.Red);
            batch.Draw(ContentLoader.Button, opponentHealthRec, Color.Red);
        }

        public static void DrawLevel(SpriteBatch batch, Monster user, Monster opponent) {
            var userLevelPos = new Vector2(opponentpos.X + 32, opponentpos.Y - 96);
            var opponentLevelPos = new Vector2(userpos.X + 32, userpos.Y + 64);

            batch.DrawString(ContentLoader.Arial, user.Level.ToString(), userLevelPos, Color.White);
            batch.DrawString(ContentLoader.Arial, opponent.Level.ToString(), opponentLevelPos, Color.White);
        }

        public static void DrawAilment() {

        }

        public static void DrawMessage(string message) {
            Rectangle rec = new Rectangle(0 - ContentLoader.Button.Width, ContentLoader.GrassyBackground.Height, ContentLoader.Button.Width, ContentLoader.Button.Height);
            Batch.Begin();
            var b = new Button(rec, message, ContentLoader.Arial);
            //Batch.DrawString(ContentLoader.Arial, message, new Vector2(500, 500), Color.White);
            b.Draw(Batch);
            Batch.End();
        }

        public static void DrawMonsterInfo(SpriteBatch batch, Monster m) {
            Texture2D background = ContentLoader.MonsterViewer;
            var nameSize = ContentLoader.Arial.MeasureString(m.Name);
            var descriptionSize = ContentLoader.Arial.MeasureString(m.Name);
            Rectangle backgroundRectangle = new Rectangle(8, 8, background.Width, background.Height);
            var frontPos = new Vector2(backgroundRectangle.X - 22, backgroundRectangle.Y - 8);
            var namePos = new Vector2(backgroundRectangle.X - nameSize.X, backgroundRectangle.Y);
            var descriptionPos = new Vector2(backgroundRectangle.X - descriptionSize.X, backgroundRectangle.Y - descriptionSize.Y);
            var statsPos = new Vector2();
            var abilityNamePos = new Vector2();
            var abilityDescriptionPos = new Vector2();

            batch.Draw(m.FrontSprite, frontPos, Color.White);
            batch.DrawString(ContentLoader.Arial, m.Name, namePos, Color.White);
            batch.DrawString(ContentLoader.Arial, m.Description, descriptionPos, Color.White);
            //Unfinished
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
            DrawLevel(batch, user, opponent);
            DrawHealth(batch, user, opponent);
        }

        public static void ClearLists() {
            InventoryButtons = new List<Button>();
            MoveButtons = new List<ContainerButton>();
            ItemButtons = new List<ContainerButton>();
            MedicineButtons = new List<ContainerButton>();
            CaptureButtons = new List<ContainerButton>();
            PartyButtons = new List<ContainerButton>();
        }

        //public static void ClearButtons() {
        //    LastClickedContainer = null;
        //    LastClickedButton = null;
        //}

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
        #endregion

        public static Button GetClickedButton() {
            return LastClickedButton;
        }
        public static ContainerButton GetClickedContainerButton() {
            return LastClickedContainer;
        }
    }
}
