using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class ExtensionManager {
    public static void AddMany<T>(this List<T> list, params T[] elements) {
        list.AddRange(elements);
    }
}

