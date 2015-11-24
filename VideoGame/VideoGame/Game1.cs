﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using VideoGame.Classes;

namespace VideoGame {
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Character player;
        private ContentLoader _contentLoader = new ContentLoader();
        private KeyboardState currentKeyboardState, previousKeyboardState;
        private Camera2D camera;

        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = Settings.ResolutionHeigt;
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

            IsMouseVisible = true;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            ContentLoader.SetContent(Content);
            _contentLoader.LoadContent();

            var viewportAdapter = new ScalingViewportAdapter(GraphicsDevice, Settings.ResolutionWidth, Settings.ResolutionHeigt);

            camera = new Camera2D(viewportAdapter) {
                Zoom = 0.5f,
                Position = new Vector2(Area.Route1().Map.WidthInPixels / 4f,
                Area.Route1().Map.HeightInPixels / 4f)
            };

            player = new Character("Pietertje", 5000, new Inventory(), new List<Monster>(),
                ContentLoader.GronkeyFront, ContentLoader.GronkeyBack, ContentLoader.Christman, new Vector2(0, 0), camera);
            player.CurrentArea = Area.Route1();
            player.CurrentArea.EnteredArea = true;

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {
            currentKeyboardState = Keyboard.GetState();
            player.Update(gameTime, currentKeyboardState, previousKeyboardState);
            // TODO: Add your update logic here

            base.Update(gameTime);

            previousKeyboardState = currentKeyboardState;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            //Draw areas before player and opponents
            player.CurrentArea.Draw(player.Camera);

            player.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
