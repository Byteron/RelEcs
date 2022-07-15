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
    public EntityBuilder Spawn()
    {
        return new EntityBuilder(World, World.Spawn().Identity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public EntityBuilder Amend(Entity entity)
    {
        return new EntityBuilder(World, entity.Identity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T GetComponent<T>(Entity entity) where T : class
    {
        return World.GetComponent<T>(entity.Identity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T GetComponent<T>(Entity entity, Entity target) where T : class
    {
        return World.GetComponent<T>(entity.Identity, target.Identity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T GetComponent<T>(Entity entity, Type type) where T : class
    {
        var typeIdentity = World.GetTypeIdentity(type);
        return World.GetComponent<T>(entity.Identity, typeIdentity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGetComponent<T>(Entity entity, out T component) where T : class
    {
        return TryGetComponent(entity, RelEcs.Entity.None, out component);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGetComponent<T>(Entity entity, Entity target, out T component) where T : class
    {
        if (World.HasComponent<T>(entity.Identity, target.Identity))
        {
            component = World.GetComponent<T>(entity.Identity, target.Identity);
            return true;
        }

        component = null;
        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGetComponent<T>(Entity entity, Type type, out T component) where T : class
    {
        var typeIdentity = World.GetTypeIdentity(type);
        if (World.HasComponent<T>(entity.Identity, typeIdentity))
        {
            component = World.GetComponent<T>(entity.Identity, typeIdentity);
            return true;
        }

        component = null;
        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool HasComponent<T>(Entity entity) where T : class
    {
        return World.HasComponent<T>(entity.Identity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool HasComponent<T>(Entity entity, Entity target) where T : class
    {
        return World.HasComponent<T>(entity.Identity, target.Identity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool HasComponent<T>(Entity entity, Type type) where T : class
    {
        var typeIdentity = World.GetTypeIdentity(type);
        return World.HasComponent<T>(entity.Identity, typeIdentity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Entity[] GetTargets<T>(Entity entity) where T : class
    {
        return World.GetTargets<T>(entity.Identity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Despawn(Entity entity)
    {
        World.Despawn(entity.Identity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Send<T>(T trigger) where T : class
    {
        World.Send(trigger);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Receive<T>(Action<T> action) where T : class
    {
        World.Receive(system, action);
    }

    public void AddElement<T>(T element) where T : class
    {
        World.AddElement(element);
    }

    public void ReplaceElement<T>(T element) where T : class
    {
        World.ReplaceElement(element);
    }

    public void AddOrReplaceElement<T>(T element) where T : class
    {
        if (World.HasElement<T>()) World.ReplaceElement(element);
        else World.AddElement(element);
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
}