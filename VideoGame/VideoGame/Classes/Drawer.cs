using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
