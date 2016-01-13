using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VideoGame.Classes
{
    public class Shop
    {
        public Character ShopKeeper;
        public string Name;

        public Medicine SelectedMedicine;
        public Capture SelectedCapture;

        private SpriteBatch Batch;

        public Shop(int money, Inventory inventory, string name, SpriteBatch batch)
        {
            Name = name;
            ShopKeeper  = new Character("ShopKeeper", money, inventory,new List<Monster>(), null, null, ContentLoader.ChristmanWorld, Vector2.Zero);
            Batch = batch;
        }

        public void DrawInventory()
        {
            Drawer.DrawInventory(Batch, ShopKeeper);
        }
        public void Buy()
        {
            if (Drawer.DrawMedicine)
            {
                foreach (var m in ShopKeeper.Inventory.Medicine)
                {
                    if (m.Value.Name == Drawer.GetClickedButton().Text)
                    {
                        SelectedMedicine = m.Value;
                    }
                }
            }
            if (Drawer.DrawCapture)
            {
                foreach (var m in ShopKeeper.Inventory.Captures)
                {
                    if (m.Value.Name == Drawer.GetClickedButton().Text)
                    {
                        SelectedCapture = m.Value;
                    }
                }
            }
        }
        public void Sell()
        {
            if (Drawer.DrawMedicine)
            {
                foreach (var m in ShopKeeper.Inventory.Medicine)
                {
                    if (m.Value.Name == Drawer.GetClickedButton().Text)
                    {
                        SelectedMedicine = m.Value;
                    }
                }
            }
            if (Drawer.DrawCapture)
            {
                foreach (var m in ShopKeeper.Inventory.Captures)
                {
                    if (m.Value.Name == Drawer.GetClickedButton().Text)
                    {
                        SelectedCapture = m.Value;
                    }
                }
            }
        }
    }

}
