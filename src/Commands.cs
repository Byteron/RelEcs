using System;
using System.Runtime.CompilerServices;

namespace RelEcs
{
    public sealed class Commands
    {
        readonly World world;
        readonly ISystem system;

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
        public void Send<T>(T triggerStruct) where T : struct
        {
            world.Send(triggerStruct);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Receive<T>(Action<T> action) where T : struct
        {
            world.Receive(system, action);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddElement<T>(T element) where T : class
        {
            world.AddElement(element);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetElement<T>() where T : class
        {
            return world.GetElement<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetElement<T>(out T element) where T : class
        {
            return world.TryGetElement(out element);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasElement<T>() where T : class
        {
            return world.HasElement<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveElement<T>() where T : class
        {
            world.RemoveElement<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public QueryCommands Query()
        {
            return new QueryCommands(world);
        }

        public Storage<T> GetStorage<T>(EntityId target) where T : struct
        {
            return world.GetStorage<T>(target);
        }
    }

    public sealed class QueryCommands
    {
        internal readonly World World;

        readonly Mask mask;
        
        Query query;

        public QueryCommands(World world)
        {
            World = world;
            mask = new Mask();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public QueryCommands Has<T>(Entity target = default) where T : struct
        {
            var typeIndex = World.GetStorage<T>(target.Id).Index;
            mask.Has(typeIndex);
            return this;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public QueryCommands Has<T, TT>() where T : struct where TT : struct
        {
            var typeEntity = World.GetTypeEntity<TT>();
            Has<T>(typeEntity);
            return this;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public QueryCommands IsA(Entity target = default)
        {
            Has<IsA>(target);
            return this;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public QueryCommands IsA<T>() where T : struct
        {
            Has<IsA, T>();
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public QueryCommands Not<T>(Entity target = default) where T : struct
        {
            var typeIndex = World.GetStorage<T>(target.Id).Index;
            mask.Not(typeIndex);
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public QueryCommands Any<T>(Entity target = default) where T : struct
        {
            var typeIndex = World.GetStorage<T>(target.Id).Index;
            mask.Any(typeIndex);
            return this;
        }

        public int Count
        {
            get
            {
                if (query != null) return query.Count;
                
                mask.Lock();
                query = World.GetQuery(mask);

                return query.Count;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Query.Enumerator GetEnumerator()
        {
            if (query != null) return query.GetEnumerator();
            
            mask.Lock();
            query = World.GetQuery(mask);

            return query.GetEnumerator();
        }
    }
}