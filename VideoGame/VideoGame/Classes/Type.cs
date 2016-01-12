using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VideoGame.Classes {
    public enum Type {
        None,
        Normal,
        Fight,
        Fire,
        Water,
        Grass,
        Rock,
        Poison,
        Psych,
        Flying,
        Ice,
        Ghost,
        Sound
    }

    public static class TypeMethods {
        public static Type GetTypeFromString(string s) {
            switch (s) {
            case "None": return Type.None;
            case "Normal": return Type.Normal;
            case "Fight": return Type.Fight;
            case "Fire": return Type.Fire;
            case "Water": return Type.Water;
            case "Grass": return Type.Grass;
            case "Rock": return Type.Rock;
            case "Poison": return Type.Poison;
            case "Psych": return Type.Psych;
            case "Flying": return Type.Flying;
            case "Ice": return Type.Ice;
            case "Ghost": return Type.Ghost;
            case "Sound": return Type.Sound;
            }
            return Type.None;
        }
    }
}
