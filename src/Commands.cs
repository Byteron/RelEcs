using System;
using System.Runtime.CompilerServices;

namespace RelEcs;

public readonly struct Commands
{
    public readonly World World;
    readonly ISystem system;

    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal Commands(World world, ISystem system)
    {
        World = world;
        this.system = system;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Entity Spawn()
    {
        return World.Spawn();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Send<T>(T triggerStruct = default) where T : struct
    {
        World.Send(triggerStruct);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Receive<T>(Action<T> action) where T : struct
    {
        World.Receive(system, action);
    }
    
    public void AddElement<T>(T element) where T : class
    {
        World.AddElement(element);
    }

    public T GetElement<T>() where T : class
    {
        return World.GetElement<T>();
    }

    public bool TryGetElement<T>(out T element) where T : class
    {
        if (World.HasElement<T>())
        {
            element = World.GetElement<T>();
            return true;
        }

        element = null;
        return false;
    }
    
    public bool HasElement<T>() where T : class
    {
        return World.HasElement<T>();
    }
    
    public void RemoveElement<T>() where T : class
    {
        World.RemoveElement<T>();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public QueryCommands Query()
    {
        return new QueryCommands(World);
    }
}