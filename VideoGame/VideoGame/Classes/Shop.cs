using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sandbox.Classes;
using Sandbox.Classes.UI;

namespace VideoGame.Classes
{
    public class Shop
    {
        public Character ShopKeeper;
        private Character player;
        public string Name;
        private List<ContainerButton> shopCaptureButtons, shopMedicineButtons, playerCaptureButtons, playerMedicineButtons;
        private Button buyButton;
        private Button sellButton;
        private Button closeButton;
        private Button yesButton;
        private Button NoButton;
        private bool drawSellConfirm, drawBuyConfirm;
        private bool drawSell, drawBuy;
        private Conversation.Message sell, buy;

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
            shopCaptureButtons = new List<ContainerButton>();
            shopMedicineButtons = new List<ContainerButton>();
            playerCaptureButtons = new List<ContainerButton>();
            playerMedicineButtons = new List<ContainerButton>();
            StoreItemsInList(ShopKeeper);
            StoreItemsInList(player);
        }

        public void StoreItemsInList(Character character)
        {
            Rectangle rec = new Rectangle(0 - ContentLoader.Button.Width, ContentLoader.GrassyBackground.Height, ContentLoader.Button.Width, ContentLoader.Button.Height);

            int index = 1;
            foreach (var cap in character.Inventory.Captures.Values)
            {
                index++;
                //new Rectangle(rec.X += ContentLoader.Button.Width, rec.Y + (ContentLoader.Button.Height * 2), rec.Width, rec.Height
                var btn = new Button(new Rectangle(rec.X += ContentLoader.Button.Width, rec.Y + (ContentLoader.Button.Height * 2), rec.Width, rec.Height), cap.Sprite);
                if (character.Name == ShopKeeper.Name)
                    shopCaptureButtons.Add(new ContainerButton(btn, cap));
                else
                {
                    playerCaptureButtons.Add(new ContainerButton(btn, cap));
                }
            }
            rec = new Rectangle(0 - ContentLoader.Button.Width, ContentLoader.GrassyBackground.Height, ContentLoader.Button.Width, ContentLoader.Button.Height);
            index = 1;
            foreach (var med in character.Inventory.Medicine.Values)
            {
                index++;
                var btn = new Button(new Rectangle(rec.X += ContentLoader.Button.Width, rec.Y + (ContentLoader.Button.Height * 2), rec.Width, rec.Height), med.Sprite);
                if (character.Name == ShopKeeper.Name)
                    shopMedicineButtons.Add(new ContainerButton(btn, med));
                else
                {
                    playerMedicineButtons.Add(new ContainerButton(btn, med));
                }
            }
        }

        public void Draw(SpriteBatch batch, Character player)
        {
            buyButton.Draw(batch);
            sellButton.Draw(batch);
            if (drawBuy)
            {
                GetItems(player);
                DrawBuy(batch);
            }
            if (drawSell) { 
                GetItems(player);
                DrawSell(batch, player);
                }
            if (SelectedCapture != null || SelectedMedicine != null)
            {
                if (drawSell)
                {
                    sell.Draw(batch);
                }
                if (drawBuy)
                {
                    buy.Draw(batch);
                }
            }
            if (drawSellConfirm)
            {
                yesButton.Draw(batch);
                NoButton.Draw(batch);
            }
            if (drawBuyConfirm)
            {
                yesButton.Draw(batch);
                NoButton.Draw(batch);
            }
        }

        public void Update(MouseState cur, MouseState prev)
        {
            if (buyButton.IsClicked(cur, prev))
            {
                Buy(cur, prev);
                drawBuy = true;
                drawSell = false;
            }
            if (sellButton.IsClicked(cur, prev))
            {
                Sell(cur, prev);
                drawSell = true;
                drawBuy = false;
            }
            if (drawSell) Sell(cur, prev);
            if(drawBuy)Buy(cur,prev);
            if (SelectedCapture != null && SelectedCapture.Amount > 0 || SelectedMedicine != null && SelectedMedicine.Amount > 0)
            {
                if (drawSell)
                {
                    List<string> selllines = new List<string>
                        {
                            "Are you sure you want to sell this?"
                        };
                    drawSellConfirm = true;
                    sell = new Conversation.Message(selllines, Color.Black, ShopKeeper);
                    Rectangle rec = new Rectangle(0, ContentLoader.GrassyBackground.Height + (ContentLoader.Button.Height * 2), ContentLoader.Button.Width, ContentLoader.Button.Height);
                    yesButton = new Button(new Rectangle(rec.X += ContentLoader.Button.Width,rec.Y + (ContentLoader.Button.Height * 4), rec.Width, rec.Height),ContentLoader.Button, "Yes", ContentLoader.Arial);
                    NoButton =new Button(new Rectangle(rec.X += ContentLoader.Button.Width,rec.Y + (ContentLoader.Button.Height * 4), rec.Width, rec.Height),ContentLoader.Button, "No", ContentLoader.Arial);
                }
                if (drawBuy)
                {
                    List<string> buylines ; /*= new List<string>*/
                    buylines = SelectedMedicine != null 
                        ? new List<string> { $"Are you sure you want to buy this? It costs {SelectedMedicine.Worth}" } 
                        : new List<string>{$"Are you sure you want to buy this? It costs {SelectedCapture.Worth}"};
                    
                    drawBuyConfirm = true;
                    buy = new Conversation.Message(buylines, Color.Black, ShopKeeper);
                    Rectangle rec = new Rectangle(0,ContentLoader.GrassyBackground.Height + (ContentLoader.Button.Height * 2),ContentLoader.Button.Width, ContentLoader.Button.Height);
                    yesButton =new Button(new Rectangle(rec.X += ContentLoader.Button.Width,rec.Y + (ContentLoader.Button.Height * 4), rec.Width, rec.Height),ContentLoader.Button, "Yes", ContentLoader.Arial);
                    NoButton =new Button(new Rectangle(rec.X += ContentLoader.Button.Width,rec.Y + (ContentLoader.Button.Height * 4), rec.Width, rec.Height),ContentLoader.Button, "No", ContentLoader.Arial);
                }
            }
            if (drawSellConfirm)
            {
                if (yesButton.IsClicked(cur, prev))
                {
                    if (SelectedMedicine != null)
                    {
                        SelectedMedicine.Amount -= 1;
                        player.Money += SelectedMedicine.Worth / 2;
                        drawSellConfirm = false;
                        SelectedMedicine = null;
                    }
                    if (SelectedCapture != null)
                    {
                        SelectedCapture.Amount -= 1;
                        player.Money += SelectedCapture.Worth / 2;
                        drawSellConfirm = false;
                        SelectedCapture = null;
                    }
                }
                if (NoButton.IsClicked(cur, prev))
                {
                        SelectedMedicine = null;
                        SelectedCapture = null;
                }
            }

            if (drawBuyConfirm)
            {
                if (yesButton.IsClicked(cur, prev))
                {
                    if (SelectedMedicine != null)
                    {
                        player.Inventory.Add(SelectedMedicine, 1);
                        player.Money -= SelectedMedicine.Worth;
                        drawBuyConfirm = false;
                        SelectedMedicine = null;
                    }
                    if (SelectedCapture != null)
                    {
                        player.Inventory.Add(SelectedCapture, 1);
                        player.Money -= SelectedCapture.Worth;
                        drawBuyConfirm = false;
                        SelectedCapture = null;
                    }
                }
                if (NoButton.IsClicked(cur, prev))
                {
                        SelectedMedicine = null;
                        SelectedCapture = null;
                    drawBuyConfirm = false;
                }
            }
            Drawer.UpdateItemButtons(cur, prev);
        }

        public void Buy(MouseState cur, MouseState prev)
        {
            if (Drawer.DrawMedicine)
            {
                foreach (var m in shopMedicineButtons)
                {
                        if (m.Button.IsClicked(cur, prev))
                        {
                            SelectedMedicine = m.Medicine;
                        }
                }
            }
            if (Drawer.DrawCapture)
            {
                foreach (var m in shopCaptureButtons)
                {
                        if (m.Button.IsClicked(cur, prev))
                        {
                            SelectedCapture = m.Capture;
                        }
                }
            }
        }
        public void Sell(MouseState cur, MouseState prev)
        {
            if (Drawer.DrawMedicine)
            {
                foreach (var m in playerMedicineButtons)
                {
                    if (m.Button.IsClicked(cur, prev))
                    {
                        SelectedMedicine = m.Medicine;
                    }
                }
            }
            if (Drawer.DrawCapture)
            {
                foreach (var m in playerCaptureButtons)
                {
                    if (m.Button.IsClicked(cur, prev))
                    {
                        SelectedCapture = m.Capture;
                    }
                }
            }
        }

        public void DrawBuy(SpriteBatch batch)
        {
            if (Drawer.DrawCapture)
                foreach (var containerButton in shopCaptureButtons.Where(x => x.Capture.Amount > 0))
                {
                    containerButton.Button.Draw(batch);
                }
            if (Drawer.DrawMedicine)
                foreach (var containerButton in shopMedicineButtons.Where(x => x.Medicine.Amount > 0))
                {
                    containerButton.Button.Draw(batch);
                }
            Drawer.DrawInventory(batch, ShopKeeper);
        }

        public void DrawSell(SpriteBatch batch, Character player)
        {
            if (Drawer.DrawCapture)
                foreach (var containerButton in playerCaptureButtons.Where(x => x.Capture.Amount > 0))
                {
                    containerButton.Button.Draw(batch);
                }
            if (Drawer.DrawMedicine)
                foreach (var containerButton in playerMedicineButtons.Where(x => x.Medicine.Amount > 0))
                {
                    containerButton.Button.Draw(batch);
                }
            Drawer.DrawInventory(batch, player);
        }
    }

}
