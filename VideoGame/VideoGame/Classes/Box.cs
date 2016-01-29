using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sandbox.Classes.UI;
using VideoGame.Classes;

namespace Sandbox.Classes {
    public class BoxedMonster {
        public int Page;
        public Monster Monster;

        public BoxedMonster(Monster monster, int page) {
            Monster = monster;
            Page = page;
        }
    }

    public class Box {
        private int Page;
        public int CurrentPage;
        private int maxMonstersWidth = 6;
        private int maxMonstersHeight = 5;
        private int MaxMonstersPerPage => maxMonstersWidth * maxMonstersHeight;

        private Rectangle boxBounds;
        private Character player;
        public List<ContainerButton> Monsters = new List<ContainerButton>();
        public List<ContainerButton> Party = new List<ContainerButton>();
        private List<Button> switchButtons;
        private ContainerButton Selected;
        private ContainerButton ToBeMoved;
        private bool drawStats, drawSelected;
        public bool Visible;

        public void Access(Character player, Vector2 pos) {
            this.player = player;
            CurrentPage = 1;
            Page = 1;
            var index = 0;
            boxBounds = new Rectangle((int)pos.X, (int)pos.Y, 48 * maxMonstersWidth, 48 * maxMonstersHeight);
            if (player.Box.Count != Monsters.Count) {
                Party = new List<ContainerButton>();
                Monsters = new List<ContainerButton>();
                var rect = new Rectangle(-48, 0, 48, 48);
                foreach (var mon in player.Box) {
                    if (player.Box.Count > MaxMonstersPerPage) {
                        index++;
                        if (index > MaxMonstersPerPage) {
                            rect = new Rectangle(0, 0, 48, 48);
                            Page++;
                        }
                    }
                    rect = new Rectangle(rect.X += mon.PartySprite.Width, rect.Y, 48, 48);
                    if (rect.X == 48 * (maxMonstersWidth)) {
                        rect = new Rectangle(0, rect.Y += mon.PartySprite.Width, 48, 48);
                    }
                    var boxed = new BoxedMonster(mon, Page);
                    Monsters.Add(new ContainerButton(new Button(rect, mon.PartySprite), boxed));
                }
            }
            GetParty(player);
            Visible = true;
        }

        public void Disconnect() {
            Visible = false;
        }

        public void Update(MouseState cur, MouseState prev, GameTime gameTime) {
            if (Visible) {
                for (int i = 0; i < Monsters.Count; i++) {
                    var b = Monsters[i];
                    if (b.Button.IsClicked(cur, prev)) {
                        if (Selected != null) {
                            if (Selected == b) Selected = null;
                            else {
                                ToBeMoved = b;
                                if (Monsters.Contains(Selected)) { MoveMonster(false); }
                                else { MoveMonsterToList(false); }
                                Selected = null;
                                ToBeMoved = null;
                            }
                        }
                        else { Selected = b; }
                        drawStats = true;
                    }
                    b.BoxedMonster.Monster.Update(gameTime);
                }
                for (int i = 0; i < Party.Count; i++) {
                    var p = Party[i];
                    if (p.Button.IsClicked(cur, prev)) {
                        if (Selected != null) {
                            if (Selected == p) Selected = null;
                            else {
                                ToBeMoved = p;
                                if (Party.Contains(Selected)) { MoveMonster(true); }
                                else { MoveMonsterToList(true); }
                                Selected = null;
                                ToBeMoved = null;
                            }
                        }
                        else { Selected = p; }
                        drawStats = true;
                    }
                    p.Monster.Update(gameTime);
                }
            }
        }

        public void Draw(SpriteBatch batch) {
            if (Visible) {
                //Draw the box page
                batch.Draw(ContentLoader.Health, boxBounds, Color.Red);
                foreach (var con in Monsters.Where(con => con.BoxedMonster.Page == CurrentPage)) {
                    //batch.Draw(con.BoxedMonster.Monster.PartySprite, con.Button.VectorPosition, Color.White);

                    batch.Draw(con.BoxedMonster.Monster.PartySprite, con.Button.VectorPosition, null, con.BoxedMonster.Monster.SourceRectangle, null, 0f, new Vector2(2, 2), Color.White);
                }
                DrawParty(batch);

                if (drawStats) {
                    //Draw it beneath the box
                    if (Selected != null)
                        Drawer.DrawMonsterInfo(batch, Selected.BoxedMonster != null ? Selected.BoxedMonster.Monster : Selected.Monster, new Vector2(0, boxBounds.Height));
                }
            }
        }

        private void MoveMonster(bool box) {
            var selectedId = 0;
            var toBeMovedId = 0;
            if (!box) {
                selectedId = Monsters.IndexOf(Selected);
                toBeMovedId = Monsters.IndexOf(ToBeMoved);
                player.Box.MoveItem(toBeMovedId, selectedId);
            }
            else {
                selectedId = Party.IndexOf(Selected);
                toBeMovedId = Party.IndexOf(ToBeMoved);
                player.Monsters.MoveItem(toBeMovedId, selectedId);
            }
            Monsters.Clear();
            Party.Clear();
            Access(player, new Vector2(0, 0));
        }

        private void MoveMonsterToList(bool box) {
            int selectedId;
            int toBeMovedId;
            if (box) {
                selectedId = Monsters.IndexOf(Selected);
                toBeMovedId = Party.IndexOf(ToBeMoved);
                player.Monsters.MoveToList(player.Box, selectedId, toBeMovedId);
            }
            else {
                selectedId = Party.IndexOf(Selected);
                toBeMovedId = Monsters.IndexOf(ToBeMoved);
                player.Monsters.MoveToList(player.Box, toBeMovedId, selectedId);
            }
            Monsters.Clear();
            Party.Clear();
            Access(player, new Vector2(0, 0));
        }

        private void GetParty(Character player) {
            var rect = new Rectangle(boxBounds.Width, -48, 48, 48);
            foreach (var mon in player.Monsters) {
                var button = new Button(new Rectangle(rect.X, rect.Y += 48, 48, 48), mon.PartySprite);
                Party.Add(new ContainerButton(button, mon));
            }
        }

        private void DrawParty(SpriteBatch batch) {
            foreach (var con in Party) {
                batch.Draw(con.Monster.PartySprite, con.Button.VectorPosition, null, con.Monster.SourceRectangle, null, 0f, new Vector2(2, 2), Color.White);
            }
        }

    }
}
