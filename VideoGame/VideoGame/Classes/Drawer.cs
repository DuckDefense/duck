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
        private static bool drawItems, drawMedicine, drawCapture;
        private static Button bItems, bMedicine, bCapture;
        public static List<Button> MoveButtons, InventoryButtons, ItemButtons, PartyButtons;

        public static void DrawMoves(SpriteBatch batch, Character player) {
            MoveButtons = new List<Button>();
            var pos = new Vector2(ContentLoader.GrassyBackground.Height + ContentLoader.Button.Height, ContentLoader.Button.Width);
            Rectangle rec = new Rectangle((int)pos.X - ContentLoader.Button.Width, (int)pos.Y + ContentLoader.Button.Height, ContentLoader.Button.Width, ContentLoader.Button.Height);
            foreach (var move in player.Monsters[0].KnownMoves) {
                var b = new Button(new Rectangle(rec.X += ContentLoader.Button.Width, rec.Y, rec.Width, rec.Height), ContentLoader.Button, $"{move.Name}", ContentLoader.Arial);
                b.Draw(batch);
                MoveButtons.Add(b);
            }
        }

        public static void DrawInventory(SpriteBatch batch, Character player) {
            InventoryButtons = new List<Button>();
            var pos = new Vector2(ContentLoader.GrassyBackground.Height + ContentLoader.Button.Height, ContentLoader.Button.Width);
            Rectangle rec = new Rectangle((int)pos.X - ContentLoader.Button.Width, (int)pos.Y + ContentLoader.Button.Height, ContentLoader.Button.Width, ContentLoader.Button.Height);

            bItems = new Button(new Rectangle(rec.X += ContentLoader.Button.Width, rec.Y, rec.Width, rec.Height), ContentLoader.Button, "Items", ContentLoader.Arial);
            bMedicine = new Button(new Rectangle(rec.X += ContentLoader.Button.Width, rec.Y, rec.Width, rec.Height), ContentLoader.Button, "Medicine", ContentLoader.Arial);
            bCapture = new Button(new Rectangle(rec.X += ContentLoader.Button.Width, rec.Y, rec.Width, rec.Height), ContentLoader.Button, "Capture", ContentLoader.Arial);
            //var bKeyItems = new Button(new Rectangle(rec.X += ContentLoader.Button.Width, rec.Y, rec.Width, rec.Height), ContentLoader.Button, "KeyItems", ContentLoader.Arial);
            bItems.Draw(batch);
            bMedicine.Draw(batch);
            bCapture.Draw(batch);
            InventoryButtons.AddMany(bItems, bMedicine, bCapture);
            
        }

        public static void DrawItems(SpriteBatch batch, Character player) {
            
        }

        public static void DrawParty(SpriteBatch batch, Character player) {
            PartyButtons = new List<Button>();
            var party = player.Monsters;
        }

        public static void DrawBattle(SpriteBatch batch, Monster userMon, Monster oppoMon) {
            Rectangle userMonsterPos = new Rectangle(
                ContentLoader.GrassyBackground.Width - (int)(userMon.BackSprite.Width * 6.5),
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
            if(ItemButtons != null) foreach (var btn in ItemButtons) btn.Update(cur, prev);
            if(InventoryButtons != null) foreach (var btn in InventoryButtons) btn.Update(cur, prev);
            if(MoveButtons != null) foreach (var btn in MoveButtons) btn.Update(cur, prev);
            if(PartyButtons != null) foreach (var btn in PartyButtons) btn.Update(cur, prev);

            if (bItems.IsClicked(cur, prev)) {
            }
        }
    }
}
