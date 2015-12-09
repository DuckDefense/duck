﻿using System;
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

            //Window.AllowUserResizing = true;
            //Window.ClientSizeChanged += (s, e) => viewportAdapter.OnClientSizeChanged();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            ContentLoader.SetContent(Content, graphics);
            _contentLoader.LoadContent();
            //var viewportAdapter = new ScalingViewportAdapter(GraphicsDevice, Settings.ResolutionWidth, Settings.ResolutionHeight);

            //camera = new Camera2D(viewportAdapter) {
            //    Zoom = 0.5f,
            //    Position = new Vector2((Settings.ResolutionWidth / 2) - 32, (Settings.ResolutionHeight / 2) - 32)
            //};
            tegenstander = new Character("nice", 600000, new Inventory(), new List<Monster>(), null, null, ContentLoader.Christman, new Vector2(0, 0));
            tegenstander.AI = new AI(tegenstander, 16, "YOOOOOOOOOOOOOOOOOOOOOOOOOO");
            tegenstander.Monsters.Add(Monster.Gronkey(20));
            //tegenstander.Direction = Direction.Down;
            //tegenstander.SetLineOfSight(8);
            tegenstander.Debug = true;
            //tegenstander.Controllable = true;

            player = new Character("Pietertje", 5000, new Inventory(), new List<Monster>(),
                ContentLoader.GronkeyFront, ContentLoader.GronkeyBack, ContentLoader.Christman, new Vector2(50, 100), true);
            //player.Debug = true;
            player.CurrentArea = Area.Route1();
            player.CurrentArea.EnteredArea = true;
            player.CurrentArea.GetCollision(player);
            player.Monsters.Add(Monster.Gronkey(15));
            player.Monsters.Add(Monster.Brass(15));
            player.Monsters.Add(Monster.Huffstein(15));
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
            fpsCounter.Update(gameTime);
            currentMouseState = Mouse.GetState();
            currentKeyboardState = Keyboard.GetState();
            player.Update(gameTime, currentKeyboardState, previousKeyboardState);
            

            if (!battling) {
                if (encountered) {
                    //Start battle
                    currentBattle = new Battle(player, Monster.Armler(15));
                    encountered = false;
                    battling = true;
                }
            }

            if (!currentBattle.battleOver) {
                currentBattle.Update(currentMouseState, previousMouseState, gameTime);
                Drawer.UpdateBattleButtons(currentMouseState, previousMouseState);

            }
            else {
                player.SetLineOfSight(8);
                //battling = false;
                tegenstander.Update(gameTime, currentKeyboardState, previousKeyboardState);
                tegenstander.AI.Update(gameTime, player, ref AllowedToWalk, ref currentBattle);
                camera.LookAt(player.Position);
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
            GraphicsDevice.Clear(Color.Gray);
            spriteBatch.Begin();
            spriteBatch.DrawString(ContentLoader.Arial, string.Format("FPS: {0} Zoom: {1}", fpsCounter.AverageFramesPerSecond, camera.Zoom), new Vector2(5, 5), new Color(0.5f, 0.5f, 0.5f));


            if (!currentBattle.battleOver) {
                spriteBatch.Draw(ContentLoader.GrassyBackground, Vector2.Zero);
                currentBattle.Draw(spriteBatch, player);
            }
            else {
                //Draw areas before player and opponents
                player.CurrentArea.Draw(camera);
                spriteBatch.Draw(ContentLoader.Button, tegenstander.AI.Hitbox, Color.White);
                player.Draw(spriteBatch);
                tegenstander.Draw(spriteBatch);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}