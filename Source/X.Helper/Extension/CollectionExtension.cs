using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public static class CollectionExtension
{
    public static bool IsNullOrEmpty<T>(this IList<T> obj)
    {
        return obj == null || obj.Count == 0;
    }

    public static bool IsHasObject<T>(this IList<T> obj)
    {
        return obj != null && obj.Count > 0;
    }

    public static bool IsNullOrEmpty<K,V>(this IDictionary<K,V> obj)
    {
        return obj == null || obj.Count == 0;
    }

    public static bool IsNotNullOrEmpty<K, V>(this IDictionary<K, V> obj)
    {
        return obj != null && obj.Count > 0;
    }

    public static bool IsHasObject<K,V>(this IDictionary<K,V> obj)
    {
        return obj != null && obj.Count > 0;
    }
}

