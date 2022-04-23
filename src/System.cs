using System.Collections.Generic;
using System.Diagnostics;

namespace RelEcs
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
            foreach (var system in systems)
            {
                system.Run(new Commands(world, system));
            }
        }
    }

    public interface ISystem
    {
        void Run(Commands commands);
    }

    public static class SystemExtensions
    {
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