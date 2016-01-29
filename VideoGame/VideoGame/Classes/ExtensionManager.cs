using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VideoGame.Classes;

public static class ExtensionManager {
    public static void AddMany<T>(this List<T> list, params T[] elements) {
        list.AddRange(elements);
    }

    public static void Move<T>(this List<T> list, T movable, int position)
    {
        var item = list[position];
        int pos = list.IndexOf(movable);
        list[position] = movable;
        list[pos] = item;
    }

    public static void MoveItem<T>(this List<T> list, int firstIndex, int secondIndex) {
        var item = list[firstIndex];
        var secondItem = list[secondIndex];
        list[firstIndex] = secondItem;
        list[secondIndex] = item;
    }

    public static void MoveToList<T>(this List<T> list, List<T> secondList, int firstIndex, int secondIndex) {
        try {
            var item = list[secondIndex];
            var secondItem = secondList[firstIndex];
            list[secondIndex] = secondItem;
            secondList[firstIndex] = item;
        }
        catch (IndexOutOfRangeException) {
            //No idea what causes this, just make the player try again 
        }
    }

    public static void AddIfNotNull<T>(this List<T> list, T value) {
        if ((object)value != null)
            list.Add(value);
    }

    public static void AddManyIfNotNull<T>(this List<T> list, params T[] elements) {
        foreach (var el in elements) {
            list.AddIfNotNull(el);
        }
    }

    public static Dictionary<int, Monster> CombineDictionaries(Dictionary<int, Monster> dic, Dictionary<int, Monster> dic2) {
        var dictionary = dic.ToDictionary(v => v.Key, v => v.Value);
        foreach (var v2 in dic2.Where(v2 => !dictionary.ContainsKey(v2.Key))) {
            dictionary.Add(v2.Key, v2.Value);
        }
        return dictionary;
    }
}

