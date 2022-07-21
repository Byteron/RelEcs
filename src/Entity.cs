using System;
using System.Runtime.CompilerServices;

namespace RelEcs;

public class Entity : Object
{
    public static readonly Entity None = new(Identity.None);
    public static readonly Entity Any = new(Identity.Any);

    public bool IsAny => Identity == Identity.Any;
    public bool IsNone => Identity == Identity.None;

    public Identity Identity { get; }

    public Entity(Identity identity)
    {
        Identity = identity;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object obj)
    {
        return obj is Entity entity && Identity.Equals(entity.Identity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode()
    {
        return Identity.GetHashCode();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString()
    {
        return Identity.ToString();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Entity left, Entity right) => left is not null && left.Equals(right);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Entity left, Entity right) =>
        (left is null && right is not null) || (left is not null && !left.Equals(right));
}

public readonly struct EntityBuilder
{
    internal readonly World World;
    readonly Identity _identity;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public EntityBuilder(World world, Identity identity)
    {
        World = world;
        _identity = identity;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public EntityBuilder Add<T>(Entity target = default) where T : class, new()
    {
        World.AddComponent<T>(_identity, default, target?.Identity ?? Identity.None);
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public EntityBuilder Add<T>(Type type) where T : class, new()
    {
        var typeIdentity = World.GetTypeIdentity(type);
        World.AddComponent<T>(_identity, default, typeIdentity);
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public EntityBuilder Add<T>(T data) where T : class, new()
    {
        World.AddComponent(_identity, data);
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public EntityBuilder Add<T>(T data, Entity target) where T : class, new()
    {
        World.AddComponent(_identity, data, target.Identity);
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public EntityBuilder Add<T>(T data, Type type) where T : class, new()
    {
        var typeIdentity = World.GetTypeIdentity(type);
        World.AddComponent(_identity, data, typeIdentity);
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public EntityBuilder Remove<T>() where T : class
    {
        World.RemoveComponent<T>(_identity);
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public EntityBuilder Remove<T>(Entity target) where T : class
    {
        World.RemoveComponent<T>(_identity, target.Identity);
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public EntityBuilder Remove<T>(Type type) where T : class
    {
        var typeIdentity = World.GetTypeIdentity(type);
        World.RemoveComponent<T>(_identity, typeIdentity);
        return this;
    }

    public Entity Id()
    {
        return new Entity(_identity);
    }
}