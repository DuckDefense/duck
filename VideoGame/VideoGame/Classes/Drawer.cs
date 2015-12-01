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
        private static bool drawItems, drawMedicine, drawCapture;
        public static Button bItems, bMedicine, bCapture;
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

            bItems = new Button(new Rectangle(rec.X += ContentLoader.Button.Width, rec.Y + ContentLoader.Button.Height, rec.Width, rec.Height),
                ContentLoader.Button, "Items", ContentLoader.Arial);
            bMedicine = new Button(new Rectangle(rec.X += ContentLoader.Button.Width, rec.Y + ContentLoader.Button.Height, rec.Width, rec.Height),
                ContentLoader.Button, "Medicine", ContentLoader.Arial);
            bCapture = new Button(new Rectangle(rec.X += ContentLoader.Button.Width, rec.Y + ContentLoader.Button.Height, rec.Width, rec.Height),
                ContentLoader.Button, "Capture", ContentLoader.Arial);

            bItems.Draw(batch);
            bMedicine.Draw(batch);
            bCapture.Draw(batch);
            InventoryButtons.AddMany(bItems, bMedicine, bCapture);
            DrawItems(batch, player);
        }

        public static void DrawItems(SpriteBatch batch, Character player) {
            ItemButtons = new List<Button>();
            var buttonPos = 0;
            Rectangle rec = new Rectangle(buttonPos - ContentLoader.Button.Width, ContentLoader.GrassyBackground.Height, ContentLoader.Button.Width, ContentLoader.Button.Height);

            if (drawItems) {
                foreach (var item in player.Inventory.Items) {
                    var b = new Button(new Rectangle(rec.X += ContentLoader.Button.Width, rec.Y + ContentLoader.Button.Height, rec.Width, rec.Height),
                        ContentLoader.Button, $"{item.Value.Name}", ContentLoader.Arial);
                    b.Draw(batch);
                    ItemButtons.Add(b);
                }
            }
            if (drawMedicine) {
                foreach (var item in player.Inventory.Medicine) {
                    var b = new Button(new Rectangle(rec.X += ContentLoader.Button.Width, rec.Y + ContentLoader.Button.Height,
                                rec.Width, rec.Height),
                            ContentLoader.Button, $"{item.Value.Name}", ContentLoader.Arial);
                    b.Draw(batch);
                    MedicineButtons.Add(b);
                }
            }
            if (drawCapture) {
                foreach (var item in player.Inventory.Captures) {
                    var b = new Button(new Rectangle(rec.X += ContentLoader.Button.Width, rec.Y + ContentLoader.Button.Height, rec.Width, rec.Height),
                        ContentLoader.Button, $"{item.Value.Name}", ContentLoader.Arial);
                    b.Draw(batch);
                    CaptureButtons.Add(b);
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
                var b = new Button(rect, ContentLoader.Button, monster.Name, ContentLoader.Arial);
                b.Draw(batch);
                batch.Draw(monster.PartySprite, new Vector2(rect.X + monster.PartySpriteSize.X - 4, rect.Y + 4), monster.SourceRectangle, Color.White);
            }
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
                    if (btn.IsClicked(cur, prev)) LastClicked = btn;
                }
            if (MedicineButtons != null)
                foreach (var btn in MedicineButtons) {
                    btn.Update(cur, prev);
                    if (btn.IsClicked(cur, prev)) LastClicked = btn;
                }
            if (CaptureButtons != null)
                foreach (var btn in CaptureButtons) {
                    btn.Update(cur, prev);
                    if (btn.IsClicked(cur, prev)) LastClicked = btn;
                }
            if (InventoryButtons != null)
                foreach (var btn in InventoryButtons) {
                    btn.Update(cur, prev);
                    if (btn.IsClicked(cur, prev)) LastClicked = btn;
                }
            if (MoveButtons != null)
                foreach (var btn in MoveButtons) {
                    btn.Update(cur, prev);
                    if (btn.IsClicked(cur, prev)) LastClicked = btn;
                }
            if (PartyButtons != null)
                foreach (var btn in PartyButtons) {
                    btn.Update(cur, prev);
                    if (btn.IsClicked(cur, prev)) LastClicked = btn;
                }

            if (bItems != null) if (bItems.IsClicked(cur, prev)) drawItems = true;
            if (bMedicine != null) if (bMedicine.IsClicked(cur, prev)) drawMedicine = true;
            if (bCapture != null) if (bCapture.IsClicked(cur, prev)) drawCapture = true;
        }
        #endregion

        public static Button GetClickedButton() {
            return LastClicked;
        }
    }
}
