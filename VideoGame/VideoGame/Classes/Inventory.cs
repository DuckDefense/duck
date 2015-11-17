using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VideoGame.Classes {
    class Inventory {
        public Dictionary<int, Item> Items = new Dictionary<int, Item>();
        public Dictionary<int, Medicine> Medicine = new Dictionary<int, Medicine>();
        public Dictionary<int, Capture> Captures = new Dictionary<int, Capture>();
        public List<KeyItem> KeyItems = new List<KeyItem>();

        /// <summary>
        /// Add default item to inventory
        /// </summary>
        /// <param name="item"></param>
        /// <param name="amount"></param>
        public void Add(Item item, int amount) {
            if (Items.ContainsKey(item.Id)) {
                Items[item.Id].Amount += amount;
            }
            item.Amount = amount;
            Items.Add(item.Id, item);
        }

        /// <summary>
        /// Add Medicine to inventory
        /// </summary>
        /// <param name="item"></param>
        /// <param name="amount"></param>
        public void Add(Medicine item, int amount) {
            if (Medicine.ContainsKey(item.Id)) {
                Medicine[item.Id].Amount += amount;
            }
            item.Amount = amount;
            Medicine.Add(item.Id, item);
        }

        /// <summary>
        /// Add Capture item to inventory
        /// </summary>
        /// <param name="item"></param>
        /// <param name="amount"></param>
        public void Add(Capture item, int amount) {
            if (Captures.ContainsKey(item.Id)) {
                Captures[item.Id].Amount += amount;
            }
            item.Amount = amount;
            Captures.Add(item.Id, item);
        }

        /// <summary>
        /// Add key item to inventory
        /// </summary>
        /// <param name="key"></param>
        public void Add(KeyItem key) {
            KeyItems.Add(key);
        }
    }
}
