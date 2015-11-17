using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TiledSharp;

namespace VideoGame.Classes {
    internal class Area {
        private static Point Levelrange;
        public string Monsterarea;
        public List<Monster> Monsters;
        public TmxMap Map;

        public Area(string monsterarea, Point levelrange, List<Monster> monsters, TmxMap map) {
            Levelrange = levelrange;
            Monsterarea = monsterarea;
            Monsters = monsters;
            Map = map;
        }

        public void Draw(SpriteBatch batch) {
            batch.Begin();
            for (var i = 0; i < Map.Layers[0].Tiles.Count; i++) {
                int gid = Map.Layers[0].Tiles[i].Gid;
                //If tile is not empty
                if (gid != 0) {
                    int tileFrame = gid - 1;
                    int row = tileFrame / (TextureLoader.TileSet.Height / Map.TileHeight);

                    float x = (i % Map.Width) * Map.TileWidth;
                    float y = (float)Math.Floor(i / (double)Map.Width) * Map.TileHeight;

                    Rectangle tilesetRectangle = new Rectangle(Map.TileWidth * tileFrame, Map.TileHeight * row, 32, 32);

                    batch.Draw(TextureLoader.TileSet, new Rectangle((int) x, (int) y, 32, 32), tilesetRectangle, Color.White);
                }
            }
            batch.End();
        }

        #region Route1
        public static Area Route1() {
            Random random = new Random();
            Point levelrange = new Point(4, 6);
            var map = new TmxMap(@"Content/Maps/Map.tmx");
            List<Monster> monsters = new List<Monster> {
                Monster.Armler(random.Next(levelrange.X, levelrange.Y)),
                Monster.Gronkey(random.Next(levelrange.X, levelrange.Y))
            };

            return new Area("Route1", levelrange, monsters, map);
        }
        #endregion
    }
}
