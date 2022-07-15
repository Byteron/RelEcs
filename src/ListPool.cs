using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace RelEcs;

public static class ListPool<T>
{
    static readonly Stack<List<T>> Stack = new();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static List<T> Get()
    {
        return Stack.Count > 0 ? Stack.Pop() : new List<T>();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Add(List<T> list)
    {
        list.Clear();
        Stack.Push(list);
    }
}