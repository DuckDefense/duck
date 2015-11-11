using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VideoGame.Classes {
    public class Item {
        public int Id;
        public string Name;
        public string Description;
        public bool Useable;
        public int Worth;
        public int Amount;
        public int MaxAmount;
    }

    public class Capture : Item {
        public double CaptureChance;

        /// <summary>
        /// Default Capture Item
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="captureChance">Chance to catch a monster with this</param>
        /// <param name="useable"></param>
        /// <param name="worth"></param>
        /// <param name="amount"></param>
        /// <param name="maxAmount"></param>
        public Capture(int id, string name, string description, double captureChance, bool useable, int worth, int amount, int maxAmount) {
            Id = id;
            Name = name;
            Description = description;
            CaptureChance = captureChance;
            Useable = useable;
            Worth = worth;
            Amount = amount;
            MaxAmount = maxAmount;
        }

        public void Use() {
            //Throw the net
        }
    }

    public class Medicine : Item {
        public enum Cure { None, Sleep, Poisoned, Paralyzed, Burned, Frozen, All }

        public int HealAmount { get; private set; }
        public Cure Cures { get; private set; }

        /// <summary>
        /// Default healing medicine
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="healAmount"></param>
        /// <param name="useable"></param>
        /// <param name="worth"></param>
        /// <param name="amount"></param>
        /// <param name="maxAmount"></param>
        public Medicine(int id, string name, string description, int healAmount, int worth, int amount, int maxAmount) {
            Useable = true;
            Id = id;
            Name = name;
            Description = description;
            HealAmount = healAmount;
            Cures = Cure.None;
            Worth = worth;
            Amount = amount;
            MaxAmount = maxAmount;
        }

        /// <summary>
        /// Medicine that heals and cures an ailment
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="cure"></param>
        /// <param name="worth"></param>
        /// <param name="amount"></param>
        /// <param name="maxAmount"></param>
        public Medicine(int id, string name, string description, Cure cure, int worth, int amount, int maxAmount) {
            Useable = true;
            Id = id;
            Name = name;
            Description = description;
            HealAmount = 0;
            Cures = cure;
            Worth = worth;
            Amount = amount;
            MaxAmount = maxAmount;
        }

        /// <summary>
        /// Medicine that heals and cures an ailment
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="healAmount"></param>
        /// <param name="cure"></param>
        /// <param name="worth"></param>
        /// <param name="amount"></param>
        /// <param name="maxAmount"></param>
        public Medicine(int id, string name, string description, int healAmount, Cure cure, int worth, int amount, int maxAmount) {
            Useable = true;
            Id = id;
            Name = name;
            Description = description;
            HealAmount = healAmount;
            Cures = cure;
            Worth = worth;
            Amount = amount;
            MaxAmount = maxAmount;
        }

        public void Use() {
            //Use the medicine
        }
    }

    public class KeyItem : Item {
        //Not sure if we need this

        
        public void Use() {
            //Use the key!
        }
    }
}
