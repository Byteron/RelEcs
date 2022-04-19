using System.Collections.Generic;

namespace Bitron.Ecs
{
    public sealed class SystemGroup
    {
        List<ISystem> systems = new List<ISystem>();

        public SystemGroup Add(ISystem system)
        {
            systems.Add(system);
            return this;
        }

        public void Run(World world)
        {
            var commands = new Commands(world);

            for (var i = 0; i < systems.Count; i++)
            {
                systems[i].Run(commands);
            }
        }
    }

    public interface ISystem
    {
        void Run(Commands commands);
    }

    public static class SystemExtentions
    {
        public static void Run(this ISystem system, World world)
        {
            system.Run(new Commands(world));
        }
    }
}
