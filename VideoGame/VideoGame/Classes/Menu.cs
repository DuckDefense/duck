using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Sandbox.Classes.UI;
using VideoGame.Classes;

namespace Sandbox.Classes {
    public enum Selection {
        None,
        KnownMonsters,
        Party,
        Item,
        Player,
        Save,
        Mute
    }
    public class Menu {
        public Selection Selection;
        private Vector2 position;
        private Character player;
        public bool Visible = false;

        private List<ContainerButton> knownMonsterList = new List<ContainerButton>();

        public bool DrawMonsterInfo;
        public Button KnownMonstersButton, PartyButton, ItemButton, PlayerButton, SaveButton, MuteButton;
        public List<Button> ButtonList;

        public Menu(Character player, Vector2 pos) {
            ButtonList = new List<Button>();
            var buttonTexture = ContentLoader.Button;
            var hoverTexture = ContentLoader.ButtonHover;
            var clickedTexture = ContentLoader.ButtonClicked;
            var width = buttonTexture.Width;
            var height = buttonTexture.Height;
            this.player = player;
            position = pos;
            KnownMonstersButton = new Button(new Rectangle((int)pos.X, (int)pos.Y, width, height), buttonTexture, hoverTexture, clickedTexture, "Monsters", ContentLoader.Arial);
            PartyButton = new Button(new Rectangle((int)pos.X, (int)pos.Y + height, width, height), buttonTexture, hoverTexture, clickedTexture, "Party", ContentLoader.Arial);
            ItemButton = new Button(new Rectangle((int)pos.X, (int)pos.Y + (height * 2), width, height), buttonTexture, hoverTexture, clickedTexture, "Item", ContentLoader.Arial);
            PlayerButton = new Button(new Rectangle((int)pos.X, (int)pos.Y + (height * 3), width, height), buttonTexture, hoverTexture, clickedTexture, player.Name, ContentLoader.Arial);
            SaveButton = new Button(new Rectangle((int)pos.X, (int)pos.Y + (height * 4), width, height), buttonTexture, hoverTexture, clickedTexture, "Save", ContentLoader.Arial);
            MuteButton = new Button(new Rectangle((int)pos.X, (int)pos.Y + (height * 5), width, height), buttonTexture, hoverTexture, clickedTexture, "Mute", ContentLoader.Arial);

            ButtonList.AddMany(KnownMonstersButton, PartyButton, ItemButton, PlayerButton, SaveButton, MuteButton);
        }

        public void Update(GameTime gameTime, MouseState curMouseState, MouseState prevMouseState, KeyboardState curKeyboardState, KeyboardState prevKeyboardState) {
            Toggle(curKeyboardState, prevKeyboardState);
            if (!Visible) return;
            foreach (var button in ButtonList) button.Update(curMouseState, prevMouseState);

            if (KnownMonstersButton.IsClicked(curMouseState, prevMouseState)) Selection = Selection.KnownMonsters;
            if (PartyButton.IsClicked(curMouseState, prevMouseState)) Selection = Selection.Party;
            if (ItemButton.IsClicked(curMouseState, prevMouseState)) Selection = Selection.Item;
            if (PlayerButton.IsClicked(curMouseState, prevMouseState)) Selection = Selection.Player;
            if (SaveButton.IsClicked(curMouseState, prevMouseState)) Selection = Selection.Save;
            if (MuteButton.IsClicked(curMouseState, prevMouseState)) Selection = Selection.Mute;

            switch (Selection) {
            case Selection.KnownMonsters:
                if (player.KnownMonsters.Count >= knownMonsterList.Count) GetKnownMonsters();
                break;
            case Selection.Party:
                break;
            case Selection.Item:
                break;
            case Selection.Player:
                break;
            case Selection.Save:
                break;
            case Selection.Mute:
                if (SoundEffect.MasterVolume != 0f) {
                    SoundEffect.MasterVolume = 0f;
                    MuteButton.Text = "Unmute";
                }
                else {
                    SoundEffect.MasterVolume = 1f;
                }
                Selection = Selection.None;
                break;
            }
        }

        private void Toggle(KeyboardState cur, KeyboardState prev) {
            if (cur.IsKeyDown(Keys.Enter) && prev.IsKeyUp(Keys.Enter)) {
                Visible = !Visible;
                Selection = Selection.None;
            }
        }
        
        public void Draw(SpriteBatch batch, MouseState curMouseState) {
            if (!Visible) return;
            foreach (var button in ButtonList) {
                button.Draw(batch);
            }
            switch (Selection) {
            case Selection.KnownMonsters:
                DrawKnownMonsters(batch);
                foreach (var con in knownMonsterList) {
                    if (con.Button.IsHovering(curMouseState)) Drawer.DrawMonsterInfo(batch, con.Monster, true); 
                }
                break;
            case Selection.Party:
                break;
            case Selection.Item:
                break;
            case Selection.Player:
                break;
            case Selection.Save:
                break;
            case Selection.Mute:
                break;
            }
        }

        private void GetKnownMonsters() {
            var pos = new Vector2(-96, 0);
            foreach (var m in player.KnownMonsters.Values) {
                //Draw monsters from left to right up to the point where no other fits there
                if (pos.X + (m.FrontSprite.Width * 2) >= Settings.ResolutionWidth) {
                    pos.X = 0;
                    pos.Y += m.FrontSprite.Height;
                }
                pos.X += m.FrontSprite.Width;
                var b = new Button(new Rectangle((int)pos.X, (int)pos.Y, m.FrontSprite.Width, m.FrontSprite.Height), m.FrontSprite);
                var c = new ContainerButton(b, m);
                knownMonsterList.Add(c);
            }
        }

        private void DrawKnownMonsters(SpriteBatch batch) {
            batch.Draw(ContentLoader.Health, new Rectangle(0, 0, (96 * 7) + 1, (96 * 7) + 1), Color.DarkRed);
            foreach (var con in knownMonsterList) {
                if (con.Monster.Id <= 7) {
                    var rect = new Rectangle(((con.Monster.Id - 1) * 96), 0, 96, 96);
                    batch.Draw(ContentLoader.Health, new Rectangle(rect.X - 1, rect.Y - 1, rect.Width + 2, rect.Height + 2), Color.Brown);
                    batch.Draw(con.Monster.FrontSprite, rect, Color.White);
                    con.Button.Position = rect;
                }
                else if (con.Monster.Id <= 14) {
                    var rect = new Rectangle(((con.Monster.Id - 8) * 96), 96, 96, 96);
                    batch.Draw(ContentLoader.Health, new Rectangle(rect.X - 1, rect.Y - 1, rect.Width + 2, rect.Height + 2), Color.Brown);
                    batch.Draw(con.Monster.FrontSprite, rect, Color.White);
                    con.Button.Position = rect;
                }
                else if (con.Monster.Id <= 21) {
                    var rect = new Rectangle(((con.Monster.Id - 15) * 96), 96 * 2, 96, 96);
                    batch.Draw(ContentLoader.Health, new Rectangle(rect.X - 1, rect.Y - 1, rect.Width + 2, rect.Height + 2), Color.Brown);
                    batch.Draw(con.Monster.FrontSprite, rect, Color.White);
                    con.Button.Position = rect;
                }
                else if (con.Monster.Id <= 28) {
                    var rect = new Rectangle(((con.Monster.Id - 22) * 96), 96 * 3, 96, 96);
                    batch.Draw(ContentLoader.Health, new Rectangle(rect.X - 1, rect.Y - 1, rect.Width + 2, rect.Height + 2), Color.Brown);
                    batch.Draw(con.Monster.FrontSprite, rect, Color.White);
                    con.Button.Position = rect;
                }
            }
        }

        private void DrawParty(SpriteBatch batch) {

        }

        private void DrawItem(SpriteBatch batch) {

        }

        private void DrawPlayer(SpriteBatch batch) {

        }

        private void DrawSave(SpriteBatch batch) {

        }
    }
}
