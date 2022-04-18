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

        public QueryCommands Query()
        {
            return new QueryCommands(world);
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

        public Entity[] Apply()
        {
            return world.Query(mask);
        }
    }
}