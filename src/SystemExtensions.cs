using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace RelEcs;

public static class SystemExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Run(this ASystem aSystem, World world)
    {
        var stopWatch = new Stopwatch();
        stopWatch.Start();
        
        aSystem.World = world;
        aSystem.Run();

        stopWatch.Stop();
        world.SystemExecutionTimes.Add((aSystem.GetType(), stopWatch.Elapsed));
    }
}