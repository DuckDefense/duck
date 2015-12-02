using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
}

