using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Maps.Tiled;

namespace VideoGame.Classes {
    public class Area {
        public bool Debug = true;
        private static Point Levelrange;
        public string Name;
        public List<Monster> Monsters;
        public bool EnteredArea = false;
        public TiledMap Map;
        private static Texture2D tileset;
        private Rectangle encounterHitbox;
        private Rectangle collisionHitbox;
        private Vector2 SpawnLocation;
        List<Character> OpponentList = new List<Character>();
        Dictionary<string, Rectangle> AreaColiders = new Dictionary<string, Rectangle>();
        List<Rectangle> EncounterColiders = new List<Rectangle>();
        List<Rectangle> CollisionColiders = new List<Rectangle>();
        static int tileWidth = 32;
        static int tileHeight = 32;
        private Vector2 previousPos = new Vector2();

        public Area(string name, Point levelrange, List<Monster> monsters, List<Character> opponentList, Vector2 spawnLocation, TiledMap map) {
            Levelrange = levelrange;
            Name = name;
            Monsters = monsters;
            OpponentList = opponentList;
            SpawnLocation = spawnLocation;
            Map = map;
        }

        public void Draw(Camera2D camera, SpriteBatch batch) {
            Map.Draw(camera, true);
            //foreach (var layer in Map.Layers) layer.Draw(camera);

            if (Debug) {
                foreach (var opponent in OpponentList) {
                    batch.Draw(ContentLoader.Health, opponent.AI.Hitbox, Color.White);
                }
            foreach (var encounterColider in EncounterColiders) { batch.Draw(ContentLoader.Button, encounterColider, Color.Green); }
            foreach (var collisionColider in CollisionColiders) { batch.Draw(ContentLoader.Button, collisionColider, Color.Blue); }
            foreach (var areaCollider in AreaColiders) { batch.Draw(ContentLoader.Button, areaCollider.Value, Color.Aqua); }
            }
            foreach (var opponent in OpponentList) opponent.Draw(batch);
        }

        public void Update(GameTime gameTime, KeyboardState currentKeyboardState, KeyboardState previousKeyboardState,
            Character player, ref Battle currentBattle) {
            if (OpponentList == null) return;
            if (OpponentList.Count == 0) return;
                foreach (var opponent in OpponentList) {
                    opponent.Update(gameTime, currentKeyboardState, previousKeyboardState);
                    opponent.AI.Update(player, ref currentBattle);
                }
        }

        public void GetCollision(Character player) {
            CollisionColiders.Clear();
            var collisionLayers = Map.TileLayers.Where(layer => layer.Properties.ContainsKey("Collision") && layer.Properties.ContainsValue("true")).ToList();
            foreach (var layer in collisionLayers) {
                foreach (var tile in layer.Tiles) {
                    if (tile.Id != 0) {
                        collisionHitbox = new Rectangle((tile.X * tileWidth), (tile.Y * tileHeight), tileWidth, tileHeight);
                        CollisionColiders.Add(collisionHitbox);
                    }
                }
            }
            if (CollisionColiders.Count != 0) {
                foreach (var collision in CollisionColiders) {
                    if (player.Hitbox.Intersects(collision)) {
                        var moduloX = player.Position.X % 32;
                        var moduloY = player.Position.Y % 32;
                        var vector = new Vector2(collision.X, collision.Y);
                        //If right of it
                        if (vector.X >= player.Position.X) {
                            if (player.RightCollide(collision)) {
                                if (moduloX >= 16) moduloX -= 32;
                                player.Position.X -= moduloX;
                            }
                        }
                        //if left of it
                        if (vector.X <= player.Position.X) {
                            if (player.LeftCollide(collision)) {
                                if (moduloX >= 16) moduloX -= 32;
                                player.Position.X -= moduloX;
                            }
                        }
                        //if above it
                        if (player.UpperCollide(collision))
                            if (vector.Y >= player.Position.Y - 32) {
                                if (moduloY >= 16) moduloY -= 32;
                                player.Position.Y -= moduloY;
                            }
                        //}
                        //if below it
                        if (player.LowerCollide(collision))
                            if (vector.Y <= player.Position.Y + 32) {
                                if (moduloY >= 16) moduloY -= 32;
                                player.Position.Y -= moduloY;
                            }
                    }
                }
            }
        }

        public void GetEncounters(Character player, ref Battle battle, ref bool battling) {
            bool test = true;
            EncounterColiders.Clear();
            var encounterLayers = Map.TileLayers.Where(layer => layer.Properties.ContainsKey("Encounters") && layer.Properties.ContainsValue("true")).ToList();
            foreach (var layer in encounterLayers) {
                foreach (var tile in layer.Tiles) {
                    if (tile.Id != 0) {
                        encounterHitbox = new Rectangle((tile.X * tileWidth), (tile.Y * tileHeight), tileWidth, tileHeight);
                        EncounterColiders.Add(encounterHitbox);
                    }
                }
            }
            if (EncounterColiders.Count != 0) {
                foreach (var collision in EncounterColiders) {
                    if (player.Hitbox.Intersects(collision)) {
                        Random rand = new Random();

                        if (player.Position != previousPos && player.Moved >= 29f) {
                            var chanceToEncounter = rand.Next(0, 100);


                            if (chanceToEncounter <= 5) {
                                battling = true;
                                battle = new Battle(player, GetRandomMonster());
                            }
                        }
                    }
                }
            }
            previousPos = player.Position;
        }

        public void GetArea(Character player) {
            AreaColiders.Clear();
            var collisionLayers = Map.TileLayers.Where(layer => layer.Properties.ContainsKey("EnterArea")).ToList();
            foreach (var layer in collisionLayers) {
                foreach (var tile in layer.Tiles.Where(tile => tile.Id != 0)) {
                    //if(AreaColiders.ContainsKey(layer.Properties.Values))
                    collisionHitbox = new Rectangle((tile.X * tileWidth), (tile.Y * tileHeight), tileWidth, tileHeight);
                    var areaName = layer.Properties.Values.ElementAt(0);
                    if (!AreaColiders.ContainsKey(areaName)) {
                        AreaColiders.Add(areaName, collisionHitbox);
                    }
                }
            }
            if (AreaColiders.Count != 0) {
                foreach (var entry in AreaColiders.Where(entry => player.Hitbox.Intersects(entry.Value))) {
                    //Enter area
                    player.CurrentArea = GetAreaFromName(entry.Key);
                    player.Position = player.CurrentArea.SpawnLocation;
                    break;
                }
            }
        }

        private Area GetAreaFromName(string n) {
            //return area if n is the name
            switch (n) {
            case "Route 1":
                return Route1();
            case "City":
                return City();
            }
            return null;
        }

        public Monster GetRandomMonster() {
            CryptoRandom rand = new CryptoRandom();
            int index = rand.Next(0, Monsters.Count);
            Monsters[index].Stats.Health = Monsters[index].MaxHealth;
            return Monsters[index];
        }

        #region Route1
        public static
        Area Route1() {
            Character tegenstander;
            tegenstander = new Character("Nice guy", 6700,
                new Inventory(),
                new List<Monster> { Monster.Armler(5), Monster.Huffstein(10) },
                ContentLoader.Button, ContentLoader.Button, ContentLoader.Christman,
                new Vector2(192, 192));
            tegenstander.AI = new AI(tegenstander, 8, "Nice to meat you");
            tegenstander.Debug = true;

            Random random = new Random();
            Point levelrange = new Point(15, 18);
            var map = ContentLoader.Route2;
            var spawn = new Vector2(320, 320);
            List<Monster> monsters = new List<Monster> {
                Monster.Armler(random.Next(levelrange.X, levelrange.Y)),
                Monster.Gronkey(random.Next(levelrange.X, levelrange.Y)),
                Monster.Brass(random.Next(levelrange.X, levelrange.Y))
            };
            List<Character> opponents = new List<Character> {
                tegenstander
            };

            return new Area("Route 1", levelrange, monsters, opponents, spawn, map);
        }
        #endregion

        public static Area City() {
            Random random = new Random();
            Point levelrange = new Point(3, 8);
            var map = ContentLoader.City;
            var spawn = new Vector2(96, 96);
            List<Monster> monsters = new List<Monster>();
            List<Character> opponents = new List<Character>();

            return new Area("City", levelrange, monsters, opponents, spawn, map);
        }
    }
}
