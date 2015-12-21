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
        private Character player, tegenstander;
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
            var tegenstanderMonsters = new List<Monster> { Monster.Armler(5), Monster.Huffstein(10) };
            tegenstander = new Character("Nice guy", 6700, new Inventory(), tegenstanderMonsters, 
                ContentLoader.Button, ContentLoader.Button, ContentLoader.Christman, new Vector2(200, 195));
            tegenstander.AI = new AI(tegenstander, 8, "YOOOOOOOOOOOOOOOOOOOOOOO");
            player = new Character("Pietertje", 5000, new Inventory(), new List<Monster>(),
                ContentLoader.GronkeyFront, ContentLoader.GronkeyBack, ContentLoader.Christman, new Vector2(150, 100), true);
            player.Debug = true;
            player.CurrentArea = Area.Route1();
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

            // TODO: use this.Content to load your game content here
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
                tegenstander.Update(gameTime, currentKeyboardState, previousKeyboardState);
                tegenstander.AI.Update(player, ref currentBattle);
                player.Update(gameTime, currentKeyboardState, previousKeyboardState);
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
            if (!currentBattle.battleOver) {
                spriteBatch.Draw(ContentLoader.GrassyBackground, Vector2.Zero);
                currentBattle.Draw(spriteBatch, player);
            }
            else {
                //Draw areas before player and opponents
                player.CurrentArea.Draw(camera, spriteBatch);
                player.CurrentArea.EnteredArea = false;
                player.Draw(spriteBatch);
                tegenstander.Draw(spriteBatch);
            }
            spriteBatch.DrawString(ContentLoader.Arial, $"FPS: {fpsCounter.AverageFramesPerSecond}", new Vector2(5, 5), Color.Yellow);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
