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

        public EntityCommands Spawn()
        {
            return Entity(world.Spawn());
        }

        public EntityCommands Entity(Entity entity)
        {
            return new EntityCommands(this, entity);
        }

        internal void Despawn(Entity entity)
        {
            world.Despawn(entity);
        }

        public QueryCommands Query()
        {
            return new QueryCommands(world);
        }

        public ref T AddComponent<T>(Entity entity) where T : struct, IComponent
        {
            return ref world.AddComponent<T>(entity);
        }

        public ref T AddComponent<T>(Entity entity, T component) where T : struct, IComponent
        {
            ref var newComponent = ref world.AddComponent<T>(entity);
            newComponent = component;
            return ref newComponent;
        }

        public ref T GetComponent<T>(Entity entity) where T: struct, IComponent
        {
            return ref world.GetComponent<T>(entity);
        }

        public void RemoveComponent<T>(Entity entity) where T : struct, IComponent
        {
            world.RemoveComponent<T>(entity);
        }

        public void AddRelation<T>(Entity entity, Entity target) where T: struct, IRelation
        {
            world.AddRelation<T>(entity, target);
        }

        public Relation<T> GetRelation<T>(Entity entity) where T: struct, IRelation
        {
            return world.GetRelation<T>(entity);
        }

        public void RemoveRelation<T>(Entity entity) where T: struct, IRelation
        {
            world.RemoveRelation<T>(entity);
        }

        public bool IsAlive(Entity entity)
        {
            return world.IsAlive(entity);
        }
    }

    public sealed class EntityCommands
    {
        Commands commands;
        Entity entity;

        internal EntityCommands(Commands commands, Entity entity)
        {
            this.commands = commands;
            this.entity = entity;
        }

        public EntityCommands Add<T>() where T : struct, IComponent
        {
            commands.AddComponent<T>(entity);
            return this;
        }

        public EntityCommands Add<T>(T data) where T : struct, IComponent
        {
            commands.AddComponent<T>(entity, data);
            return this;
        }

        public EntityCommands Add<T>(Entity target) where T: struct, IRelation
        {
            commands.AddRelation<T>(entity, target);
            return this;
        }

        public EntityCommands Remove<T>() where T : struct, IComponent, IRelation
        {
            if (typeof(IComponent).IsAssignableFrom(typeof(T)))
            {
                commands.RemoveComponent<T>(entity);
            }
            else if (typeof(IRelation).IsAssignableFrom(typeof(T)))
            {
                commands.RemoveRelation<T>(entity);
            }
            return this;
        }

        public Entity Id()
        {
            return entity;
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

        public QueryCommands With<T>() where T : struct, IComponent
        {
            mask.With<T>();
            return this;
        }

        public QueryCommands Without<T>() where T : struct, IComponent
        {
            mask.Without<T>();
            return this;
        }

        public QueryCommands IsA<T>() where T : struct, IRelation
        {
            // TODO: make it filter for specific relatitions
            mask.IsA<T>();
            return this;
        }

        public QueryCommands IsA<T>(Entity target) where T : struct, IRelation
        {
            // TODO: make it filter for specific relatitions
            mask.IsA<T>(target);
            return this;
        }

        public Entity[] Apply()
        {
            return world.Query(mask);
        }

        // public QueryCommands Added<T>() where T : struct, IComponent
        // {
        //     mask.Added<T>();
        //     return this;
        // }

        // public QueryCommands Removed<T>() where T : struct, IComponent
        // {
        //     mask.Removed<T>();
        //     return this;
        // }
    }
}