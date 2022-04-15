using System;

namespace Bitron.Ecs
{
    public sealed class Commands
    {
        World _world;

        internal Commands(World world)
        {
            _world = world;
        }

        public EntityCommands Spawn()
        {
            return Entity(_world.Spawn());
        }

        public EntityCommands Entity(Entity entity)
        {
            return new EntityCommands(this, entity);
        }

        internal void Despawn(Entity entity)
        {
            _world.Despawn(entity);
        }

        public QueryCommands Query()
        {
            return new QueryCommands(_world);
        }

        public ref Component AddComponent<Component>(Entity entity) where Component : struct
        {
            return ref _world.AddComponent<Component>(entity);
        }

        public ref Component AddComponent<Component>(Entity entity, Component component) where Component : struct
        {
            ref var newComponent = ref _world.AddComponent<Component>(entity);
            newComponent = component;
            return ref newComponent;
        }

        public delegate void RefAction<C>(ref C c);
        public void ForEach<C>(RefAction<C> action)
            where C : struct
        {
            var mask = new Mask();
            mask.Include<C>();

            var entities = _world.Query(mask);

            var storage = _world.GetStorage<C>();

            foreach (Entity entity in entities)
            {
                action(ref storage.Get(entity));
            }
        }

        public delegate void RefAction<C1, C2>(ref C1 c1, ref C2 c2);
        public void ForEach<C1, C2>(RefAction<C1, C2> action)
            where C1 : struct
            where C2 : struct
        {
            var mask = new Mask();
            mask.Include<C1>();
            mask.Include<C2>();

            var entities = _world.Query(mask);

            var storage1 = _world.GetStorage<C1>();
            var storage2 = _world.GetStorage<C2>();

            foreach (Entity entity in entities)
            {
                action(ref storage1.Get(entity), ref storage2.Get(entity));
            }
        }

        public void RemoveComponent<Component>(Entity entity) where Component : struct
        {
            var storage = _world.GetStorage<Component>();
            storage.Remove(entity);
        }

        public bool IsAlive(Entity entity)
        {
            return _world.IsAlive(entity);
        }
    }

    public sealed class EntityCommands
    {
        Commands _commands;
        Entity _entity;

        internal EntityCommands(Commands commands, Entity entity)
        {
            _commands = commands;
            _entity = entity;
        }

        public EntityCommands Add<Component>() where Component : struct
        {

            _commands.AddComponent<Component>(_entity);
            return this;
        }

        public EntityCommands Add<Component>(Component component) where Component : struct
        {
            _commands.AddComponent<Component>(_entity, component);
            return this;
        }

        public EntityCommands Remove<Component>() where Component : struct
        {
            _commands.RemoveComponent<Component>(_entity);
            return this;
        }

        public Entity Id()
        {
            return _entity;
        }
    }

    public sealed class QueryCommands
    {
        Entity[] _entities;

        Mask _mask = new Mask();

        World _world;

        internal QueryCommands(World world)
        {
            _world = world;
        }

        public QueryCommands Include<T>() where T : struct
        {
            _mask.Include<T>();
            return this;
        }

        public QueryCommands Exclude<T>() where T : struct
        {
            _mask.Exclude<T>();
            return this;
        }

        public QueryCommands Added<T>() where T : struct
        {
            _mask.Added<T>();
            return this;
        }

        public QueryCommands Removed<T>() where T : struct
        {
            _mask.Removed<T>();
            return this;
        }

        public void Run(Action action)
        {
            _entities = _world.Query(_mask);

            foreach (Entity entity in _entities)
            {
                // ???
            }
        }
    }
}