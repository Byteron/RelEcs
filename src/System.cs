using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RelEcs
{
    public interface ISystem
    {
        void Run(World world);
    }

    public interface ITriggerSystem { }
    
    public interface ITriggerSystem<T> : ITriggerSystem
    {
        void Run(World world, T trigger);
    }
    
    public sealed class SystemGroup
    {
        readonly List<ISystem> _systems = new();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SystemGroup Add(ISystem aSystem)
        {
            _systems.Add(aSystem);
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Run(World world)
        {
            foreach (var system in _systems)
            {
                system.Run(world);
                world.RunTriggerSystems();
            }
        }
    }
}