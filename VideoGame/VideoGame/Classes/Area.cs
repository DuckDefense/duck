using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Maps.Tiled;

namespace VideoGame.Classes {
    public class Area {

        private static Point Levelrange;
        public string Name;
        public List<Monster> Monsters;
        public bool EnteredArea = false;
        public TiledMap Map;
        private static Texture2D tileset;
        static int tileWidth;
        static int tileHeight;

        public Area(string name, Point levelrange, List<Monster> monsters, TiledMap map) {
            Levelrange = levelrange;
            Name = name;
            Monsters = monsters;
            Map = map;
        }
        
        public void Draw(Camera2D camera) {
            {
                foreach (var layer in Map.Layers) {
                    layer.Draw(camera);
                }
            }
        }

        public void GetCollision() {
            foreach (var layer in Map.Layers) {
                if (layer.Properties.Count != 0) {
                    var layerPropKeys = layer.Properties.Keys.ToList();
                    var layerPropValues = layer.Properties.Values.ToList();
                    foreach (var key in layerPropKeys) {
                        if (key == "Collision") {
                            foreach (var value in layerPropValues) {
                                if (value == "true") {
                                    //There is collision and player can not move here
                                }
                            }
                        }
                        if (key == "Encounters") {
                            foreach (var value in layerPropValues) {
                                if (value == "true") {
                                    //When walking on this spot there is a chance to get encounters
                                }
                            }
                        }
                    }
                }
            }
        }

        #region Route1
            public static
            Area Route1() {
            Random random = new Random();
            Point levelrange = new Point(3, 8);
            
            var map = ContentLoader.Map;

            List<Monster> monsters = new List<Monster> {
                Monster.Armler(random.Next(levelrange.X, levelrange.Y)),
                Monster.Gronkey(random.Next(levelrange.X, levelrange.Y))
            };

            return new Area("Route 1", levelrange, monsters, map);
        }
        #endregion
    }
}
