using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VideoGame.Classes
{
    internal class Area
    {
        private static Point Levelrange;
        public string Monsterarea;
        public List<Monster> Monsters;
        public Texture2D Map;
        public Texture2D Collision;

        public Area(string monsterarea, Point levelrange, List<Monster>monsters, Texture2D map, Texture2D collision)
        {
            Levelrange = levelrange;
            Monsterarea = monsterarea;
            Monsters = monsters;
            Map = map;
            Collision = collision;
        }

        #region Route1
        public static Area Route1()
        {
            Random random = new Random();
            Point levelrange = new Point(4,6);
            List<Monster> monsters = new List<Monster> {
                Monster.Armler(random.Next(levelrange.X, levelrange.Y)),
                Monster.Gronkey(random.Next(levelrange.X, levelrange.Y))
            };

            return new Area("Route1", levelrange, monsters, TextureLoader.GronkeyFront, TextureLoader.GronkeyBack);
        }
        #endregion
        #region Route2
        public static Area Route2()
        {
            Random random = new Random();
            Point levelrange = new Point(6, 9);
            List<Monster> monsters = new List<Monster> {
                Monster.Armler(random.Next(levelrange.X, levelrange.Y)),
                Monster.Gronkey(random.Next(levelrange.X, levelrange.Y))
            };

            return new Area("Route2", levelrange, monsters, TextureLoader.GronkeyFront, TextureLoader.GronkeyBack);
        }
        #endregion
        #region Route3
        public static Area Route3()
        {
            Random random = new Random();
            Point levelrange = new Point(9, 12);
            List<Monster> monsters = new List<Monster> {
                Monster.Armler(random.Next(levelrange.X, levelrange.Y)),
                Monster.Gronkey(random.Next(levelrange.X, levelrange.Y))
            };

            return new Area("Route3", levelrange, monsters, TextureLoader.GronkeyFront, TextureLoader.GronkeyBack);
        }
#endregion
    }
}
