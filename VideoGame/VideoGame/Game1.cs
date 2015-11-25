using System.Collections.Generic;
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
        private MouseState currentMouseState, previousMouseState;
        private Camera2D camera;
        private bool battling = true;
        private Button attackButton;
        private Button runButton;
        private Button inventoryButton;
        private Button partyButton;
        private Vector2 battleBackgroundPos;
        private Battle currentBattle;

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

            var viewportAdapter = new ScalingViewportAdapter(GraphicsDevice, Settings.ResolutionWidth, Settings.ResolutionHeight);

            camera = new Camera2D(viewportAdapter) {
                Zoom = 0.5f,
                Position = new Vector2((Settings.ResolutionWidth/2) - 32, (Settings.ResolutionHeight /2) - 32)
            };

            player = new Character("Pietertje", 5000, new Inventory(), new List<Monster>(),
                ContentLoader.GronkeyFront, ContentLoader.GronkeyBack, ContentLoader.Christman, camera.Position, true);
            player.CurrentArea = Area.Route1();
            player.CurrentArea.EnteredArea = true;
            player.Monsters.Add(Monster.Gronkey(15));

            int buttonPos = 0;

            attackButton = new Button(new Rectangle(buttonPos, ContentLoader.GrassyBackground.Height,
                ContentLoader.Button.Width, ContentLoader.Button.Height), ContentLoader.Button, "Attack", ContentLoader.Arial);
            inventoryButton = new Button(new Rectangle((int)(buttonPos + 64), ContentLoader.GrassyBackground.Height,
                ContentLoader.Button.Width, ContentLoader.Button.Height), ContentLoader.Button, "Items", ContentLoader.Arial);
            partyButton = new Button(new Rectangle((int)(buttonPos + 128), ContentLoader.GrassyBackground.Height,
                ContentLoader.Button.Width, ContentLoader.Button.Height), ContentLoader.Button, "Party", ContentLoader.Arial);
            runButton = new Button(new Rectangle((int)(buttonPos + 192), ContentLoader.GrassyBackground.Height,
                ContentLoader.Button.Width, ContentLoader.Button.Height), ContentLoader.Button, "Run", ContentLoader.Arial);




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
        protected override void Update(GameTime gameTime)
        {
            currentMouseState = Mouse.GetState();
            currentKeyboardState = Keyboard.GetState();
            //player.Update(gameTime, currentKeyboardState, previousKeyboardState);
            Movement(currentKeyboardState);

            currentBattle = new Battle(player, Monster.Gronkey(5));

            if (battling)
            {
                attackButton.Update(currentMouseState, previousMouseState);
                inventoryButton.Update(currentMouseState, previousMouseState);
                partyButton.Update(currentMouseState, previousMouseState);
                runButton.Update(currentMouseState, previousMouseState);

                if (attackButton.IsClicked(currentMouseState, previousMouseState))
                {
                    currentBattle.Selection = Selection.Attack;
                }
                if (inventoryButton.IsClicked(currentMouseState, previousMouseState))
                {
                    currentBattle.Selection = Selection.Item;
                }
                if (partyButton.IsClicked(currentMouseState, previousMouseState))
                {
                    currentBattle.Selection = Selection.Party;
                }
                if (runButton.IsClicked(currentMouseState, previousMouseState))
                {
                    currentBattle.Selection = Selection.Run;
                }
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
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            if (battling)
            {
                spriteBatch.Draw(ContentLoader.GrassyBackground, Vector2.Zero);
                attackButton.Draw(spriteBatch);
                runButton.Draw(spriteBatch);
                inventoryButton.Draw(spriteBatch);
                partyButton.Draw(spriteBatch);
            }
            //Draw areas before player and opponents
            player.CurrentArea.Draw(camera);

            player.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void Movement(KeyboardState cur) {
            if (cur.IsKeyDown(Settings.moveUp) || cur.IsKeyDown(Keys.Up))
                camera.Move(new Vector2(0, -2));
            if (cur.IsKeyDown(Settings.moveDown) || cur.IsKeyDown(Keys.Down))
                camera.Move(new Vector2(0, 2));
            if (cur.IsKeyDown(Settings.moveLeft) || cur.IsKeyDown(Keys.Left))
                camera.Move(new Vector2(-2, 0));
            if (cur.IsKeyDown(Settings.moveRight) || cur.IsKeyDown(Keys.Right))
                camera.Move(new Vector2(2, 0));
        }
    }
}
