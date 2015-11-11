using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VideoGame.Classes {

    public enum Kind {
        Physical,
        Special,
        NonDamage
    }

    public class Move {

        public string Name;
        public string Description;
        public int Damage;
        public Kind Kind;
        public int Accuracy;
        public Type Type;

        public Move(string name, string description, int damage, int accuracy, Kind kind, Type type) {
            Name = name;
            Description = description;
            Damage = damage;
            Accuracy = accuracy;
            Kind = kind;
            Type = type;
        }
        #region Preset Moves

        #region Physical
        public static Move Tackle(Monster user, Monster receiver) {
            return new Move("Tackle", "A fullbody tackle",
                60, 100, Kind.Physical, Type.Normal);
        }
        #endregion

        #region Special
        public static Move Bubble(Monster user, Monster receiver) {
            return new Move("Bubble", "A fullbody tackle",
                60, 100, Kind.Physical, Type.Water);
        }

        #endregion

        #region NonDamage
        public static Move Glare(Monster user, Monster receiver) {
            receiver.Stats = new StatModifier(0, 0, 0, 0, 90).ApplyModifiers(receiver);
            return new Move("Glare", "The monster gives a cold glare and slightly lowers opponents speed", 
                0, 70, Kind.NonDamage, Type.Normal);
        }  
        #endregion
        #endregion
    }
}
