using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace RelEcs;

public static class SystemExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Run(this System system, World world)
    {
        var stopWatch = new Stopwatch();
        stopWatch.Start();
        
        system.World = world;
        system.Run();

        stopWatch.Stop();
        world.SystemExecutionTimes.Add((system.GetType(), stopWatch.Elapsed));
    }
}