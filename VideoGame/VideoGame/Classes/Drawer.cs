using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VideoGame.Classes {
    public static class Drawer {

        public static void DrawMoves(SpriteBatch batch, Character player, Button attackButton) {
            Rectangle pos = new Rectangle(attackButton.Position.X - ContentLoader.Button.Width, attackButton.Position.Y + ContentLoader.Button.Height, ContentLoader.Button.Width, ContentLoader.Button.Height);
            foreach (var move in player.Monsters[0].KnownMoves) {
                var b = new Button(new Rectangle(pos.X += ContentLoader.Button.Width, pos.Y, pos.Width, pos.Height), ContentLoader.Button, $"{move.Name}", ContentLoader.Arial);
                b.Draw(batch);
            }
        }

        public static void DrawItems(SpriteBatch batch, Character player) { }
        public static void DrawParty(SpriteBatch batch, Character player) { }

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
    }
}
