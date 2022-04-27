using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace RelEcs
{
    public sealed class SystemGroup
    {
        readonly List<ISystem> systems = new List<ISystem>();

        public SystemGroup Add(ISystem system)
        {
            systems.Add(system);
            return this;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Run(World world)
        {
            foreach (var system in systems)
            {
                system.Run(new Commands(world, system));
            }
        }
    }

    public interface ISystem
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void Run(Commands commands);
    }

    public static class SystemExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Run(this ISystem system, World world)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            system.Run(new Commands(world, system));

            stopWatch.Stop();
            world.SystemExecutionTimes.Add((system.GetType(), stopWatch.Elapsed));
        }
    }
}