using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RelEcs;

public class Query
{
    internal readonly Mask Mask;
    
    public readonly List<Table> Tables;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Query( Mask mask, List<Table> tables)
    {
        Mask = mask;
        Tables = tables;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AddTable(Table table)
    {
        Tables.Add(table);
    }
}