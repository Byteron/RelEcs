using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RelEcs;

public sealed class TableEdge
{
    public Table Add;
    public Table Remove;
}

public sealed class Table
{
    const int StartCapacity = 4;
    
    public readonly int Id;

    public readonly SortedSet<StorageType> Types;
    
    public Identity[] Entities => entities;
    
    public int Count { get; private set; }
    public bool IsEmpty => Count == 0;
    public bool IsLocked { get; private set; }
    
    readonly World world;
    
    int lockCount;

    Identity[] entities;
    readonly Array[] storages;

    readonly Dictionary<StorageType, TableEdge> edges = new();
    readonly Dictionary<StorageType, int> indices = new();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Table(int id, World world, SortedSet<StorageType> types)
    {
        this.world = world;

        Id = id;
        Types = types;

        entities = new Identity[StartCapacity];

        var i = 0;
        foreach (var type in types)
        {
            if (type.IsTag) continue;
            indices.Add(type, i++);
        }

        storages = new Array[indices.Count];

        foreach (var (type, index) in indices)
        {
            storages[index] = Array.CreateInstance(type.Type, StartCapacity);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Add(Identity identity)
    {
        EnsureCapacity(Count + 1);
        entities[Count] = identity;
        return Count++;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Remove(int row)
    {
        if (row >= Count) throw new ArgumentOutOfRangeException(nameof(row), "row cannot be greater or equal to count");

        Count--;

        if (row < Count)
        {
            entities[row] = entities[Count];

            foreach (var storage in storages)
            {
                Array.Copy(storage, Count, storage, row, 1);
            }
            
            world.GetEntityMeta(entities[row]).Row = row;
        }

        entities[Count] = default;

        foreach (var storage in storages)
        {
            Array.Clear(storage, Count, 1);
        }
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TableEdge GetTableEdge(StorageType type)
    {
        if (edges.TryGetValue(type, out var edge)) return edge;

        edge = new TableEdge();
        edges[type] = edge;

        return edge;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T[] GetStorage<T>(Identity target)
    {
        var type = StorageType.Create<T>(target);
        return (T[])GetStorage(type);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Array GetStorage(StorageType type)
    {
        if (type.IsTag) throw new Exception("Cannot get Storage of Tag Component");
        return storages[indices[type]];
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void EnsureCapacity(int capacity)
    {
        if (capacity <= 0) throw new ArgumentOutOfRangeException(nameof(capacity), "minCapacity must be positive");
        if (capacity <= entities.Length) return;

        Resize(Math.Max(capacity, StartCapacity) << 1);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void Resize(int length)
    {
        if (length < 0) throw new ArgumentOutOfRangeException(nameof(length), "length cannot be negative");
        if (length < Count)
            throw new ArgumentOutOfRangeException(nameof(length), "length cannot be smaller than Count");

        Array.Resize(ref entities, length);

        for (var i = 0; i < storages.Length; i++)
        {
            var elementType = storages[i].GetType().GetElementType()!;
            var newStorage = Array.CreateInstance(elementType, length);
            Array.Copy(storages[i], newStorage, Math.Min(storages[i].Length, length));
            storages[i] = newStorage;
        }
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int MoveEntry(Identity identity, int oldRow, Table oldTable, Table newTable)
    {
        var newRow = newTable.Add(identity);
        
        foreach (var (type, oldIndex) in oldTable.indices)
        {
            if (!newTable.indices.TryGetValue(type, out var newIndex) || newIndex < 0) continue;

            var oldStorage = oldTable.storages[oldIndex];
            var newStorage = newTable.storages[newIndex];

            Array.Copy(oldStorage, oldRow, newStorage, newRow, 1);
        }

        oldTable.Remove(oldRow);

        return newRow;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Lock()
    {
        lockCount++;
        IsLocked = true;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Unlock()
    {
        lockCount--;
        if (lockCount == 0)
        {
            IsLocked = false;
        }
    }
}