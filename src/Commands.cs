namespace Bitron.Ecs
{
    public sealed class Commands
    {
        private World _world;

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

        public ref Component AddComponent<Component>(Entity entity) where Component: struct
        {
            return ref _world.AddComponent<Component>(entity);
        }

        public ref Component AddComponent<Component>(Entity entity, Component component) where Component: struct
        {
            ref var newComponent = ref _world.AddComponent<Component>(entity);
            newComponent = component;
            return ref newComponent;
        }

        public void RemoveComponent<Component>(Entity entity) where Component: struct
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
        private Commands _commands;
        private Entity _entity;

        internal EntityCommands(Commands commands, Entity entity)
        {
            _commands = commands;
            _entity = entity;
        }

        public EntityCommands Add<Component>() where Component: struct
        {
            
            _commands.AddComponent<Component>(_entity);
            return this;
        }

        public EntityCommands Add<Component>(Component component) where Component: struct
        {
            _commands.AddComponent<Component>(_entity, component);
            return this;
        }

        public EntityCommands Remove<Component>() where Component: struct
        {
            _commands.RemoveComponent<Component>(_entity);
            return this;
        }

        public Entity Id()
        {
            return _entity;
        }
    }
}