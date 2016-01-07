using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.Maps.Tiled;
using MonoGame.Extended.ViewportAdapters;
using Sandbox.Classes;
using VideoGame.Classes;
using Settings = VideoGame.Classes.Settings;
using Type = VideoGame.Classes.Type;

namespace VideoGame {
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Menu menu;
        private Character player;
        private ContentLoader _contentLoader = new ContentLoader();
        private KeyboardState currentKeyboardState, previousKeyboardState;
        private MouseState currentMouseState, previousMouseState;
        private Camera2D camera;
        private Vector2 battleBackgroundPos;
        private Battle currentBattle;
        private bool battling = false;
        private bool encountered;
        public bool AllowedToWalk = true;
        private ViewportAdapter viewportAdapter;
        private FramesPerSecondCounter fpsCounter;

        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = Settings.ResolutionHeight;
            graphics.PreferredBackBufferWidth = Settings.ResolutionWidth;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {
            // TODO: Add your initialization logic here

            fpsCounter = new FramesPerSecondCounter();
            viewportAdapter = new BoxingViewportAdapter(GraphicsDevice, Settings.ResolutionWidth, Settings.ResolutionHeight);
            camera = new Camera2D(viewportAdapter) {
                Zoom = 1f
            };
            IsMouseVisible = true;

            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += (s, e) => viewportAdapter.OnClientSizeChanged();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            ContentLoader.SetContent(Content, graphics);
            _contentLoader.LoadContent();
            player = new Character("Pietertje", 5000, new Inventory(), new List<Monster>(),
                ContentLoader.GronkeyFront, ContentLoader.GronkeyBack, ContentLoader.MCGirl, new Vector2(96, 96), true);
            player.Debug = true;
            player.CurrentArea = Area.City(player);
            player.CurrentArea.EnteredArea = true;
            player.Monsters.Add(Monster.Gronkey(15));
            player.Monsters.Add(Monster.Brass(15));
            player.Monsters.Add(Monster.Huffstein(15));
            player.Monsters.Add(Monster.Armler(15));
            player.Monsters.Add(Monster.Armler(15));
            player.Inventory.Add(Medicine.RoosVicee(), 1);
            player.Inventory.Add(Medicine.MagicStone(), 3);
            player.Inventory.Add(Medicine.Salt(), 2);
            player.Inventory.Add(Capture.RottenNet(), 198);
            player.Inventory.Add(Capture.RegularNet(), 1);
            player.Inventory.Add(Capture.GreatNet(), 4);
            currentBattle = new Battle(player, Monster.Gronkey(40000)) {
                battleOver = true,
                battleStart = false
            };
            player.Monsters[0].ReceiveExp(Monster.Gronkey(50));
            player.KnownMonsters.Add(Monster.Gronkey(5).Id, Monster.Gronkey(5));
            player.KnownMonsters.Add(Monster.Armler(5).Id, Monster.Armler(5));
            player.KnownMonsters.Add(Monster.Huffstein(5).Id, Monster.Huffstein(5));
            player.KnownMonsters.Add(Monster.Brass(5).Id, Monster.Brass(5));
            player.KnownMonsters.Add(Monster.Pantsler(5).Id, Monster.Pantsler(5));
            player.KnownMonsters.Add(Monster.Prestler(5).Id, Monster.Prestler(5));
            player.KnownMonsters.Add(Monster.Mimird(5).Id, Monster.Mimird(5));
            player.KnownMonsters.Add(Monster.Fester(5).Id, Monster.Fester(5));
            
            menu = new Menu(player, new Vector2(Settings.ResolutionWidth - 64, 0));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent() {
            _contentLoader.UnloadContent();
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {
            currentMouseState = Mouse.GetState();
            currentKeyboardState = Keyboard.GetState();

            if (!currentBattle.battleOver) {
                currentBattle.Update(currentMouseState, previousMouseState, gameTime);
                Drawer.UpdateBattleButtons(currentMouseState, previousMouseState);
                player.MonsterUpdate(gameTime);
            }
            else {
                player.Update(gameTime, currentKeyboardState, previousKeyboardState);
                menu.Update(gameTime, currentMouseState, previousMouseState, currentKeyboardState, previousKeyboardState);
                player.CurrentArea.Update(gameTime, currentKeyboardState, previousKeyboardState, player, ref currentBattle);
                player.CurrentArea.GetArea(player);
                player.CurrentArea.GetCollision(player);
                player.CurrentArea.GetEncounters(player, ref currentBattle, ref battling);
            }

            base.Update(gameTime);

            previousMouseState = currentMouseState;
            previousKeyboardState = currentKeyboardState;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            fpsCounter.Update(gameTime);
            GraphicsDevice.Clear(Color.Gray);

            spriteBatch.Begin();
            spriteBatch.Draw(ContentLoader.Grid, Vector2.Zero, Color.White);
            if (!currentBattle.battleOver) {
                spriteBatch.Draw(ContentLoader.GrassyBackground, Vector2.Zero);
                currentBattle.Draw(spriteBatch, player);
            }
            else {
                //Draw areas before player and opponents
                player.CurrentArea.Draw(camera, spriteBatch);
                player.CurrentArea.EnteredArea = false;
                player.Draw(spriteBatch);
                menu.Draw(spriteBatch, currentMouseState);
            }
            //spriteBatch.DrawString(ContentLoader.Arial, $"FPS: {fpsCounter.AverageFramesPerSecond}", new Vector2(5, 5), Color.Yellow);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
