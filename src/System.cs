using System.Collections.Generic;

namespace Bitron.Ecs
{
    public sealed class EcsSystemGroup
    {
        private List<ISystem> _systems = new List<ISystem>();

        public EcsSystemGroup Add(ISystem system)
        {
            _systems.Add(system);
            return this;
        }

        public void Run(World world)
        {
            for(var i = 0; i < _systems.Count; i++)
            {
                _systems[i].Run(new Commands(world));
            }
        }
    }

    public interface ISystem
    {
        void Run(Commands commands);
    }
}
