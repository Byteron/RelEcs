using System;

namespace Bitron.Ecs
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

        public Entity Spawn()
        {
            return world.Spawn();
        }

        public void Send<T>() where T : struct
        {
            world.Send<T>();
        }

        public void Send<T>(T eventStruct) where T : struct
        {
            world.Send<T>(eventStruct);
        }

        public void Receive<T>(Action<T> action) where T : struct
        {
            world.Receive<T>(system, action);
        }

        public void AddResource<T>(T resource) where T : class
        {
            world.AddResource<T>(resource);
        }

        public T GetResource<T>() where T : class
        {
            return world.GetResource<T>();
        }

        public void RemoveResource<T>() where T : class
        {
            world.RemoveResource<T>();
        }

        public QueryCommands Query()
        {
            return new QueryCommands(world);
        }
    }

    public sealed class QueryCommands
    {
        World world;
        Mask mask;

        public QueryCommands(World world)
        {
            this.world = world;
            mask = new Mask(world);
        }

        public QueryCommands With<T>(Entity target = default) where T : struct
        {
            mask.With<T>(target);
            return this;
        }

        public QueryCommands Without<T>(Entity target = default) where T : struct
        {
            mask.Without<T>(target);
            return this;
        }

        public Query Apply()
        {
            return world.GetQuery(mask);
        }
    }
}