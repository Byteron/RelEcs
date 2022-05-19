using System.Reflection;
using System.Runtime.CompilerServices;

namespace RelEcs;

public static class TypeIdConverter
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong Value<T>(Identity identity)
    {
        return TypeIdAssigner<T>.Id | (ulong)identity.Generation << 16 | (ulong)identity.Id << 32;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Identity Identity(ulong value)
    {
        return new Identity((int)(value >> 32), (ushort)(value >> 16));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ushort Type(ulong value)
    {
        return (ushort)value;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsTag<T>()
    {
        return TypeIdAssigner<T>.IsTag;
    }

    class TypeIdAssigner
    {
        protected static ushort Counter;
    }

    // ReSharper disable once ClassNeverInstantiated.Local
    class TypeIdAssigner<T> : TypeIdAssigner
    {
        // ReSharper disable once StaticMemberInGenericType
        public static readonly ushort Id;
        // ReSharper disable once StaticMemberInGenericType
        public static readonly bool IsTag;
        

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TypeIdAssigner() 
        {
            Id = ++Counter;
            IsTag = typeof(T).GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).Length == 0;
        }
    }
}