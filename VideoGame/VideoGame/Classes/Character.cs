using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VideoGame.Classes {
    class Character {
        public bool Controllable;
        public string Name;
        public int Money;
        public Inventory Inventory;
        public List<Monster> Monsters;

        /// <summary>
        /// New character
        /// </summary>
        /// <param name="name">Characters name</param>
        /// <param name="money">Amount of money the character has</param>
        /// <param name="inventory">Inventory of the character</param>
        /// <param name="monsters">Monsters that the character has</param>
        /// <param name="controllable">Is this a playable character</param>
        public Character(string name, int money, Inventory inventory, List<Monster> monsters, bool controllable = false) {
            Name = name;
            Money = money;
            Inventory = inventory;
            Monsters = monsters;
            Controllable = controllable;
        }
    }
}
