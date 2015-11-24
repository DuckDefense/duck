using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using Fantas;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Turn_Based_Fanatsy.Class;

namespace MonoGameUI
{
    public class ConsoleText
    {
        public string Text { get; set; }
        public Color Color { get; set; }
        public SpriteFont Font { get; set; }

        public ConsoleText() { }
        public ConsoleText(string text, Color color, SpriteFont font)
        {
            Text = text;
            Color = color;
            Font = font;
        }

        public void Draw(SpriteBatch sp)
        {

        }
    }

    public class DevConsole : DrawableGameComponent
    {
        public delegate void OnCommandEventHandler(DevConsole console, string cmd);
        public event OnCommandEventHandler OnCommand;

        int selectedCommand;
        SpriteBatch spriteBatch;
        Game game;
        private float newPos;
        private PropertyInfo[] gameVariables;
        List<string> commandList = new List<string>();

        //Timer
        public float cursorTimer = 1f;
        public const float cursorDelay = 1f;
        public float holdDownTimer = .5f;
        public const float holdDownDelay = .5f;

        List<ConsoleText> buffer;
        //Used to keep track of what commands have been entered
        List<string> history = new List<string>();
        private int cursorIndex = 0;
        string currentText = "";
        string cursor = "|";
        int scrollPos = 0;

        Keys[] allowedKeys;
        Dictionary<Keys, string> keyStrings = new Dictionary<Keys, string>();
        Dictionary<Keys, string> shiftKeys = new Dictionary<Keys, string>();

        Texture2D background;

        public DevConsole(Game game, SpriteBatch spp)
            : base(game)
        {
            this.Visible = false;

            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public;
            //gameVariables = Game1.player.GetType().GetProperties();
            //Add a list with changeable variables.
            spriteBatch = spp;
            this.game = game;

            buffer = new List<ConsoleText>();

            #region Keys
            allowedKeys = new Keys[] { Keys.A, Keys.B, Keys.C, Keys.D, Keys.E, Keys.F, Keys.G, Keys.H, Keys.I, Keys.J, Keys.K, Keys.L, Keys.M, Keys.N, Keys.O, Keys.P, Keys.Q, Keys.R, Keys.S,
                                      Keys.T, Keys.U, Keys.V, Keys.W, Keys.X, Keys.Y, Keys.Z, Keys.D0, Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9,
                                      Keys.NumPad0, Keys.NumPad1,Keys.NumPad2, Keys.NumPad3, Keys.NumPad4, Keys.NumPad5, Keys.NumPad6, Keys.NumPad7, Keys.NumPad8, Keys.NumPad9,
                                      Keys.Separator, Keys.OemSemicolon, Keys.OemQuotes, Keys.OemQuestion, Keys.OemPlus, Keys.OemPipe, Keys.OemPeriod, Keys.OemOpenBrackets, Keys.OemMinus,
                                      Keys.OemComma, Keys.OemCloseBrackets, Keys.OemBackslash};
            keyStrings.Add(Keys.D0, "0");
            keyStrings.Add(Keys.D1, "1");
            keyStrings.Add(Keys.D2, "2");
            keyStrings.Add(Keys.D3, "3");
            keyStrings.Add(Keys.D4, "4");
            keyStrings.Add(Keys.D5, "5");
            keyStrings.Add(Keys.D6, "6");
            keyStrings.Add(Keys.D7, "7");
            keyStrings.Add(Keys.D8, "8");
            keyStrings.Add(Keys.D9, "9");

            keyStrings.Add(Keys.NumPad0, "0");
            keyStrings.Add(Keys.NumPad1, "1");
            keyStrings.Add(Keys.NumPad2, "2");
            keyStrings.Add(Keys.NumPad3, "3");
            keyStrings.Add(Keys.NumPad4, "4");
            keyStrings.Add(Keys.NumPad5, "5");
            keyStrings.Add(Keys.NumPad6, "6");
            keyStrings.Add(Keys.NumPad7, "7");
            keyStrings.Add(Keys.NumPad8, "8");
            keyStrings.Add(Keys.NumPad9, "9");

            keyStrings.Add(Keys.Separator, "");
            keyStrings.Add(Keys.OemSemicolon, ";");
            keyStrings.Add(Keys.OemQuotes, "\"");
            keyStrings.Add(Keys.OemQuestion, "?");
            keyStrings.Add(Keys.OemPlus, "+");
            keyStrings.Add(Keys.OemPipe, "!");
            keyStrings.Add(Keys.OemPeriod, ".");
            keyStrings.Add(Keys.OemOpenBrackets, "{");
            keyStrings.Add(Keys.OemCloseBrackets, "}");
            keyStrings.Add(Keys.OemMinus, "-");
            keyStrings.Add(Keys.OemComma, ",");
            keyStrings.Add(Keys.OemBackslash, "\\");

            shiftKeys.Add(Keys.D1, "!");
            shiftKeys.Add(Keys.D2, "@");
            shiftKeys.Add(Keys.D3, "#");
            shiftKeys.Add(Keys.D4, "$");
            shiftKeys.Add(Keys.D5, "%");
            shiftKeys.Add(Keys.D6, "^");
            shiftKeys.Add(Keys.D7, "&");
            shiftKeys.Add(Keys.D8, "*");
            shiftKeys.Add(Keys.D9, "(");
            shiftKeys.Add(Keys.D0, ")");
            #endregion
        }

        public void AddText(ConsoleText text)
        {
            buffer.Add(text);
        }
        public void AddText(string text, SpriteFont font, Color color)
        {
            AddText(new ConsoleText(text, color, font));
        }
        public void AddText(string text, SpriteFont font)
        {
            AddText(text, font, Color.White);
        }
        public void AddText(string text, Color color)
        {
            AddText(text, Game1.arial, color);
        }
        public void AddText(string text)
        {
            AddText(text, Game1.arial, Color.White);
        }

        public void LoadContent()
        {
            background = game.Content.Load<Texture2D>(@"Sprites\Battle\Highlight");
        }

        int historyInd = -1024;
        KeyboardState oldKeyState = Keyboard.GetState();
        MouseState oldMouseState = Mouse.GetState();
        public override void Update(GameTime time)
        {
            KeyboardState keyState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();

            if (keyState.IsKeyDown(Keys.OemTilde) && !oldKeyState.IsKeyDown(Keys.OemTilde))
            {
                this.Visible = !this.Visible;
                currentText = "";
            }

            if (historyInd == -1024)
                historyInd = history.Count;
            else if (historyInd < 0)
                historyInd = 0;
            else if (historyInd >= history.Count)
                historyInd = history.Count;

            if (this.Visible)
            {
                cursorTimer -= (float)time.ElapsedGameTime.TotalSeconds;
                if (cursorTimer < 0) //Every 0.25 second
                {
                    if (cursor == "|") cursor = "";
                    else if (cursor == "") cursor = "|";
                    cursorTimer = cursorDelay; //Reset Timer
                }

                if (keyState.IsKeyDown(Keys.Back))
                {
                    if (!oldKeyState.IsKeyDown(Keys.Back))
                        DeleteLastChar();
                    else
                    {
                        holdDownTimer -= (float)time.ElapsedGameTime.TotalSeconds;
                        if (holdDownTimer < 0)
                        {
                            if (keyState.IsKeyDown(Keys.Back))
                                DeleteLastChar();
                        }
                    }
                }
                else if (keyState.IsKeyDown(Keys.Enter) && !oldKeyState.IsKeyDown(Keys.Enter)) { handleCommand(); }
                else if (keyState.IsKeyDown(Keys.Tab) && !oldKeyState.IsKeyDown(Keys.Tab)) { try { currentText = commandList[0]; } catch { } }
                else if (keyState.IsKeyDown(Keys.Space) && !oldKeyState.IsKeyDown(Keys.Space)) { currentText += " "; }
                else if (keyState.IsKeyDown(Keys.Up) && !oldKeyState.IsKeyDown(Keys.Up))
                {
                    try
                    {
                        historyInd--;
                        if (historyInd < 0)
                            historyInd = 0;
                        currentText = history[historyInd];
                    }
                    catch { }
                }
                else if (keyState.IsKeyDown(Keys.Down) && !oldKeyState.IsKeyDown(Keys.Down))
                {
                    historyInd++;
                    if (historyInd >= history.Count)
                    {
                        historyInd = history.Count - 1;
                        currentText = "";
                    }
                    else
                        currentText = history[historyInd];
                }
                else
                {
                    holdDownTimer = holdDownDelay; //Reset hold down delay
                    cursorIndex++;
                    bool shiftDown = false;
                    string chr;
                    foreach (Keys key in keyState.GetPressedKeys())
                    {
                        if (!allowedKeys.Contains(key) || oldKeyState.GetPressedKeys().Contains(key))
                            continue;
                        shiftDown = isShiftDown(keyState);
                        if (keyStrings.ContainsKey(key) && !(shiftDown && shiftKeys.ContainsKey(key)))
                            currentText += keyStrings[key];
                        else
                        {
                            if (shiftDown && shiftKeys.ContainsKey(key))
                            {
                                currentText += shiftKeys[key];
                            }
                            else
                            {
                                chr = Enum.GetName(typeof(Keys), key);
                                if (!shiftDown)
                                {
                                    chr = chr.ToLower();
                                }
                                currentText += chr;
                            }
                            showCommands();
                        }
                        break; //only need to process one key really
                    }
                }


                if (mouseState.ScrollWheelValue > oldMouseState.ScrollWheelValue)
                    scrollPos++;
                else if (mouseState.ScrollWheelValue < oldMouseState.ScrollWheelValue)
                    scrollPos--;
            }

            oldKeyState = keyState;
            oldMouseState = mouseState;
        }

        private void DeleteLastChar()
        {
            if (currentText.Length > 0)
            {
                historyInd = history.Count;
                try
                {
                    commandList.Clear();
                    currentText = currentText.Substring(0, currentText.Length - 1);
                    showCommands();
                }
                catch { }
            }
        }

        bool isShiftDown(KeyboardState kst)
        {
            return (kst.IsKeyDown(Keys.LeftShift) || kst.IsKeyDown(Keys.RightShift));
        }

        void showCommands()
        {
            commandList = new List<string>();
            if (currentText != "")
                foreach (var command in Game1.commandList)
                {
                    if (command.Contains(currentText))
                    {
                        commandList.Add(command);
                    }
                }
            else commandList.Clear();
        }

        void handleCommand()
        {
            commandList.Clear();
            cursor = "";
            AddText(currentText);

            if (currentText == "quit" || currentText == "exit")
            {
                game.Exit();
                return;
            }
            if (currentText == "clear")
            {
                buffer.Clear();
                AddText("Console Cleared", Color.Orange);
            }
            if (currentText == "hide")
            {
                this.Visible = false;
            }

            if (OnCommand != null)
                OnCommand(this, currentText);

            history.Add(currentText);
            historyInd = history.Count;
            currentText = "";
            cursor = "|";
        }

        Rectangle bgpos = new Rectangle(17, 16, 663, 172);
        ConsoleText ctxt;
        Vector2 txtpos = new Vector2(28, 25);
        Vector2 ctxtpos = new Vector2(28, 165);
        public override void Draw(GameTime gameTime)
        {
            if (spriteBatch == null || spriteBatch.IsDisposed)
                return;

            spriteBatch.Begin();

            spriteBatch.Draw(background, bgpos, Color.White);
            int start = buffer.Count - 9;
            if (start < 0)
                start = 0;

            for (int i = start; i < buffer.Count; i++)
            {
                ctxt = buffer[i];
                spriteBatch.DrawString(ctxt.Font, ctxt.Text, txtpos, ctxt.Color);

                txtpos.Y += ctxt.Font.LineSpacing + 2;
            }
            txtpos.Y = 25;


            if (commandList != null)
            {
                if (commandList.Count <= 5)
                    for (int i = 0; i < commandList.Count; i++)
                    {
                        if (i < 4)
                            spriteBatch.DrawString(ctxt.Font,
                                commandList[i],
                                new Vector2(ctxtpos.X, ctxtpos.Y + ((i + 1) * Game1.arial.MeasureString(commandList[i]).Y)),
                                Color.White);
                        else
                            spriteBatch.DrawString(Game1.arial, "...",
                                   new Vector2(ctxtpos.X, ctxtpos.Y + ((i + 1) * Game1.arial.MeasureString(commandList[i]).Y)),
                                   Color.White);
                    }
                //else
                //{
                //    for (int i = 0; i <= 5; i++)
                //    {
                //        if (i < 5)
                //        {
                //            spriteBatch.DrawString(ctxt.Font,
                //                commandList[i],
                //                new Vector2(ctxtpos.X, ctxtpos.Y + ((i + 1) * Game1.arial.MeasureString(commandList[i]).Y)),
                //                Color.White);
                //        }else
                //spriteBatch.DrawString(Game1.arial, "...",
                //       new Vector2(ctxtpos.X, ctxtpos.Y + ((i + 1) * Game1.arial.MeasureString(commandList[i]).Y)),
                //Color.White);
                //        
                //}
                //}
                //}
            }

            spriteBatch.DrawString(Game1.arial, cursor, new Vector2(ctxtpos.X + (Game1.arial.MeasureString(currentText).X), ctxtpos.Y), Color.White);
            spriteBatch.DrawString(Game1.arial, currentText, ctxtpos, Color.White);

            spriteBatch.End();
        }
    }
}
