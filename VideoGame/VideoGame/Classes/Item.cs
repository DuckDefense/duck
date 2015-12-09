﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.Xna.Framework;
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
        public int CaptureChance;

        //Capture animation
        public List<Vector2> Patrols = new List<Vector2>();
        public int currentPatrol = 1;
        public int lastPatrol = 1;
        public float DistanceBetween;

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
        public Capture(int id, string name, string description, Texture2D sprite, int captureChance, bool useable, int worth, int amount, int maxAmount) {
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

        public void Use(Monster monster, Character player) {
            //Throw the net
            Random random = new Random();
            int capturechance = 0;

            CaptureAnimation(new List<Vector2>()
            {
                new Vector2(20,20),
                new Vector2(50,50)
            });

            if (monster.Stats.Health < (monster.PreviousStats.Health / 100 * 20)) capturechance += 25;
            else if (monster.Stats.Health < (monster.PreviousStats.Health / 100 * 40)) capturechance += 20;
            else if (monster.Stats.Health < (monster.PreviousStats.Health / 100 * 60)) capturechance += 15;
            else if (monster.Stats.Health < (monster.PreviousStats.Health / 100 * 80)) capturechance += 10;

            switch (monster.Ailment) {
            case Ailment.Sleep: capturechance += 20; break;
            case Ailment.Poisoned: capturechance += 10; break;
            case Ailment.Burned: capturechance += 10; break;
            case Ailment.Dazzled: capturechance += 10; break;
            case Ailment.Frozen: capturechance += 20; break;
            case Ailment.Frenzied: capturechance += 5; break;
            }

            capturechance += CaptureChance;
            capturechance += (player.Monsters[0].Level - monster.Level);
            capturechance -= monster.CaptureChance;

            if (capturechance < 5) capturechance = 5; 
            var dice = random.Next(0, 100);
            if (dice < capturechance) {
                monster.UId = RandomId.GenerateRandomUId();
                if (player.Monsters.Count >= 6) player.Box.Add(monster); 
                else player.Monsters.Add(monster);
            }
            Amount--;

            if (Amount < 1) player.Inventory.Captures.Remove(Id); 
        }

        public void CaptureAnimation(List<Vector2> PatrolList)
        {
            Patrols = PatrolList;
            lastPatrol = PatrolList.Count;
        }

        public static Capture RottenNet() {
            return new Capture(1, "Rotten Net", "Worn out net which doesn't seem up to the job.", ContentLoader.RottenNet, 5, true, 5, 1, 999);
        }
        public static Capture RegularNet() {
            return new Capture(2, "Regular Net", "Regular net which seems decent at first glance.", ContentLoader.RegularNet, 45, true, 200, 1, 99);
        }
        public static Capture GreatNet() {
            return new Capture(3, "Great Net", "A big net that will occasionally catch a monster.", ContentLoader.GreatNet, 65, true, 500, 1, 99);
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

        public void Use(Monster monster, Character player) {
            if (Cures != Cure.None) {
                if (Cures == Cure.All) {
                    monster.Ailment = Ailment.Normal;
                }
                if (monster.Ailment == (Ailment)Cures) {
                    monster.Ailment = Ailment.Normal;
                }
            }
            if ((monster.Stats.Health + HealAmount) >= monster.MaxHealth) {
                monster.Stats.Health = monster.MaxHealth;
            }
            else if (monster.Stats.Health != monster.MaxHealth)
                monster.Stats.Health += HealAmount;
            else {
                Amount++;
            }
            if (monster.Stats.Health < monster.PreviousStats.Health) {
                monster.Stats.Health = monster.PreviousStats.Health;
            }
            Amount--;

            if (Amount < 1) {
                player.Inventory.Medicine.Remove(Id);
            }
        }

        public static Medicine LeafBandage() {
            return new Medicine(1, "Leaf Bandage", "Bandage that heals 10 HP", ContentLoader.LeafBandage, 10, 50, 1, 99);
        }
        public static Medicine MagicStone() {
            return new Medicine(2, "Magic Stone", "Stone that heals 20 HP", ContentLoader.MagicStone, 50, 500, 1, 99);
        }

        public static Medicine AntiPoison() {
            return new Medicine(3, "Anti Poison", "Cures poison", ContentLoader.AntiPoison, Cure.Poisoned, 200, 1, 99);
        }

        public static Medicine BucketOfWater() {
            return new Medicine(4, "Bucket Of Water", "Throw the bucket with a high speed at the monster", ContentLoader.BucketOfWater, -20, Cure.Burned, 150, 1, 99);
        }

        public static Medicine Salt() {
            return new Medicine(5, "Salt", "Throw salt at the frozen monster\n" + " !WARNING if your monster is a snail it WILL die!", ContentLoader.Salt, Cure.Frozen, 150, 1, 99);
        }

        public static Medicine AirHorn() {
            return new Medicine(6, "Air Horn", "Blow the horn in the ears of the monster", ContentLoader.AirHorn, Cure.Sleep, 200, 1, 99);
        }

        public static Medicine RoosVicee() {
            return new Medicine(8, "RoosVicee", "Komt wel goed schatje", ContentLoader.RoosVicee, Cure.All, 586, 1, 99);
        }
    }

    public class KeyItem : Item {
        //Not sure if we need this


        public void Use() {
            //Use the key!
        }
    }
}
