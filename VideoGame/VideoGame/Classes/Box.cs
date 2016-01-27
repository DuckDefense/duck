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
        private int maxMonstersHeight = 8;
        private int MaxMonstersPerPage => maxMonstersWidth * maxMonstersHeight;

        private Rectangle boxBounds;
        public List<ContainerButton> Monsters;
        public List<ContainerButton> Party;
        private List<Button> switchButtons;
        private Monster selectedMonster;
        private bool drawStats, drawSwitch;
        public bool Visible;

        public void Access(Character player, Vector2 pos) {
            Party = new List<ContainerButton>();
            Monsters = new List<ContainerButton>();
            CurrentPage = 1;
            Page = 1;
            var index = 0;
            boxBounds = new Rectangle((int)pos.X, (int)pos.Y, 48 * maxMonstersWidth, 48 * maxMonstersHeight);
            var rect = new Rectangle(-48, 0, 24, 24);
            foreach (var mon in player.Box) {
                if (player.Box.Count > MaxMonstersPerPage) {
                    index++;
                    if (index > MaxMonstersPerPage) {
                        rect = new Rectangle(0, 0, 24, 24);
                        Page++;
                    }
                }
                rect = new Rectangle(rect.X += mon.PartySprite.Width, rect.Y, 24, 24);
                if (rect.X == 48 * (maxMonstersWidth)) {
                    rect = new Rectangle(0, rect.Y += mon.PartySprite.Width, 24, 24);
                }
                var boxed = new BoxedMonster(mon, Page);
                Monsters.Add(new ContainerButton(new Button(rect, mon.PartySprite), boxed));
            }
            GetParty(player);
            Visible = true;
        }

        public void Disconnect() {
            Visible = false;
        }

        public void Update(MouseState cur, MouseState prev, GameTime gameTime) {
            if (Visible) {
                foreach (var b in Monsters) {
                    if (b.Button.IsClicked(cur, prev)) {
                        selectedMonster = b.BoxedMonster.Monster;
                        drawStats = true;
                        GetSwitch();
                    }
                    b.BoxedMonster.Monster.Update(gameTime);
                }
                foreach (var b in Party) {
                    b.Monster.Update(gameTime);
                }
            }
        }

        public void Draw(SpriteBatch batch) {
            if (Visible) {
                //Draw the box page
                batch.Draw(ContentLoader.Health, boxBounds, Color.Red);
                foreach (var con in Monsters.Where(con => con.BoxedMonster.Page == CurrentPage)) {
                    //batch.Draw(con.BoxedMonster.Monster.PartySprite, con.Button.VectorPosition, Color.White);

                    batch.Draw(con.BoxedMonster.Monster.PartySprite, con.Button.VectorPosition, null, con.BoxedMonster.Monster.SourceRectangle, null,0f, new Vector2(2,2), Color.White);
                }

                if (drawStats) {
                    //Draw it beneath the box
                    Drawer.DrawMonsterInfo(batch, selectedMonster,
                        new Vector2(boxBounds.X, boxBounds.Height + ContentLoader.MonsterViewer.Height + 32));
                }
                if (drawSwitch) {
                    //Ask if the player would like to switch it out with any of the monsters in the party, or an empty spot
                    DrawSwitch(batch);
                }
                DrawParty(batch);
            }
        }

        private void GetSwitch() {
            switchButtons = new List<Button>();
            int index = 0;
            var rect = new Rectangle(boxBounds.Width + 24, 24, 32, 32);
            foreach (var mon in Party) {
                index++;
                switchButtons.Add(new Button(new Rectangle(rect.X, rect.Y += 24, rect.Height, rect.Width), ContentLoader.Button, ContentLoader.ButtonHover, ContentLoader.ButtonClicked,
                    $"{index + 1}: {mon.Monster.Name}", ContentLoader.Arial));
            }
        }

        private void DrawSwitch(SpriteBatch batch) {
            foreach (var btn in switchButtons) {
                btn.Draw(batch);
            }
        }


        private void DrawParty(SpriteBatch batch) {
            foreach (var con in Party) {
                batch.Draw(con.Monster.PartySprite, con.Button.VectorPosition, null, con.Monster.SourceRectangle, null, 0f, new Vector2(2, 2), Color.White);
            }
        }

        private void GetParty(Character player) {
            var rect = new Rectangle(boxBounds.Width + 48, 24, 24, 24);
            foreach (var mon in player.Monsters) {
                var button = new Button(new Rectangle(rect.X, rect.Y += 24, 24, 24), mon.PartySprite);
                Party.Add(new ContainerButton(button, mon));
            }
        }
    }
}
