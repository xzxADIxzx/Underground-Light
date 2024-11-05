global using static Light.Tools;

namespace Light;

using Light.Content;
using System.Collections.Generic;

/// <summary> Set of different tools for simplifying life and systematization of code. </summary>
public static class Tools
{
    #region enumerables

    /// <summary> Returns the index of the object in the given enumerable. </summary>
    public static int IndexOf<T>(this IEnumerable<T> seq, T obj)
    {
        int index = 0;
        foreach (var item in seq)
        {
            if (item.Equals(obj)) return index;
            index++;
        }
        return -1;
    }

    /// <summary> Iterates each object in the given enumerable. </summary>
    public static void Each<T>(this IEnumerable<T> seq, Cons<T> cons)
    {
        foreach (var item in seq) cons(item);
    }

    /// <summary> Iterates each object in the given enumerable that are suitable for the given predicate. </summary>
    public static void Each<T>(this IEnumerable<T> seq, Pred<T> pred, Cons<T> cons)
    {
        foreach (var item in seq) if (pred(item)) cons(item);
    }

    /// <summary> Whether all of the elements match the given predicate. </summary>
    public static bool All<T>(this IEnumerable<T> seq, Pred<T> pred)
    {
        foreach (var item in seq) if (!pred(item)) return false;
        return true;
    }

    /// <summary> Whether any of the elements match the given predicate. </summary>
    public static bool Any<T>(this IEnumerable<T> seq, Pred<T> pred)
    {
        foreach (var item in seq) if (pred(item)) return true;
        return false;
    }

    #endregion
    #region delegates

    /// <summary> Performs an abstract action without any arguments or return value. </summary>
    public delegate void Call();

    /// <summary> Consumes one value. </summary>
    public delegate void Cons<T>(T t);

    /// <summary> Consumes two values. </summary>
    public delegate void Cons<T, K>(T t, K k);

    /// <summary> Predicate that consumes one value. </summary>
    public delegate bool Pred<T>(T t);

    /// <summary> Provider that returns one value. </summary>
    public delegate T Prov<T>();

    /// <summary> Function that consumes one value and returns another. </summary>
    public delegate K Func<T, K>(T t);

    #endregion
    #region entities

    /// <summary> Whether the type is a player. </summary>
    public static bool IsPlayer(this EntityType type) => type == EntityType.Player;

    /// <summary> Whether the type is an enemy. </summary>
    public static bool IsEnemy(this EntityType type) => type >= EntityType.Player;

    #endregion
}
