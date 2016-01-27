using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sandbox.Classes;
using Sandbox.Classes.UI;

namespace VideoGame.Classes {
    public class Shop {
        public Character ShopKeeper;
        private Character player;
        public string Name;
        private List<ContainerButton> shopButtonList, playerButtonList;
        private Button buyButton;
        private Button sellButton;
        private Button closeButton;
        private bool drawSell, drawBuy;

        public Medicine SelectedMedicine;
        public Capture SelectedCapture;


        public Shop(string shopName, Character guy, Character player)
        {
            Name = shopName;
            ShopKeeper = guy;
            this.player = player;
            GetItems(player);
            var buttonPos = 0;
            Rectangle rec = new Rectangle(buttonPos - ContentLoader.Button.Width, ContentLoader.GrassyBackground.Height, ContentLoader.Button.Width, ContentLoader.Button.Height);

            buyButton = new Button(new Rectangle(rec.X += ContentLoader.Button.Width, rec.Y, rec.Width, rec.Height), ContentLoader.Button, "Buy", ContentLoader.Arial);
            sellButton = new Button(new Rectangle(rec.X += ContentLoader.Button.Width, rec.Y, rec.Width, rec.Height), ContentLoader.Button, "Sell", ContentLoader.Arial);
            closeButton = new Button(new Rectangle(rec.X += ContentLoader.Button.Width, rec.Y, rec.Width, rec.Height), ContentLoader.Button, "Leave", ContentLoader.Arial);
        }


        public void GetItems(Character player)
        {
            shopButtonList = new List<ContainerButton>();
            playerButtonList = new List<ContainerButton>();
            StoreItemsInList(shopButtonList, ShopKeeper);
            StoreItemsInList(playerButtonList, player);
        }

        public void StoreItemsInList(List<ContainerButton> list, Character character)
        {
            Rectangle rect = new Rectangle(0, 0, 0, 0);
            int index = 1;
            foreach (var cap in character.Inventory.Captures.Values)
            {
                index++;
                var btn = new Button(new Rectangle(rect.X * index, rect.Y, cap.Sprite.Width, cap.Sprite.Height), cap.Sprite);
                list.Add(new ContainerButton(btn, cap));
            }
            rect = new Rectangle(0, 0, 0, 0);
            index = 1;
            foreach (var med in character.Inventory.Medicine.Values)
            {
                index++;
                var btn = new Button(new Rectangle(rect.X * index, rect.Y, med.Sprite.Width, med.Sprite.Height), med.Sprite);
                list.Add(new ContainerButton(btn, med));
            }
        }

        public void Draw(SpriteBatch batch, Character player)
        {
            buyButton.Draw(batch);
            sellButton.Draw(batch);
            if(drawBuy) DrawBuy(batch);
            if(drawSell) DrawSell(batch, player);
        }

        public void Update(MouseState cur, MouseState prev)
        {
            if (buyButton.IsClicked(cur, prev))
            {
                Buy();
                drawBuy = true;
                drawSell = false;
            }
            if (sellButton.IsClicked(cur, prev))
            {
                Sell();
                drawSell = true;
                drawBuy = false;
            }
            Drawer.UpdateItemButtons(cur, prev);
        }

        public void Buy() {
            if (Drawer.DrawMedicine) {
                foreach (var m in ShopKeeper.Inventory.Medicine) {
                    if(Drawer.GetClickedButton() != null)
                    if (m.Value.Name == Drawer.GetClickedButton().Text) {
                        SelectedMedicine = m.Value;
                    }
                }
            }
            if (Drawer.DrawCapture) {
                foreach (var m in ShopKeeper.Inventory.Captures) {
                    if(Drawer.GetClickedButton() != null)
                    if (m.Value.Name == Drawer.GetClickedButton().Text) {
                        SelectedCapture = m.Value;
                    }
                }
            }
        }
        public void Sell() {
            if (Drawer.DrawMedicine) {
                foreach (var m in player.Inventory.Medicine) {
                    if(Drawer.GetClickedButton() != null)
                    if (m.Value.Name == Drawer.GetClickedButton().Text) {
                        SelectedMedicine = m.Value;
                    }
                }
            }
            if (Drawer.DrawCapture) {
                foreach (var m in player.Inventory.Captures) {
                    if(Drawer.GetClickedButton() != null)
                    if (m.Value.Name == Drawer.GetClickedButton().Text) {
                        SelectedCapture = m.Value;
                    }
                }
            }
        }

        public void DrawBuy(SpriteBatch batch) {
            Drawer.DrawInventory(batch, ShopKeeper);
        }

        public void DrawSell(SpriteBatch batch, Character player) {
            Drawer.DrawInventory(batch, player);
        }
    }

}
