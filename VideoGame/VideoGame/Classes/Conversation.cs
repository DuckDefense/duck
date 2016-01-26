using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using VideoGame.Classes;

namespace Sandbox.Classes {
    public class Conversation {

        public class Message {
            public List<string> Lines;
            public string CurrentLine;
            public int CurrentIndex;
            public SpriteFont Font;
            public Character Character;

            public Vector2 Position;
            public Vector2 NamePosition;

            public Color Color;
            public Texture2D SourceTexture;
            public bool Visible;
            public bool Said;

            public Message(List<string> lines, Color color, Character character) {
                Lines = lines;
                CurrentIndex = 0;
                CurrentLine = Lines[CurrentIndex];
                Color = color;
                Font = ContentLoader.Arial;
                SourceTexture = ContentLoader.MessageBox;
                Position = new Vector2(0, Settings.ResolutionHeight - SourceTexture.Height);
                Character = character;
                Visible = false;
            }

            public void Update(KeyboardState curKey, KeyboardState prevKey, Character player) {
                if (Visible) {
                    player.Talking = true;
                    Character.Talking = true;
                    if (curKey.IsKeyDown(Settings.conversation) && prevKey.IsKeyUp(Settings.conversation)) {
                        if (CurrentIndex < Lines.Count - 1) {
                            CurrentIndex++;
                            CurrentLine = Lines[CurrentIndex];
                        }
                        else {
                            Visible = false;
                            Said = true;
                        }
                    }
                }
                else {
                    player.Talking = false;
                    Character.Talking = false;
                }
            }

            public void Draw(SpriteBatch batch) {
                batch.Draw(SourceTexture, Position, Color.White);
                var namesize = Font.MeasureString(Character.Name);
                var nameTextureSize = new Rectangle(13, 0, 136, 27);

                //Set name in the middle of the name thing 136 x 27
                batch.DrawString(Font, CurrentLine, new Vector2(8, Settings.ResolutionHeight - (SourceTexture.Height - 32)), Color);
                NamePosition = new Vector2(nameTextureSize.X + ((nameTextureSize.Width - namesize.X) / 2), Position.Y + 6);
                batch.DrawString(Font, Character.Name, NamePosition, Color.Black);
            }
        }

        public class BattleMessage {
            public List<string> Lines;
            public string CurrentLine;
            public int CurrentIndex;
            public SpriteFont Font;

            public Vector2 Position;

            public Color Color;
            public Texture2D SourceTexture;
            public bool Visible;
            public bool Said;

            public BattleMessage(List<string> lines, Color color) {
                Lines = lines;
                CurrentIndex = 0;
                CurrentLine = Lines[CurrentIndex];
                Color = color;
                Font = ContentLoader.Arial;
                SourceTexture = ContentLoader.BattleMessage;
                Position = new Vector2(0, Settings.ResolutionHeight - SourceTexture.Height);
                Visible = true;
            }

            public void Update(KeyboardState curKey, KeyboardState prevKey, Character player) {
                if (Visible) {
                    player.Talking = true;
                    if (curKey.IsKeyDown(Settings.conversation) && prevKey.IsKeyUp(Settings.conversation)) {
                        if (CurrentIndex < Lines.Count - 1) {
                            CurrentIndex++;
                            CurrentLine = Lines[CurrentIndex];
                        }
                        else {
                            Visible = false;
                            Said = true;
                        }
                    }
                }
                else {
                    player.Talking = false;
                }
            }

            public void Draw(SpriteBatch batch) {
                if (Visible) {
                    batch.Draw(SourceTexture, Position, Color.White);
                    batch.DrawString(Font, CurrentLine,
                        new Vector2(8, Settings.ResolutionHeight - (SourceTexture.Height - 32)), Color);
                }
            }
        }

        public class Choice {

        }
    }
}
