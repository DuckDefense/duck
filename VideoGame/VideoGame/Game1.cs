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
using OpenTK.Graphics.OpenGL;
using Sandbox.Classes;
using Sandbox.Forms;
using VideoGame.Classes;
using VideoGame.Forms;
using Settings = VideoGame.Classes.Settings;
using Type = VideoGame.Classes.Type;

namespace VideoGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
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
        private bool drawStarters;
        private bool battling = false;
        private bool encountered;
        private int selectedMonster;
        public bool AllowedToWalk = true;
        private ViewportAdapter viewportAdapter;
        private FramesPerSecondCounter fpsCounter;
        private Conversation.Message intro;
        Button Armler;
        Button Mimird;
        Button Guilail;


        public Game1()
        {
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
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            fpsCounter = new FramesPerSecondCounter();
            viewportAdapter = new BoxingViewportAdapter(GraphicsDevice, Settings.ResolutionWidth, Settings.ResolutionHeight);
            camera = new Camera2D(viewportAdapter)
            {
                Zoom = 1f
            };
            IsMouseVisible = true;

            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += (s, e) => viewportAdapter.OnClientSizeChanged();
            DatabaseConnector.SetConnectionString(Settings.ServerName, Settings.Username, Settings.Password, Settings.DatabaseName);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            ContentLoader.SetContent(Content, graphics);
            _contentLoader.LoadContent();

            var playerList = DatabaseConnector.GetCharacters(Login.UserName);
            player = playerList[0];

            if (player.Monsters.Count == 0)
            {

                List<string> lines = new List<string>
                {
                    "Oh..............",
                    "Hello there didn't see you",
                    "I'm proffessor Koak",
                    $"You must be {player.Name}",
                    "Nice to meet you",
                    "I've heard you are new here",
                    "This is a place overrun with monsters",
                    "Without your own monster these monster can be deadly",
                    "So let's get started",
                    "I've three monsters ready",
                    "You will have the option to choose one of them!",
                    "All monsters have a type",
                    "The monsters you can choose out of are the types: fire, water and grass",
                    "Now you can choose your own monster",
                    "Choose wisely"
                };
                intro = new Conversation.Message(lines, Color.Black, new Character("Koak"));
                intro.Visible = true;
                Armler = new Button(new Rectangle(96, 128, 96, 96), ContentLoader.ArmlerFront);
                Mimird = new Button(new Rectangle(192, 128, 96, 96), ContentLoader.MimirdFront);
                Guilail = new Button(new Rectangle(288, 128, 96, 96), ContentLoader.GuilailFront);
            }

            menu = new Menu(player, new Vector2(Settings.ResolutionWidth - 64, 0));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            _contentLoader.UnloadContent();
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

            //Check if the player is new and has no monsters
            if (player.Monsters.Count == 0)
            {
                intro.Update(currentKeyboardState, previousKeyboardState, player);
                if (intro.CurrentIndex == intro.Lines.Count - 1)
                    drawStarters = true;
                if (drawStarters)
                {
                    Armler.Update(currentMouseState, previousMouseState);
                    Mimird.Update(currentMouseState, previousMouseState);
                    Guilail.Update(currentMouseState, previousMouseState);
                    if (Armler.IsClicked(currentMouseState, previousMouseState))
                    {
                        selectedMonster = 1;
                        player.Monsters.Add(DatabaseConnector.GetMonster(selectedMonster, 5));
                        drawStarters = false;
                        AllowedToWalk = true;
                    }
                    if (Mimird.IsClicked(currentMouseState, previousMouseState))
                    {
                        selectedMonster = 4;
                        player.Monsters.Add(DatabaseConnector.GetMonster(selectedMonster, 5));
                        drawStarters = false;
                        AllowedToWalk = true;
                    }
                    if (Guilail.IsClicked(currentMouseState, previousMouseState))
                    {
                        selectedMonster = 7;
                        drawStarters = false;
                        player.Monsters.Add(DatabaseConnector.GetMonster(selectedMonster, 5));
                        AllowedToWalk = true;
                    }
                }
            }

            Drawer.UpdateMessage(currentKeyboardState, previousKeyboardState, player);
            if (currentBattle != null && !currentBattle.battleOver)
            {
                currentBattle.Update(currentMouseState, previousMouseState, gameTime);
                Drawer.UpdateBattleButtons(currentMouseState, previousMouseState);
                player.MonsterUpdate(gameTime);
            }
            else
            {
                menu.Update(gameTime, currentMouseState, previousMouseState, currentKeyboardState, previousKeyboardState);
                //if (!menu.Visible) {
                player.Update(gameTime, currentKeyboardState, previousKeyboardState);
                player.CurrentArea.Update(gameTime, currentKeyboardState, previousKeyboardState, player,
                    ref currentBattle);
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
        protected override void Draw(GameTime gameTime)
        {
            fpsCounter.Update(gameTime);
            GraphicsDevice.Clear(Color.Gray);
            spriteBatch.Begin();

            if (player.Monsters.Count == 0)
            {
                GraphicsDevice.Clear(Color.Black);
                spriteBatch.Draw(ContentLoader.Koak, new Vector2(150,256));
                intro.Draw(spriteBatch);
                if (drawStarters)
                {
                    Armler.Draw(spriteBatch);
                    Mimird.Draw(spriteBatch);
                    Guilail.Draw(spriteBatch);
                }
            }
            else
            {
                spriteBatch.Draw(ContentLoader.Grid, Vector2.Zero, Color.White);
                Drawer.DrawMessage(spriteBatch);
                if (currentBattle != null && !currentBattle.battleOver)
                {
                    spriteBatch.Draw(ContentLoader.GrassyBackground, Vector2.Zero);
                    currentBattle.Draw(spriteBatch, player);
                }
                else
                {
                    //Draw areas before player and opponents
                    player.CurrentArea.Draw(camera, spriteBatch);
                    player.CurrentArea.EnteredArea = false;
                    player.Draw(spriteBatch);
                    menu.Draw(spriteBatch, currentMouseState, previousMouseState);
                }
            }


            spriteBatch.DrawString(ContentLoader.Arial, $"FPS: {fpsCounter.AverageFramesPerSecond}", new Vector2(5, 5), Color.Yellow);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
