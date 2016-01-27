using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended;
using MonoGame.Extended.Maps.Tiled;
using Sandbox.Classes;

namespace VideoGame.Classes {
    public class Area {
        public bool Debug = true;
        private static SoundEffectInstance Sound;
        private static Point Levelrange;
        public string Name;
        public List<Monster> Monsters;
        public bool EnteredArea = false;
        public TiledMap Map;
        private static Texture2D tileset;
        private Rectangle encounterHitbox;
        private Rectangle collisionHitbox;
        private Vector2 SpawnLocation;
        public List<Character> OpponentList = new List<Character>();
        private Dictionary<Rectangle, string> AreaColliders = new Dictionary<Rectangle, string>();
        private List<Rectangle> EncounterColliders = new List<Rectangle>();
        private List<Rectangle> CollisionColliders = new List<Rectangle>();
        private List<Rectangle> BoxColliders = new List<Rectangle>();
        private List<Shop> shopList = new List<Shop>();
        private static int tileWidth = 32;
        private static int tileHeight = 32;
        private Vector2 previousPos = new Vector2();
        public Box Box;
        private bool BoxAccesed = false;

        public Area() {
        }

        public Area(string name, Point levelrange, List<Monster> monsters, List<Character> opponentList,
            Vector2 spawnLocation, TiledMap map) {
            Levelrange = levelrange;
            Name = name;
            Monsters = monsters;
            OpponentList = opponentList;
            SpawnLocation = spawnLocation;
            Map = map;
            Box = new Box();
        }

        public void Draw(Camera2D camera, SpriteBatch batch, Character player) {
            foreach (var layer in Map.Layers) layer.Draw();

            if (Debug) {
                foreach (var encounterCollide in EncounterColliders) {
                    batch.Draw(ContentLoader.Button, encounterCollide, Color.Green);
                }
                foreach (var collisionCollide in CollisionColliders) {
                    batch.Draw(ContentLoader.Button, collisionCollide, Color.Blue);
                }
                foreach (var areaCollide in AreaColliders) {
                    batch.Draw(ContentLoader.Button, areaCollide.Key, Color.Aqua);
                }
                foreach (var opponent in OpponentList) {
                    batch.Draw(ContentLoader.Health, opponent.AI.Hitbox, Color.White);
                }
            }
            foreach (var opponent in OpponentList) {
                opponent.Draw(batch);
            }
            foreach (var shop in shopList) {
                shop.Draw(batch, player);
            }
            Box.Draw(batch);
            foreach (var boxCollide in BoxColliders) {
                batch.Draw(ContentLoader.HealLady, boxCollide, Color.Yellow);
            }
        }

        public void Update(GameTime gameTime, KeyboardState currentKeyboardState, KeyboardState previousKeyboardState,
            MouseState currentMouseState, MouseState previousMouseState,
            Character player, ref Battle currentBattle) {
            if (OpponentList == null) return;
            if (OpponentList.Count == 0) return;
            foreach (var opponent in OpponentList) {
                opponent.UpdateMessages(currentKeyboardState, previousKeyboardState, player);
                opponent.Update(gameTime, currentKeyboardState, previousKeyboardState);
                opponent.AI.Update(player, ref currentBattle, ref shopList, currentMouseState, previousMouseState);
            }
            foreach (var shop in shopList) {
                shop.Update(currentMouseState, previousMouseState);
            }
            Box.Update(currentMouseState, previousMouseState, gameTime);
        }

        public void GetCollision(Character player) {
            CollisionColliders.Clear();
            var collisionLayers = Map.TileLayers.Where(layer => layer.Properties.ContainsKey("Collision") && layer.Properties.ContainsValue("true")).ToList();
            foreach (var layer in collisionLayers) {
                foreach (var tile in layer.Tiles) {
                    if (tile.Id != 0) {
                        collisionHitbox = new Rectangle((tile.X * tileWidth), (tile.Y * tileHeight), tileWidth, tileHeight);
                        CollisionColliders.Add(collisionHitbox);
                    }
                }
            }
            var boxLayer = Map.TileLayers.Where(layer => layer.Properties.ContainsKey("Box") && layer.Properties.ContainsValue("true")).ToList();
            foreach (var tile in boxLayer.SelectMany(layer => layer.Tiles.Where(x => x.Id != 0))) {
                collisionHitbox = new Rectangle((tile.X * tileWidth), (tile.Y * tileHeight), tileWidth, tileHeight);
                CollisionColliders.Add(collisionHitbox);
            }
            if (CollisionColliders.Count != 0) {
                foreach (var collision in CollisionColliders) {
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

        public void GetBox(Character player) {
            var boxLayers = Map.TileLayers.Where(layer => layer.Properties.ContainsKey("Box") && layer.Properties.ContainsValue("true")).ToList();
            foreach (var layer in boxLayers) {
                foreach (var tile in layer.Tiles.Where(tile => tile.Id != 0)) {
                    encounterHitbox = new Rectangle((tile.X * tileWidth), (tile.Y * (tileHeight * 2)), tileWidth, tileHeight);
                    BoxColliders.Add(encounterHitbox);
                }
            }
            if (BoxColliders.Count != 0) {
                foreach (var collision in BoxColliders) {
                    if (player.Hitbox.Intersects(collision)) {
                        //Show the box
                        if (!BoxAccesed) {
                            Box.Access(player, new Vector2(0, 0));
                            BoxAccesed = true;
                        }
                    }
                    else {
                        Box.Disconnect();
                        BoxAccesed = false;
                    }
                }
            }
        }

        public void GetEncounters(Character player, ref Battle battle, ref bool battling) {
            EncounterColliders.Clear();
            var encounterLayers = Map.TileLayers.Where(layer => layer.Properties.ContainsKey("Encounters") && layer.Properties.ContainsValue("true")).ToList();
            foreach (var layer in encounterLayers) {
                foreach (var tile in layer.Tiles) {
                    if (tile.Id != 0) {
                        encounterHitbox = new Rectangle((tile.X * tileWidth), (tile.Y * tileHeight), tileWidth, tileHeight);
                        EncounterColliders.Add(encounterHitbox);
                    }
                }
            }
            if (EncounterColliders.Count != 0) {
                foreach (var collision in EncounterColliders) {
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
            AreaColliders.Clear();
            var collisionLayers = Map.TileLayers.Where(layer => layer.Properties.ContainsKey("EnterArea")).ToList();
            foreach (var layer in collisionLayers) {
                foreach (var tile in layer.Tiles.Where(tile => tile.Id != 0)) {
                    //if(AreaColliders.ContainsKey(layer.Properties.Values))
                    collisionHitbox = new Rectangle((tile.X * tileWidth), (tile.Y * tileHeight), tileWidth, tileHeight);
                    var areaName = layer.Properties.Values.ElementAt(0);
                    if (!AreaColliders.ContainsKey(collisionHitbox))
                        AreaColliders.Add(collisionHitbox, areaName);
                }
            }
            if (AreaColliders.Count != 0) {
                foreach (var entry in AreaColliders.Where(entry => player.Hitbox.Intersects(entry.Key))) {
                    //Enter area
                    player.CountingDown = false;
                    player.Moved = 0;
                    player.PreviousArea = player.CurrentArea;
                    player.CurrentArea = GetAreaFromName(entry.Value, player);
                    player.Position = player.CurrentArea.SpawnLocation;
                    break;
                }
            }
        }

        public static Area GetAreaFromName(string n, Character player) {
            //return area if n is the name
            switch (n.ToLower()) {
            case "route 1":
                return Route1(player);
            case "route 2":
                return Route2(player);
            case "route 3":
                return Route3(player);
            case "route 4":
                return Route4(player);
            case "route 5":
                return Route5(player);
            case "route 6":
                return Route6(player);
            case "route 7":
                return Route7(player);
            case "route 8":
                return Route8(player);
            case "city":
                Random random = new Random();
                return random.Next(0, 10) == 2 ? EasterCity(player) : City(player);
            case "eastercity":
                return EasterCity(player);
            case "shop":
                return Shop();
            case "secrettunnel":
                return SecretTunnel(player);
            case "secretcity":
                return SecretCity(player);
            }
            return null;
        }

        public Monster GetRandomMonster() {
            CryptoRandom rand = new CryptoRandom();
            int index = rand.Next(0, Monsters.Count);
            Monsters[index].Stats.Health = Monsters[index].MaxHealth;
            Monsters[index].Ailment = Ailment.Normal;
            return Monsters[index];
        }

        #region Route1

        public static
            Area Route1(Character player) {
            if (Sound != null)
                Sound.Stop();

            Sound = ContentLoader.RouteSong.CreateInstance();
            Sound.IsLooped = true;
            Sound.Play();

            Character tegenstander;
            Inventory inventory = new Inventory();
            inventory.Add(Medicine.MagicStone(), 2);
            List<string> battleLine = new List<string> { "Hah! Almost didn't catch you" };
            List<string> winLine = new List<string> { "That's what you get for getting caught by me" };
            List<string> loseLine = new List<string> { "I wish I didn't catch you" };
            tegenstander = new Character("Christguy", 6700, inventory,
                new List<Monster> {
                    DatabaseConnector.GetMonster(1, 5),
                    DatabaseConnector.GetMonster(4, 15)
                },
                battleLine, winLine, loseLine,
                ContentLoader.Button, ContentLoader.Button, ContentLoader.ChristmanWorld,
                new Vector2(192, 192));
            tegenstander.AI = new AI(tegenstander, 8);
            tegenstander.Debug = true;

            Random random = new Random();
            Point levelrange = new Point(2, 8);
            var map = ContentLoader.Route1;
            Vector2 spawn = Vector2.One;
            if (player.PreviousArea != null)
                if (player.PreviousArea.Name == "City") {
                    spawn = new Vector2(384, 32);
                }
                else if (player.PreviousArea.Name == "EasterCity") {
                    spawn = new Vector2(384, 32);
                }
                else {
                    spawn = new Vector2(192, 416);
                }
            List<Monster> monsters = new List<Monster>
            {
                DatabaseConnector.GetMonster(1, random.Next(levelrange.X, levelrange.Y)),
                DatabaseConnector.GetMonster(10, random.Next(levelrange.X, levelrange.Y)),
                DatabaseConnector.GetMonster(12, random.Next(levelrange.X, levelrange.Y))
            };
            List<Character> opponents = new List<Character>
            {
                tegenstander
            };

            return new Area("Route 1", levelrange, monsters, opponents, spawn, map);
        }

        #endregion

        #region Route2

        public static
            Area Route2(Character player) {
            if (Sound != null)
                Sound.Stop();

            Sound = ContentLoader.RouteSong.CreateInstance();
            Sound.IsLooped = true;
            Sound.Play();

            //Character tegenstander;
            //tegenstander = new Character("Nice guy", 6700,
            //    new Inventory(),
            //    new List<Monster> { Monster.Armler(5), Monster.Huffstein(10) },
            //    ContentLoader.Button, ContentLoader.Button, ContentLoader.Christman,
            //    new Vector2(192, 192));
            //tegenstander.AI = new AI(tegenstander, 8, "Nice to meat you");
            //tegenstander.Debug = true;

            Random random = new Random();
            Point levelrange = new Point(8, 13);
            var map = ContentLoader.Route2;
            Vector2 spawn = Vector2.One;
            if (player.PreviousArea != null)
                if (player.PreviousArea.Name == "Route 1") {
                    spawn = new Vector2(64, 32);
                }
                else {
                    spawn = new Vector2(32, 384);
                }
            List<Monster> monsters = new List<Monster>
            {
                DatabaseConnector.GetMonster(1, random.Next(levelrange.X, levelrange.Y)),
                DatabaseConnector.GetMonster(10, random.Next(levelrange.X, levelrange.Y)),
                DatabaseConnector.GetMonster(12, random.Next(levelrange.X, levelrange.Y))
            };
            List<Character> opponents = new List<Character> {
            };

            return new Area("Route 2", levelrange, monsters, opponents, spawn, map);
        }

        #endregion

        #region Route3

        public static
            Area Route3(Character player) {
            if (Sound != null)
                Sound.Stop();

            Sound = ContentLoader.RouteSong.CreateInstance();
            Sound.IsLooped = true;
            Sound.Play();

            //Character tegenstander;
            //tegenstander = new Character("Nice guy", 6700,
            //    new Inventory(),
            //    new List<Monster> { Monster.Armler(5), Monster.Huffstein(10) },
            //    ContentLoader.Button, ContentLoader.Button, ContentLoader.Christman,
            //    new Vector2(192, 192));
            //tegenstander.AI = new AI(tegenstander, 8, "Nice to meat you");
            //tegenstander.Debug = true;

            Random random = new Random();
            Point levelrange = new Point(13, 17);
            var map = ContentLoader.Route3;
            Vector2 spawn = Vector2.One;
            if (player.PreviousArea != null)
                if (player.PreviousArea.Name == "Route 2") {
                    spawn = new Vector2(736, 392);
                }
                else {
                    spawn = new Vector2(352, 32);
                }
            List<Monster> monsters = new List<Monster>
            {
                DatabaseConnector.GetMonster(1, random.Next(levelrange.X, levelrange.Y)),
                DatabaseConnector.GetMonster(10, random.Next(levelrange.X, levelrange.Y)),
                DatabaseConnector.GetMonster(12, random.Next(levelrange.X, levelrange.Y))
            };
            List<Character> opponents = new List<Character> {
            };

            return new Area("Route 3", levelrange, monsters, opponents, spawn, map);
        }

        #endregion

        #region Route4

        public static
            Area Route4(Character player) {
            if (Sound != null)
                Sound.Stop();

            Sound = ContentLoader.RouteSong.CreateInstance();
            Sound.IsLooped = true;
            Sound.Play();

            //Character tegenstander;
            //tegenstander = new Character("Nice guy", 6700,
            //    new Inventory(),
            //    new List<Monster> { Monster.Armler(5), Monster.Huffstein(10) },
            //    ContentLoader.Button, ContentLoader.Button, ContentLoader.Christman,
            //    new Vector2(192, 192));
            //tegenstander.AI = new AI(tegenstander, 8, "Nice to meat you");
            //tegenstander.Debug = true;

            Random random = new Random();
            Point levelrange = new Point(17, 22);
            var map = ContentLoader.Route4;
            Vector2 spawn = Vector2.One;
            if (player.PreviousArea != null)
                if (player.PreviousArea.Name == "Route 3") {
                    spawn = new Vector2(384, 416);
                }
                else {
                    spawn = new Vector2(704, 96);
                }
            List<Monster> monsters = new List<Monster>
            {
                DatabaseConnector.GetMonster(1, random.Next(levelrange.X, levelrange.Y)),
                DatabaseConnector.GetMonster(10, random.Next(levelrange.X, levelrange.Y)),
                DatabaseConnector.GetMonster(12, random.Next(levelrange.X, levelrange.Y))
            };
            List<Character> opponents = new List<Character> {
            };

            return new Area("Route 4", levelrange, monsters, opponents, spawn, map);
        }

        #endregion

        #region Route5

        public static
            Area Route5(Character player) {
            if (Sound != null)
                Sound.Stop();

            Sound = ContentLoader.RouteSong.CreateInstance();
            Sound.IsLooped = true;
            Sound.Play();

            //Character tegenstander;
            //tegenstander = new Character("Nice guy", 6700,
            //    new Inventory(),
            //    new List<Monster> { Monster.Armler(5), Monster.Huffstein(10) },
            //    ContentLoader.Button, ContentLoader.Button, ContentLoader.Christman,
            //    new Vector2(192, 192));
            //tegenstander.AI = new AI(tegenstander, 8, "Nice to meat you");
            //tegenstander.Debug = true;

            Random random = new Random();
            Point levelrange = new Point(22, 27);
            var map = ContentLoader.Route5;
            Vector2 spawn = Vector2.One;
            if (player.PreviousArea != null)
                if (player.PreviousArea.Name == "Route 4") {
                    spawn = new Vector2(608, 416);
                }
                else {
                    spawn = new Vector2(0, 32);
                }
            List<Monster> monsters = new List<Monster>
            {
                DatabaseConnector.GetMonster(1, random.Next(levelrange.X, levelrange.Y)),
                DatabaseConnector.GetMonster(10, random.Next(levelrange.X, levelrange.Y)),
                DatabaseConnector.GetMonster(12, random.Next(levelrange.X, levelrange.Y))
            };
            List<Character> opponents = new List<Character> {
            };

            return new Area("Route 5", levelrange, monsters, opponents, spawn, map);
        }

        #endregion

        #region Route6

        public static
            Area Route6(Character player) {
            if (Sound != null)
                Sound.Stop();

            Sound = ContentLoader.RouteSong.CreateInstance();
            Sound.IsLooped = true;
            Sound.Play();

            //Character tegenstander;
            //tegenstander = new Character("Nice guy", 6700,
            //    new Inventory(),
            //    new List<Monster> { Monster.Armler(5), Monster.Huffstein(10) },
            //    ContentLoader.Button, ContentLoader.Button, ContentLoader.Christman,
            //    new Vector2(192, 192));
            //tegenstander.AI = new AI(tegenstander, 8, "Nice to meat you");
            //tegenstander.Debug = true;

            Random random = new Random();
            Point levelrange = new Point(27, 30);
            var map = ContentLoader.Route6;
            Vector2 spawn = Vector2.One;
            if (player.PreviousArea != null)
                if (player.PreviousArea.Name == "Route 5") {
                    spawn = new Vector2(64, 32);
                }
                else {
                    spawn = new Vector2(96, 384);
                }
            List<Monster> monsters = new List<Monster>
            {
                DatabaseConnector.GetMonster(1, random.Next(levelrange.X, levelrange.Y)),
                DatabaseConnector.GetMonster(10, random.Next(levelrange.X, levelrange.Y)),
                DatabaseConnector.GetMonster(12, random.Next(levelrange.X, levelrange.Y))
            };
            List<Character> opponents = new List<Character> {
            };

            return new Area("Route 6", levelrange, monsters, opponents, spawn, map);
        }

        #endregion

        #region Route7

        public static
            Area Route7(Character player) {
            if (Sound != null)
                Sound.Stop();

            Sound = ContentLoader.RouteSong.CreateInstance();
            Sound.IsLooped = true;
            Sound.Play();

            //Character tegenstander;
            //tegenstander = new Character("Nice guy", 6700,
            //    new Inventory(),
            //    new List<Monster> { Monster.Armler(5), Monster.Huffstein(10) },
            //    ContentLoader.Button, ContentLoader.Button, ContentLoader.Christman,
            //    new Vector2(192, 192));
            //tegenstander.AI = new AI(tegenstander, 8, "Nice to meat you");
            //tegenstander.Debug = true;

            Random random = new Random();
            Point levelrange = new Point(22, 27);
            var map = ContentLoader.Route7;
            Vector2 spawn = Vector2.One;
            if (player.PreviousArea != null)
                if (player.PreviousArea.Name == "Route 6") {
                    spawn = new Vector2(96, 448);
                }
                else {
                    spawn = new Vector2(672, 384);
                }
            List<Monster> monsters = new List<Monster>
            {
                DatabaseConnector.GetMonster(1, random.Next(levelrange.X, levelrange.Y)),
                DatabaseConnector.GetMonster(10, random.Next(levelrange.X, levelrange.Y)),
                DatabaseConnector.GetMonster(12, random.Next(levelrange.X, levelrange.Y))
            };
            List<Character> opponents = new List<Character> {
            };

            return new Area("Route 7", levelrange, monsters, opponents, spawn, map);
        }

        #endregion

        #region Route8

        public static
            Area Route8(Character player) {
            if (Sound != null)
                Sound.Stop();

            Sound = ContentLoader.RouteSong.CreateInstance();
            Sound.IsLooped = true;
            Sound.Play();

            //Character tegenstander;
            //tegenstander = new Character("Nice guy", 6700,
            //    new Inventory(),
            //    new List<Monster> { Monster.Armler(5), Monster.Huffstein(10) },
            //    ContentLoader.Button, ContentLoader.Button, ContentLoader.Christman,
            //    new Vector2(192, 192));
            //tegenstander.AI = new AI(tegenstander, 8, "Nice to meat you");
            //tegenstander.Debug = true;

            Random random = new Random();
            Point levelrange = new Point(22, 27);
            var map = ContentLoader.Route8;
            Vector2 spawn = Vector2.One;
            if (player.PreviousArea != null)
                if (player.PreviousArea.Name == "Route 7") {
                    spawn = new Vector2(672, 128);
                }
                else {
                    spawn = new Vector2(256, 320);
                }
            List<Monster> monsters = new List<Monster>
            {
                DatabaseConnector.GetMonster(1, random.Next(levelrange.X, levelrange.Y)),
                DatabaseConnector.GetMonster(10, random.Next(levelrange.X, levelrange.Y)),
                DatabaseConnector.GetMonster(12, random.Next(levelrange.X, levelrange.Y))
            };
            List<Character> opponents = new List<Character> {
            };

            return new Area("Route 8", levelrange, monsters, opponents, spawn, map);
        }

        #endregion

        public static Area City(Character player) {
            if (Sound != null)
                Sound.Stop();

            Sound = ContentLoader.TownSong.CreateInstance();
            Sound.IsLooped = true;
            Sound.Play();

            Point levelrange = new Point(3, 8);
            var map = ContentLoader.City;

            Vector2 spawn = Vector2.One;
            if (player.PreviousArea != null)
                if (player.PreviousArea.Name == "Shop") {
                    spawn = new Vector2(256, 192);
                }
                else if (player.PreviousArea.Name == "Route 1") {
                    spawn = new Vector2(256, 416);
                }
                else {
                    spawn = new Vector2(32, 96);
                }

            List<Monster> monsters = new List<Monster>();
            List<Character> opponents = new List<Character>();
            //Sound = ContentLoader.RouteSong;
            //Sound.IsLooped = true;
            //Sound.Play();

            return new Area("City", levelrange, monsters, opponents, spawn, map);
        }

        public static Area Shop() {
            var spawn = new Vector2(288, 256);
            var map = ContentLoader.Shop;
            var inventory = new Inventory();
            inventory.Add(Capture.RottenNet(), 99);
            var introShopLines = new List<string> { "Hello!\n Do you want to buy something?" };
            var byeShopLines = new List<string> { "Take a look", "Thanks for stopping by" };
            var ShopLady = new Character("HealLady", 0, inventory, introShopLines, byeShopLines, NPCKind.Shop,
                ContentLoader.ChristmanFront, ContentLoader.ChristmanBack, ContentLoader.HealLady,
                new Vector2(288, 64));

            ShopLady.AI = new AI(ShopLady, 2);
            ShopLady.Debug = true;
            ShopLady.Direction = Direction.Down;
            ShopLady.SetLineOfSight(2);

            var introLines = new List<string> { "Hello!\n Do you want to restore your monsters?" };
            var byeLines = new List<string> { "Here you go", "Thanks for stopping by" };
            var healLady = new Character("HealLady", 0, null, introLines, byeLines, NPCKind.Healer,
                ContentLoader.ChristmanFront, ContentLoader.ChristmanBack, ContentLoader.HealLady,
                new Vector2(224, 64));

            healLady.AI = new AI(healLady, 2);
            healLady.Debug = true;
            healLady.Direction = Direction.Down;
            healLady.SetLineOfSight(2);

            List<Character> opponents = new List<Character>();
            opponents.Add(healLady);
            opponents.Add(ShopLady);

            return new Area("Shop", Point.Zero, new List<Monster>(), opponents, spawn, map);
        }

        #region Secret tunnel

        public static Area SecretTunnel(Character player) {
            if (Sound != null)
                Sound.Stop();

            Sound = ContentLoader.TownSong.CreateInstance();
            Sound.IsLooped = true;
            Sound.Play();

            Random random = new Random();
            Point levelrange = new Point(3, 8);
            var map = ContentLoader.SecretTunnel;

            Vector2 spawn = Vector2.One;
            if (player.PreviousArea != null)
                if (player.PreviousArea.Name == "EasterCity") {
                    spawn = new Vector2(384, 416);
                }
                else {
                    spawn = new Vector2(384, 32);
                }

            List<Monster> monsters = new List<Monster>();
            List<Character> opponents = new List<Character>();
            //Sound = ContentLoader.RouteSong;
            //Sound.IsLooped = true;
            //Sound.Play();

            return new Area("SecretTunnel", levelrange, monsters, opponents, spawn, map);
        }
        #endregion

        #region Secret City

        public static Area SecretCity(Character player) {
            if (Sound != null)
                Sound.Stop();

            Sound = ContentLoader.TownSong.CreateInstance();
            Sound.IsLooped = true;
            Sound.Play();

            Random random = new Random();
            Point levelrange = new Point(3, 8);
            var map = ContentLoader.SecretCity;

            Vector2 spawn = Vector2.One;
            spawn = new Vector2(384, 416);

            List<Monster> monsters = new List<Monster>();
            List<Character> opponents = new List<Character>();
            //Sound = ContentLoader.RouteSong;
            //Sound.IsLooped = true;
            //Sound.Play();

            return new Area("SecretCity", levelrange, monsters, opponents, spawn, map);
        }
        #endregion

        public static Area EasterCity(Character player) {
            if (Sound != null)
                Sound.Stop();

            Sound = ContentLoader.TownSong.CreateInstance();
            Sound.IsLooped = true;
            Sound.Play();

            Point levelrange = new Point(3, 8);
            var map = ContentLoader.EasterCity;

            Vector2 spawn = Vector2.One;
            if (player.PreviousArea != null)
                if (player.PreviousArea.Name == "Shop") {
                    spawn = new Vector2(256, 192);
                }
                else if (player.PreviousArea.Name == "Route 1") {
                    spawn = new Vector2(256, 416);
                }
                else {
                    spawn = new Vector2(32, 96);
                }

            List<Monster> monsters = new List<Monster>();
            List<Character> opponents = new List<Character>();
            //Sound = ContentLoader.RouteSong;
            //Sound.IsLooped = true;
            //Sound.Play();

            return new Area("EasterCity", levelrange, monsters, opponents, spawn, map);
        }
    }
}
