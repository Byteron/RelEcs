using System;

namespace Bitron.Ecs
{
    public sealed class Commands
    {
        World world;

        internal Commands(World world)
        {
            this.world = world;
        }

        public Entity Spawn()
        {
            return world.Spawn();
        }

        public Entity Entity(Id id)
        {
            return new Entity(world, id);
        }

        internal void Despawn(Entity entity)
        {
            world.Despawn(entity.Id);
        }

        public QueryCommands Query()
        {
            return new QueryCommands(world);
        }

        public ref T AddComponent<T>(Entity entity) where T : struct
        {
            return ref world.AddComponent<T>(entity.Id);
        }

        public ref T AddComponent<T>(Entity entity, T component) where T : struct
        {
            ref var newComponent = ref world.AddComponent<T>(entity.Id);
            newComponent = component;
            return ref newComponent;
        }

        public ref T GetComponent<T>(Entity entity) where T: struct
        {
            return ref world.GetComponent<T>(entity.Id);
        }

        public void RemoveComponent<T>(Entity entity) where T : struct
        {
            world.RemoveComponent<T>(entity.Id);
        }

        public void AddRelation<T>(Entity entity, Entity target) where T: struct
        {
            world.AddRelation<T>(entity.Id, target);
        }

        public Relation<T> GetRelation<T>(Entity entity) where T: struct
        {
            return world.GetRelation<T>(entity.Id);
        }

        public void RemoveRelation<T>(Entity entity) where T: struct
        {
            world.RemoveRelation<T>(entity.Id);
        }

        public bool IsAlive(Entity entity)
        {
            return world.IsAlive(entity.Id);
        }
    }

    public sealed class QueryCommands
    {
        Entity[] entities;

        Mask mask = new Mask();

        World world;

        internal QueryCommands(World world)
        {
            this.world = world;
        }

        public QueryCommands With<T>() where T : struct
        {
            mask.With<T>();
            return this;
        }

        public QueryCommands Without<T>() where T : struct
        {
            mask.Without<T>();
            return this;
        }

        public QueryCommands IsA<T>() where T : struct
        {
            // TODO: make it filter for specific relatitions
            mask.IsA<T>();
            return this;
        }

        public QueryCommands IsA<T>(Entity target) where T : struct
        {
            // TODO: make it filter for specific relatitions
            mask.IsA<T>(target);
            return this;
        }

        public Entity[] Apply()
        {
            return world.Query(mask);
        }

        // public QueryCommands Added<T>() where T : struct
        // {
        //     mask.Added<T>();
        //     return this;
        // }

        // public QueryCommands Removed<T>() where T : struct
        // {
        //     mask.Removed<T>();
        //     return this;
        // }
    }
}