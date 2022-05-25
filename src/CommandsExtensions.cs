using System.Runtime.CompilerServices;

namespace RelEcs;

public static class CommandsExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static QueryCommands<C> Query<C>(this Commands commands)
    {
        return new QueryCommands<C>(commands.World);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static QueryCommands<C1, C2> Query<C1, C2>(this Commands commands)
    {
        return new QueryCommands<C1, C2>(commands.World);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static QueryCommands<C1, C2, C3> Query<C1, C2, C3>(this Commands commands)
    {
        return new QueryCommands<C1, C2, C3>(commands.World);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static QueryCommands<C1, C2, C3, C4> Query<C1, C2, C3, C4>(this Commands commands)
    {
        return new QueryCommands<C1, C2, C3, C4>(commands.World);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static QueryCommands<C1, C2, C3, C4, C5> Query<C1, C2, C3, C4, C5>(this Commands commands)
    {
        return new QueryCommands<C1, C2, C3, C4, C5>(commands.World);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static QueryCommands<C1, C2, C3, C4, C5, C6> Query<C1, C2, C3, C4, C5, C6>(this Commands commands)
    {
        return new QueryCommands<C1, C2, C3, C4, C5, C6>(commands.World);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static QueryCommands<C1, C2, C3, C4, C5, C6, C7> Query<C1, C2, C3, C4, C5, C6, C7>(this Commands commands)
    {
        return new QueryCommands<C1, C2, C3, C4, C5, C6, C7>(commands.World);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static QueryCommands<C1, C2, C3, C4, C5, C6, C7, C8> Query<C1, C2, C3, C4, C5, C6, C7, C8>(this Commands commands)
    {
        return new QueryCommands<C1, C2, C3, C4, C5, C6, C7, C8>(commands.World);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static QueryCommands<C1, C2, C3, C4, C5, C6, C7, C8, C9> Query<C1, C2, C3, C4, C5, C6, C7, C8, C9>(this Commands commands)
    {
        return new QueryCommands<C1, C2, C3, C4, C5, C6, C7, C8, C9>(commands.World);
    }
}