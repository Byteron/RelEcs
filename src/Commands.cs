using System;
using System.Runtime.CompilerServices;

namespace RelEcs
{
    public sealed class Commands
    {
        World world;
        ISystem system;

        internal Commands(World world, ISystem system)
        {
            this.world = world;
            this.system = system;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Entity Spawn()
        {
            return world.Spawn();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Send<T>() where T : struct
        {
            world.Send<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Send<T>(T eventStruct) where T : struct
        {
            world.Send<T>(eventStruct);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Receive<T>(Action<T> action) where T : struct
        {
            world.Receive<T>(system, action);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddResource<T>(T resource) where T : class
        {
            world.AddResource<T>(resource);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetResource<T>() where T : class
        {
            return world.GetResource<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetResource<T>(out T resource) where T : class
        {
            return world.TryGetResource<T>(out resource);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasResource<T>() where T : class
        {
            return world.HasResource<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveResource<T>() where T : class
        {
            world.RemoveResource<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public QueryCommands Query()
        {
            return new QueryCommands(world);
        }

        internal Storage<T> GetStorage<T>(EntityId target) where T : struct
        {
            return world.GetStorage<T>(target);
        }
    }

    public sealed class QueryCommands
    {
        internal World World;

        Query query;
        Mask mask;

        public QueryCommands(World world)
        {
            World = world;
            mask = Mask.New(world);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public QueryCommands With<T>(Entity target = default) where T : struct
        {
            mask.With<T>(target);
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public QueryCommands Optional<T>(Entity target = default) where T : struct
        {
            mask.Optional<T>(target);
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public QueryCommands Without<T>(Entity target = default) where T : struct
        {
            mask.Without<T>(target);
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Query.Enumerator GetEnumerator()
        {
            if (query == null)
            {
                query = mask.Apply();
            }

            return query.GetEnumerator();
        }
    }
}