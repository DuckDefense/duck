using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace VideoGame.Classes {
    public static class Drawer {
        private static Button LastClicked;
        public static bool DrawMedicine, DrawCapture;
        public static Button bMedicine, bCapture;
        public static List<Button> MoveButtons, InventoryButtons, ItemButtons, MedicineButtons, CaptureButtons, PartyButtons;

        #region Battle
        public static void DrawMoves(SpriteBatch batch, Character player) {
            MoveButtons = new List<Button>();
            var buttonPos = 0;
            Rectangle rec = new Rectangle(buttonPos - ContentLoader.Button.Width, ContentLoader.GrassyBackground.Height, ContentLoader.Button.Width, ContentLoader.Button.Height);
            foreach (var move in player.Monsters[0].KnownMoves) {
                var b = new Button(new Rectangle(rec.X += ContentLoader.Button.Width, rec.Y + ContentLoader.Button.Height, rec.Width, rec.Height), ContentLoader.Button, $"{move.Name}", ContentLoader.Arial);
                b.Draw(batch);
                MoveButtons.Add(b);
            }
        }

        public static void DrawInventory(SpriteBatch batch, Character player) {
            InventoryButtons = new List<Button>();
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
            ItemButtons = new List<Button>();
            MedicineButtons = new List<Button>();
            CaptureButtons = new List<Button>();
            var buttonPos = 0;
            Rectangle rec = new Rectangle(buttonPos - ContentLoader.Button.Width, ContentLoader.GrassyBackground.Height, ContentLoader.Button.Width, ContentLoader.Button.Height);
            if (DrawMedicine) {
                foreach (var item in player.Inventory.Medicine) {
                    var amountPos = ContentLoader.Arial.MeasureString(item.Value.Name);
                    var b = new Button(new Rectangle(rec.X += ContentLoader.Button.Width, rec.Y + (ContentLoader.Button.Height * 2), rec.Width, rec.Height),
                            ContentLoader.Button) { Text = item.Value.Name };
                    b.Draw(batch);
                    MedicineButtons.Add(b);
                    batch.DrawString(ContentLoader.Arial, $"{item.Value.Amount}", new Vector2(b.Position.X + amountPos.X, b.Position.Y * 4),Color.White);
                    batch.Draw(item.Value.Sprite, new Vector2(b.Position.X + (item.Value.Sprite.Width / 2), b.Position.Y), Color.White);
                }
            }
            if (DrawCapture) {
                foreach (var item in player.Inventory.Captures) {
                    var b = new Button(new Rectangle(rec.X += ContentLoader.Button.Width, rec.Y + (ContentLoader.Button.Height * 2), rec.Width, rec.Height),
                        ContentLoader.Button) { Text = item.Value.Name };
                    b.Draw(batch);
                    CaptureButtons.Add(b);
                    batch.Draw(item.Value.Sprite, new Vector2(b.Position.X + (item.Value.Sprite.Width / 2), b.Position.Y), Color.White);
                }
            }
        }

        public static void DrawParty(SpriteBatch batch, Character player) {
            PartyButtons = new List<Button>();
            var party = player.Monsters;
            var buttonPos = 0;
            Rectangle rec = new Rectangle(buttonPos - ContentLoader.Button.Width, ContentLoader.GrassyBackground.Height, ContentLoader.Button.Width, ContentLoader.Button.Height);

            foreach (var monster in party) {
                var rect = new Rectangle(rec.X += ContentLoader.Button.Width, rec.Y + ContentLoader.Button.Height, rec.Width, rec.Height);
                var b = new Button(rect, ContentLoader.Button) {Text = monster.Name };
                b.Draw(batch);
                batch.Draw(monster.PartySprite, new Vector2(rect.X + monster.PartySpriteSize.X - 4, rect.Y + 4), monster.SourceRectangle, Color.White);
            }
        }

        public static void DrawHealth(SpriteBatch batch) {

        }

        public static void DrawMonsterInfo(SpriteBatch batch, Monster m) {
            Texture2D background = ContentLoader.Button;
            var frontPos = new Vector2();
            var namePos = new Vector2();
            var descriptionPos = new Vector2();

            batch.Draw(m.FrontSprite, frontPos, Color.White);
            batch.DrawString(ContentLoader.Arial, m.Name, namePos, Color.White);
            batch.DrawString(ContentLoader.Arial, m.Description, descriptionPos, Color.White);
            //Unfinished
        }

        public static void DrawBattle(SpriteBatch batch, Monster userMon, Monster oppoMon) {
            Rectangle userMonsterPos = new Rectangle(
                ContentLoader.GrassyBackground.Width - (ContentLoader.GrassyBackground.Width),
                ContentLoader.GrassyBackground.Height - userMon.BackSprite.Height,
                userMon.BackSprite.Width,
                userMon.BackSprite.Height);
            Rectangle opponentMonsterPos = new Rectangle(
                ContentLoader.GrassyBackground.Width - (int)(oppoMon.FrontSprite.Width * 2),
                ContentLoader.GrassyBackground.Height - (int)(oppoMon.FrontSprite.Height * 2),
                oppoMon.FrontSprite.Width,
                oppoMon.FrontSprite.Height);

            batch.Draw(userMon.BackSprite, new Vector2(userMonsterPos.X, userMonsterPos.Y));
            batch.Draw(oppoMon.FrontSprite, new Vector2(opponentMonsterPos.X, opponentMonsterPos.Y));
        }

        public static void UpdateBattleButtons(MouseState cur, MouseState prev) {
            if (ItemButtons != null)
                foreach (var btn in ItemButtons) {
                    btn.Update(cur, prev);
                    if (btn.IsClicked(cur, prev)) {
                        LastClicked = btn;
                    }
                }
            if (MedicineButtons != null)
                foreach (var btn in MedicineButtons) {
                    btn.Update(cur, prev);
                    if (btn.IsClicked(cur, prev)) {
                        LastClicked = btn;
                    }
                }
            if (CaptureButtons != null)
                foreach (var btn in CaptureButtons) {
                    btn.Update(cur, prev);
                    if (btn.IsClicked(cur, prev)) {
                        LastClicked = btn;
                    }
                }
            if (InventoryButtons != null)
                foreach (var btn in InventoryButtons) {
                    btn.Update(cur, prev);
                    if (btn.IsClicked(cur, prev)) {
                        LastClicked = btn;
                    }
                }
            if (MoveButtons != null)
                foreach (var btn in MoveButtons) {
                    btn.Update(cur, prev);
                    if (btn.IsClicked(cur, prev)) {
                        LastClicked = btn;
                    }
                }
            if (PartyButtons != null)
                foreach (var btn in PartyButtons) {
                    btn.Update(cur, prev);
                    if (btn.IsClicked(cur, prev)) {
                        LastClicked = btn;
                    }
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
            return LastClicked;
        }
    }
}
