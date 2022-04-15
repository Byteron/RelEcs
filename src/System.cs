using System.Collections.Generic;

namespace Bitron.Ecs
{
    public sealed class SystemGroup
    {
        List<ISystem> _systems = new List<ISystem>();

        public SystemGroup Add(ISystem system)
        {
            _systems.Add(system);
            return this;
        }

        public void Run(World world)
        {
            for (var i = 0; i < _systems.Count; i++)
            {
                _systems[i].Run(new Commands(world));
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
