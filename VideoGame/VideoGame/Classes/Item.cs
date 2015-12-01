using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace VideoGame.Classes {
    public class Item {
        public int Id;
        public string Name;
        public string Description;
        public bool Useable;
        public int Worth;
        public int Amount;
        public int MaxAmount;
        public Texture2D Sprite;
    }

    public class Capture : Item {
        public double CaptureChance;

        /// <summary>
        /// Default Capture Item
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="sprite"></param>
        /// <param name="captureChance">Chance to catch a monster with this</param>
        /// <param name="useable"></param>
        /// <param name="worth"></param>
        /// <param name="amount"></param>
        /// <param name="maxAmount"></param>
        public Capture(int id, string name, string description, Texture2D sprite, double captureChance, bool useable, int worth, int amount, int maxAmount) {
            Id = id;
            Name = name;
            Description = description;
            Sprite = sprite;
            CaptureChance = captureChance;
            Useable = useable;
            Worth = worth;
            Amount = amount;
            MaxAmount = maxAmount;
        }

        public void Use() {
            //Throw the net
        }

        public static Capture RottenNet() {
            return new Capture(1, "Rotten Net", "Worn out net which doesn't seem up to the job.", ContentLoader.RottenNet, 45, true, 5, 1, 999);
        }
        public static Capture RegularNet() {
            return new Capture(2, "Regular Net", "Regular net which seems decent at first glance.", ContentLoader.RegularNet, 45, true, 200, 1, 99);
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
        /// <param name="sprite"></param>
        /// <param name="healAmount"></param>
        /// <param name="worth"></param>
        /// <param name="amount"></param>
        /// <param name="maxAmount"></param>
        public Medicine(int id, string name, string description, Texture2D sprite, int healAmount, int worth, int amount, int maxAmount) {
            Useable = true;
            Id = id;
            Name = name;
            Description = description;
            Sprite = sprite;
            HealAmount = healAmount;
            Cures = Cure.None;
            Useable = true;
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
        public Medicine(int id, string name, string description, Texture2D sprite, Cure cure, int worth, int amount, int maxAmount) {
            Useable = true;
            Id = id;
            Name = name;
            Description = description;
            Sprite = sprite;
            HealAmount = 0;
            Cures = cure;
            Useable = true;
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
        public Medicine(int id, string name, string description, Texture2D sprite, int healAmount, Cure cure, int worth, int amount, int maxAmount) {
            Useable = true;
            Id = id;
            Name = name;
            Description = description;
            Sprite = sprite;
            HealAmount = healAmount;
            Cures = cure;
            Useable = true;
            Worth = worth;
            Amount = amount;
            MaxAmount = maxAmount;
        }

        public void Use() {
            //Use the medicine
        }

        public static Medicine LeafBandage()
        {
            return new Medicine(1,"Leaf Bandage","Bandage that heals 10 HP", ContentLoader.LeafBandage, 10, 50, 1, 99);
        }
        public static Medicine MagicStone()
        {
            return new Medicine(2, "Magic Stone", "Stone that heals 20 HP", ContentLoader.MagicStone, 50, 500, 1, 99);
        }

        public static Medicine AntiPoison()
        {
            return new Medicine(3,"Anti Poison", "Cures poison", ContentLoader.AntiPoison, Cure.Poisoned, 200, 1, 99);
        }

        public static Medicine BucketOfWater()
        {
            return new Medicine(4,"Bucket Of Water", "Throw the bucket with a high speed at the monster", ContentLoader.BucketOfWater, -20,Cure.Burned,150,1,99);
        }

        public static Medicine Salt()
        {
            return new Medicine(5, "Salt", "Throw salt at the frozen monster\n" + " !WARNING if your monster is a snail it WILL die!", ContentLoader.Salt, Cure.Frozen, 150, 1 ,99);
        }

        public static Medicine AirHorn()
        {
            return new Medicine(6, "Air Horn", "Blow the horn in the ears of the monster", ContentLoader.AirHorn, Cure.Sleep, 200, 1,99);
        }

        public static Medicine SpinalCord()
        {
            return new Medicine(7, "Spinal Cord", "Perform surgery on the monter to give it a new spinal cord\n" +
                                                  "Even if it didn't have one before.",
                ContentLoader.SpinalCord ,Cure.Paralyzed, 250, 1, 99);
        }

        public static Medicine RoosVicee()
        {
            return new Medicine(8, "RoosVicee", "Komt wel goed schatje", ContentLoader.RoosVicee, Cure.All, 586, 1,99);
        }
    }

    public class KeyItem : Item {
        //Not sure if we need this

        
        public void Use() {
            //Use the key!
        }
    }
}
