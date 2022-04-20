namespace Bitron.Ecs
{
    public sealed class Commands
    {
        World world;

        public Commands(World world)
        {
            this.world = world;
        }

        public Entity Spawn()
        {
            return world.Spawn();
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
        Entity[] entities;
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

        public QueryCommands Added<T>(Entity target = default) where T : struct
        {
            mask.Added<T>(target);
            return this;
        }

        public QueryCommands Removed<T>(Entity target = default) where T : struct
        {
            mask.Removed<T>(target);
            return this;
        }

        public Entity[] Apply()
        {
            return world.Query(mask);
        }
    }
}