using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Sandbox.Classes.UI;
using VideoGame.Classes;
using Keys = Microsoft.Xna.Framework.Input.Keys;

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
        private List<ContainerButton> partyList = new List<ContainerButton>();
        private List<ContainerButton> movesList = new List<ContainerButton>();
        private List<ContainerButton> itemList = new List<ContainerButton>();
        private List<ContainerButton> captureList = new List<ContainerButton>();
        private List<ContainerButton> medicineList = new List<ContainerButton>();
        private List<Button> PartyMenuList = new List<Button>();

        private bool drawPartyMenu, drawStatus, drawMoves, drawMove;
        private bool movable;
        private Button moveButton;
        private Button statusButton;
        private Button movesButton;

        public Button KnownMonstersButton, PartyButton, ItemButton, PlayerButton, SaveButton, MuteButton;
        private Button generalItems, medicineItems, captureItems;
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
                if (player.Monsters.Count >= partyList.Count) GetPartyMonsters();
                    foreach (var b in PartyMenuList) {
                        b.Update(curMouseState, prevMouseState);
                    }
                break;
            case Selection.Item:
                if (player.Inventory.Items.Count >= itemList.Count) GetItems();
                if (player.Inventory.Captures.Count >= captureList.Count) GetCapture();
                if (player.Inventory.Medicine.Count >= medicineList.Count) GetMedicine();
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
                    MuteButton.Text = "Mute";
                }
                Selection = Selection.None;
                break;
            }
        }

        public void Draw(SpriteBatch batch, MouseState curMouseState, MouseState prevMouseState) {
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
                DrawParty(batch);
                foreach (var con in partyList) {
                    //Try this with a click instead hover, also find out why IsClicked doesn't work anymore
                    if (con.Button.IsHovering(curMouseState)) {
                        GetPartyMenu(con);
                        DrawPartyMenu(batch);
                        if (statusButton.IsHeld(curMouseState)) {
                            ResetDraws();
                            drawStatus = true;
                        }
                        else if (moveButton.IsHeld(curMouseState)) {
                            ResetDraws();
                            drawMove = true;
                            movable = true;
                        }
                        else if (movesButton.IsHeld(curMouseState)) {
                            ResetDraws();
                            drawMoves = true;
                        }
                        //else ResetDraws();
                        if (drawStatus) Drawer.DrawMonsterInfo(batch, con.Monster);
                        if(drawMove) { }
                        if (drawMoves) {
                            GetMoves(con);
                            DrawMoves(batch);
                        }
                    }
                }
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

        private void ResetDraws() {
            drawStatus = false;
            drawMoves = false;
            drawMove = false;
            movable = false;

        }

        private void Toggle(KeyboardState cur, KeyboardState prev) {
            if (cur.IsKeyDown(Keys.Enter) && prev.IsKeyUp(Keys.Enter)) {
                Visible = !Visible;
                Selection = Selection.None;
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

        private void GetPartyMonsters() {
            for (int i = 0; i < player.Monsters.Count; i++) {
                var m = player.Monsters[i];
                var pos = new Rectangle((int)position.X - 192, (int)position.Y, m.FrontSprite.Width / 2, m.FrontSprite.Height / 2);
                pos = i % 2 == 0
                    ? new Rectangle(pos.X - 99, pos.Y + (pos.Height * (i)), pos.Width * 3, pos.Height * 2)
                    : new Rectangle(pos.X - (ContentLoader.Button.Width - (111)), pos.Y + (pos.Height * (i - 1)), pos.Width * 3, pos.Height * 2);
                partyList.Add(new ContainerButton(new Button(pos, ContentLoader.Button), m));
            }
        }

        private void GetPartyMenu(ContainerButton con) {
            PartyMenuList.Clear();
            var pos = new Rectangle(con.Button.Position.X - 1, con.Button.Position.Y, ContentLoader.Button.Width, ContentLoader.Button.Height);
            moveButton = new Button(new Rectangle(pos.X, pos.Y, pos.Width, pos.Height), ContentLoader.Button, ContentLoader.ButtonHover, ContentLoader.ButtonClicked, "Move", ContentLoader.Arial);
            statusButton = new Button(new Rectangle(pos.X, pos.Y + (pos.Height), pos.Width, pos.Height), ContentLoader.Button, ContentLoader.ButtonHover, ContentLoader.ButtonClicked, "Status", ContentLoader.Arial);
            movesButton = new Button(new Rectangle(pos.X, pos.Y + (pos.Height * 2), pos.Width, pos.Height), ContentLoader.Button, ContentLoader.ButtonHover, ContentLoader.ButtonClicked, "Moves", ContentLoader.Arial);

            PartyMenuList.AddMany(moveButton, statusButton, movesButton);
        }

        private void GetMoves(ContainerButton con) {
            movesList.Clear();
            for (int i = 0; i < con.Monster.Moves.Count; i++) {
                var move = con.Monster.Moves[i];
                var pos = new Rectangle(0, 0, 64, 64);
                pos = new Rectangle(pos.X, pos.Y + (pos.Height*(i)), pos.Width*2, pos.Height*2);
                var b = new Button(pos, ContentLoader.Health, ContentLoader.Health, ContentLoader.Health, move.Name, ContentLoader.Arial);
                movesList.Add(new ContainerButton(b, move));
            }

        }

        private void GetItems() {
            foreach (var item in player.Inventory.Items.Values) {
                var pos = new Rectangle((int)position.X - 288, (int)position.Y, item.Sprite.Width / 2, item.Sprite.Height / 2);

            }
        }
        private void GetCapture() { }
        private void GetMedicine() { }
        private void DrawKnownMonsters(SpriteBatch batch) {
            batch.Draw(ContentLoader.Health, new Rectangle(0, 0, (96 * 7) + 1, (96 * 5) + 1), Color.DarkRed);
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
            foreach (var con in partyList) {
                var pos = con.Button.Position;
                var m = con.Monster;
                batch.Draw(ContentLoader.Health, new Rectangle(pos.X - 1, pos.Y - 1, (pos.Width) + 2, pos.Height + 2), Color.IndianRed);
                batch.Draw(m.FrontSprite, new Vector2(pos.X +47, pos.Y), Color.White);
            }
        }
        private void DrawPartyMenu(SpriteBatch batch) {
            foreach (var b in PartyMenuList) {
                b.Draw(batch);
            }
        }

        private void DrawMoves(SpriteBatch batch) {
            foreach (var con in movesList) {
                var move = con.Move;
                var typeTexture = Drawer.GetTypeTexture(move.Type);
                batch.Draw(ContentLoader.Health, con.Button.Position, Color.Crimson);
                batch.Draw(typeTexture, new Vector2(con.Button.Position.X, con.Button.Position.Y + 18), Color.White);
                batch.DrawString(ContentLoader.Arial, move.Name, new Vector2(con.Button.Position.X, con.Button.Position.Y), Color.Black);
                batch.DrawString(ContentLoader.Arial, move.Description, new Vector2(con.Button.Position.X, con.Button.Position.Y + 32), Color.Black);
                batch.DrawString(ContentLoader.Arial, $"{move.Uses}/{move.MaxUses}", new Vector2(con.Button.Position.X, con.Button.Position.Y + (32 + typeTexture.Height)), Color.Black);
            }
        }

        private void DrawItem(SpriteBatch batch) {
            var pos = new Rectangle((int)position.X + ContentLoader.Button.Width, (int)position.Y, ContentLoader.Button.Width, ContentLoader.Button.Height);
            generalItems = new Button(new Rectangle(pos.X, pos.Y, pos.Width, pos.Height), ContentLoader.Button, ContentLoader.ButtonHover, ContentLoader.ButtonClicked, "General", ContentLoader.Arial);
            captureItems = new Button(new Rectangle(pos.X, pos.Y, pos.Width, pos.Height), ContentLoader.Button, ContentLoader.ButtonHover, ContentLoader.ButtonClicked, "Capture", ContentLoader.Arial);
            medicineItems = new Button(new Rectangle(pos.X, pos.Y, pos.Width, pos.Height), ContentLoader.Button, ContentLoader.ButtonHover, ContentLoader.ButtonClicked, "Medicine", ContentLoader.Arial);
        }

        private void DrawPlayer(SpriteBatch batch) {

        }

        private void DrawSave(SpriteBatch batch) {

        }
    }
}
