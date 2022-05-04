using System;
using System.Runtime.CompilerServices;

namespace RelEcs
{
    public static class QueryExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QueryCommands Has<C1, C2>(this QueryCommands query)
        where C1 : struct
        where C2: struct
        {
            query.Has<C1>().Has<C2>();
            return query;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QueryCommands Has<C1, C2, C3>(this QueryCommands query)
        where C1 : struct
        where C2: struct
        where C3: struct
        {
            query.Has<C1>().Has<C2>().Has<C3>();
            return query;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QueryCommands Has<C1, C2, C3, C4>(this QueryCommands query)
        where C1 : struct
        where C2: struct
        where C3: struct
        where C4: struct
        {
            query.Has<C1>().Has<C2>().Has<C3>().Has<C4>();
            return query;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QueryCommands Has<C1, C2, C3, C4, C5>(this QueryCommands query)
        where C1 : struct
        where C2: struct
        where C3: struct
        where C4: struct
        where C5: struct
        {
            query.Has<C1>().Has<C2>().Has<C3>().Has<C4>().Has<C5>();
            return query;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QueryCommands Has<C1, C2, C3, C4, C5, C6>(this QueryCommands query)
        where C1 : struct
        where C2: struct
        where C3: struct
        where C4: struct
        where C5: struct
        where C6: struct
        {
            query.Has<C1>().Has<C2>().Has<C3>().Has<C4>().Has<C5>().Has<C6>();
            return query;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QueryCommands Has<C1, C2, C3, C4, C5, C6, C7>(this QueryCommands query)
        where C1 : struct
        where C2: struct
        where C3: struct
        where C4: struct
        where C5: struct
        where C6: struct
        where C7: struct
        {
            query.Has<C1>().Has<C2>().Has<C3>().Has<C4>().Has<C5>().Has<C6>().Has<C7>();
            return query;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QueryCommands Has<C1, C2, C3, C4, C5, C6, C7, C8>(this QueryCommands query)
        where C1 : struct
        where C2: struct
        where C3: struct
        where C4: struct
        where C5: struct
        where C6: struct
        where C7: struct
        where C8: struct
        {
            query.Has<C1>().Has<C2>().Has<C3>().Has<C4>().Has<C5>().Has<C6>().Has<C7>().Has<C8>();
            return query;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QueryCommands Not<C1, C2>(this QueryCommands query)
        where C1 : struct
        where C2: struct
        {
            query.Not<C1>().Not<C2>();
            return query;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QueryCommands Not<C1, C2, C3>(this QueryCommands query)
        where C1 : struct
        where C2: struct
        where C3: struct
        {
            query.Not<C1>().Not<C2>().Not<C3>();
            return query;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QueryCommands Not<C1, C2, C3, C4>(this QueryCommands query)
        where C1 : struct
        where C2: struct
        where C3: struct
        where C4: struct
        {
            query.Not<C1>().Not<C2>().Not<C3>().Not<C4>();
            return query;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QueryCommands Not<C1, C2, C3, C4, C5>(this QueryCommands query)
        where C1 : struct
        where C2: struct
        where C3: struct
        where C4: struct
        where C5: struct
        {
            query.Not<C1>().Not<C2>().Not<C3>().Not<C4>().Not<C5>();
            return query;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QueryCommands Not<C1, C2, C3, C4, C5, C6>(this QueryCommands query)
        where C1 : struct
        where C2: struct
        where C3: struct
        where C4: struct
        where C5: struct
        where C6: struct
        {
            query.Not<C1>().Not<C2>().Not<C3>().Not<C4>().Not<C5>().Not<C6>();
            return query;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QueryCommands Not<C1, C2, C3, C4, C5, C6, C7>(this QueryCommands query)
        where C1 : struct
        where C2: struct
        where C3: struct
        where C4: struct
        where C5: struct
        where C6: struct
        where C7: struct
        {
            query.Not<C1>().Not<C2>().Not<C3>().Not<C4>().Not<C5>().Not<C6>().Not<C7>();
            return query;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QueryCommands Not<C1, C2, C3, C4, C5, C6, C7, C8>(this QueryCommands query)
        where C1 : struct
        where C2: struct
        where C3: struct
        where C4: struct
        where C5: struct
        where C6: struct
        where C7: struct
        where C8: struct
        {
            query.Not<C1>().Not<C2>().Not<C3>().Not<C4>().Not<C5>().Not<C6>().Not<C7>().Not<C8>();
            return query;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QueryCommands Any<C1, C2>(this QueryCommands query)
        where C1 : struct
        where C2: struct
        {
            query.Any<C1>().Any<C2>();
            return query;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QueryCommands Any<C1, C2, C3>(this QueryCommands query)
        where C1 : struct
        where C2: struct
        where C3: struct
        {
            query.Any<C1>().Any<C2>().Any<C3>();
            return query;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QueryCommands Any<C1, C2, C3, C4>(this QueryCommands query)
        where C1 : struct
        where C2: struct
        where C3: struct
        where C4: struct
        {
            query.Any<C1>().Any<C2>().Any<C3>().Any<C4>();
            return query;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QueryCommands Any<C1, C2, C3, C4, C5>(this QueryCommands query)
        where C1 : struct
        where C2: struct
        where C3: struct
        where C4: struct
        where C5: struct
        {
            query.Any<C1>().Any<C2>().Any<C3>().Any<C4>().Any<C5>();
            return query;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QueryCommands Any<C1, C2, C3, C4, C5, C6>(this QueryCommands query)
        where C1 : struct
        where C2: struct
        where C3: struct
        where C4: struct
        where C5: struct
        where C6: struct
        {
            query.Any<C1>().Any<C2>().Any<C3>().Any<C4>().Any<C5>().Any<C6>();
            return query;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QueryCommands Any<C1, C2, C3, C4, C5, C6, C7>(this QueryCommands query)
        where C1 : struct
        where C2: struct
        where C3: struct
        where C4: struct
        where C5: struct
        where C6: struct
        where C7: struct
        {
            query.Any<C1>().Any<C2>().Any<C3>().Any<C4>().Any<C5>().Any<C6>().Any<C7>();
            return query;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QueryCommands Any<C1, C2, C3, C4, C5, C6, C7, C8>(this QueryCommands query)
        where C1 : struct
        where C2: struct
        where C3: struct
        where C4: struct
        where C5: struct
        where C6: struct
        where C7: struct
        where C8: struct
        {
            query.Any<C1>().Any<C2>().Any<C3>().Any<C4>().Any<C5>().Any<C6>().Any<C7>().Any<C8>();
            return query;
        }
        
        public delegate void RefAction<C>(ref C c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForEach<C>(this QueryCommands query, RefAction<C> action)
            where C : struct
        {
            var storage = query.World.GetStorage<C>(Identity.None);

            foreach (var entity in query)
            {
                action(ref storage.Get(entity.Identity.Id));
            }
        }

        public delegate void RefAction<C1, C2>(ref C1 c1, ref C2 c2);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForEach<C1, C2>(this QueryCommands query, RefAction<C1, C2> action)
            where C1 : struct
            where C2 : struct
        {
            var storage1 = query.World.GetStorage<C1>(Identity.None);
            var storage2 = query.World.GetStorage<C2>(Identity.None);

            foreach (var entity in query)
            {
                action(ref storage1.Get(entity.Identity.Id), ref storage2.Get(entity.Identity.Id));
            }
        }

        public delegate void RefAction<C1, C2, C3>(ref C1 c1, ref C2 c2, ref C3 c3);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForEach<C1, C2, C3>(this QueryCommands query, RefAction<C1, C2, C3> action)
            where C1 : struct
            where C2 : struct
            where C3 : struct
        {
            var storage1 = query.World.GetStorage<C1>(Identity.None);
            var storage2 = query.World.GetStorage<C2>(Identity.None);
            var storage3 = query.World.GetStorage<C3>(Identity.None);

            foreach (var entity in query)
            {
                action(ref storage1.Get(entity.Identity.Id), ref storage2.Get(entity.Identity.Id), ref storage3.Get(entity.Identity.Id));
            }
        }

        public delegate void RefAction<C1, C2, C3, C4>(ref C1 c1, ref C2 c2, ref C3 c3, ref C4 c4);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForEach<C1, C2, C3, C4>(this QueryCommands query, RefAction<C1, C2, C3, C4> action)
            where C1 : struct
            where C2 : struct
            where C3 : struct
            where C4 : struct
        {
            var storage1 = query.World.GetStorage<C1>(Identity.None);
            var storage2 = query.World.GetStorage<C2>(Identity.None);
            var storage3 = query.World.GetStorage<C3>(Identity.None);
            var storage4 = query.World.GetStorage<C4>(Identity.None);

            foreach (var entity in query)
            {
                action(ref storage1.Get(entity.Identity.Id), ref storage2.Get(entity.Identity.Id), ref storage3.Get(entity.Identity.Id), ref storage4.Get(entity.Identity.Id));
            }
        }

        public delegate void RefAction<C1, C2, C3, C4, C5>(ref C1 c1, ref C2 c2, ref C3 c3, ref C4 c4, ref C5 c5);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForEach<C1, C2, C3, C4, C5>(this QueryCommands query, RefAction<C1, C2, C3, C4, C5> action)
            where C1 : struct
            where C2 : struct
            where C3 : struct
            where C4 : struct
            where C5 : struct
        {
            var storage1 = query.World.GetStorage<C1>(Identity.None);
            var storage2 = query.World.GetStorage<C2>(Identity.None);
            var storage3 = query.World.GetStorage<C3>(Identity.None);
            var storage4 = query.World.GetStorage<C4>(Identity.None);
            var storage5 = query.World.GetStorage<C5>(Identity.None);

            foreach (var entity in query)
            {
                action(ref storage1.Get(entity.Identity.Id), ref storage2.Get(entity.Identity.Id), ref storage3.Get(entity.Identity.Id), ref storage4.Get(entity.Identity.Id), ref storage5.Get(entity.Identity.Id));
            }
        }

        public delegate void RefAction<C1, C2, C3, C4, C5, C6>(ref C1 c1, ref C2 c2, ref C3 c3, ref C4 c4, ref C5 c5, ref C6 c6);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForEach<C1, C2, C3, C4, C5, C6>(this QueryCommands query, RefAction<C1, C2, C3, C4, C5, C6> action)
            where C1 : struct
            where C2 : struct
            where C3 : struct
            where C4 : struct
            where C5 : struct
            where C6 : struct
        {
            var storage1 = query.World.GetStorage<C1>(Identity.None);
            var storage2 = query.World.GetStorage<C2>(Identity.None);
            var storage3 = query.World.GetStorage<C3>(Identity.None);
            var storage4 = query.World.GetStorage<C4>(Identity.None);
            var storage5 = query.World.GetStorage<C5>(Identity.None);
            var storage6 = query.World.GetStorage<C6>(Identity.None);

            foreach (var entity in query)
            {
                action(ref storage1.Get(entity.Identity.Id), ref storage2.Get(entity.Identity.Id), ref storage3.Get(entity.Identity.Id), ref storage4.Get(entity.Identity.Id), ref storage5.Get(entity.Identity.Id), ref storage6.Get(entity.Identity.Id));
            }
        }

        public delegate void RefAction<C1, C2, C3, C4, C5, C6, C7>(ref C1 c1, ref C2 c2, ref C3 c3, ref C4 c4, ref C5 c5, ref C6 c6, ref C7 c7);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForEach<C1, C2, C3, C4, C5, C6, C7>(this QueryCommands query, RefAction<C1, C2, C3, C4, C5, C6, C7> action)
            where C1 : struct
            where C2 : struct
            where C3 : struct
            where C4 : struct
            where C5 : struct
            where C6 : struct
            where C7 : struct
        {
            var storage1 = query.World.GetStorage<C1>(Identity.None);
            var storage2 = query.World.GetStorage<C2>(Identity.None);
            var storage3 = query.World.GetStorage<C3>(Identity.None);
            var storage4 = query.World.GetStorage<C4>(Identity.None);
            var storage5 = query.World.GetStorage<C5>(Identity.None);
            var storage6 = query.World.GetStorage<C6>(Identity.None);
            var storage7 = query.World.GetStorage<C7>(Identity.None);

            foreach (var entity in query)
            {
                action(ref storage1.Get(entity.Identity.Id), ref storage2.Get(entity.Identity.Id), ref storage3.Get(entity.Identity.Id), ref storage4.Get(entity.Identity.Id), ref storage5.Get(entity.Identity.Id), ref storage6.Get(entity.Identity.Id), ref storage7.Get(entity.Identity.Id));
            }
        }

        public delegate void RefAction<C1, C2, C3, C4, C5, C6, C7, C8>(ref C1 c1, ref C2 c2, ref C3 c3, ref C4 c4, ref C5 c5, ref C6 c6, ref C7 c7, ref C8 c8);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForEach<C1, C2, C3, C4, C5, C6, C7, C8>(this QueryCommands query, RefAction<C1, C2, C3, C4, C5, C6, C7, C8> action)
            where C1 : struct
            where C2 : struct
            where C3 : struct
            where C4 : struct
            where C5 : struct
            where C6 : struct
            where C7 : struct
            where C8 : struct
        {
            var storage1 = query.World.GetStorage<C1>(Identity.None);
            var storage2 = query.World.GetStorage<C2>(Identity.None);
            var storage3 = query.World.GetStorage<C3>(Identity.None);
            var storage4 = query.World.GetStorage<C4>(Identity.None);
            var storage5 = query.World.GetStorage<C5>(Identity.None);
            var storage6 = query.World.GetStorage<C6>(Identity.None);
            var storage7 = query.World.GetStorage<C7>(Identity.None);
            var storage8 = query.World.GetStorage<C8>(Identity.None);

            foreach (var entity in query)
            {
                action(ref storage1.Get(entity.Identity.Id), ref storage2.Get(entity.Identity.Id), ref storage3.Get(entity.Identity.Id), ref storage4.Get(entity.Identity.Id), ref storage5.Get(entity.Identity.Id), ref storage6.Get(entity.Identity.Id), ref storage7.Get(entity.Identity.Id), ref storage8.Get(entity.Identity.Id));
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForEach(this QueryCommands query, Action<Entity> action)
        {
            foreach (var entity in query)
            {
                action(entity);
            }
        }

        public delegate void EntityRefAction<C>(Entity entity, ref C c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForEach<C>(this QueryCommands query, EntityRefAction<C> action)
            where C : struct
        {
            var storage = query.World.GetStorage<C>(Identity.None);

            foreach (var entity in query)
            {
                action(entity, ref storage.Get(entity.Identity.Id));
            }
        }

        public delegate void EntityRefAction<C1, C2>(Entity entity, ref C1 c1, ref C2 c2);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForEach<C1, C2>(this QueryCommands query, EntityRefAction<C1, C2> action)
            where C1 : struct
            where C2 : struct
        {
            var storage1 = query.World.GetStorage<C1>(Identity.None);
            var storage2 = query.World.GetStorage<C2>(Identity.None);

            foreach (var entity in query)
            {
                action(entity, ref storage1.Get(entity.Identity.Id), ref storage2.Get(entity.Identity.Id));
            }
        }

        public delegate void EntityRefAction<C1, C2, C3>(Entity entity, ref C1 c1, ref C2 c2, ref C3 c3);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForEach<C1, C2, C3>(this QueryCommands query, EntityRefAction<C1, C2, C3> action)
            where C1 : struct
            where C2 : struct
            where C3 : struct
        {
            var storage1 = query.World.GetStorage<C1>(Identity.None);
            var storage2 = query.World.GetStorage<C2>(Identity.None);
            var storage3 = query.World.GetStorage<C3>(Identity.None);

            foreach (var entity in query)
            {
                action(entity, ref storage1.Get(entity.Identity.Id), ref storage2.Get(entity.Identity.Id), ref storage3.Get(entity.Identity.Id));
            }
        }

        public delegate void EntityRefAction<C1, C2, C3, C4>(Entity entity, ref C1 c1, ref C2 c2, ref C3 c3, ref C4 c4);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForEach<C1, C2, C3, C4>(this QueryCommands query, EntityRefAction<C1, C2, C3, C4> action)
            where C1 : struct
            where C2 : struct
            where C3 : struct
            where C4 : struct
        {
            var storage1 = query.World.GetStorage<C1>(Identity.None);
            var storage2 = query.World.GetStorage<C2>(Identity.None);
            var storage3 = query.World.GetStorage<C3>(Identity.None);
            var storage4 = query.World.GetStorage<C4>(Identity.None);

            foreach (var entity in query)
            {
                action(entity, ref storage1.Get(entity.Identity.Id), ref storage2.Get(entity.Identity.Id), ref storage3.Get(entity.Identity.Id), ref storage4.Get(entity.Identity.Id));
            }
        }

        public delegate void EntityRefAction<C1, C2, C3, C4, C5>(Entity entity, ref C1 c1, ref C2 c2, ref C3 c3, ref C4 c4, ref C5 c5);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForEach<C1, C2, C3, C4, C5>(this QueryCommands query, EntityRefAction<C1, C2, C3, C4, C5> action)
            where C1 : struct
            where C2 : struct
            where C3 : struct
            where C4 : struct
            where C5 : struct
        {
            var storage1 = query.World.GetStorage<C1>(Identity.None);
            var storage2 = query.World.GetStorage<C2>(Identity.None);
            var storage3 = query.World.GetStorage<C3>(Identity.None);
            var storage4 = query.World.GetStorage<C4>(Identity.None);
            var storage5 = query.World.GetStorage<C5>(Identity.None);

            foreach (var entity in query)
            {
                action(entity, ref storage1.Get(entity.Identity.Id), ref storage2.Get(entity.Identity.Id), ref storage3.Get(entity.Identity.Id), ref storage4.Get(entity.Identity.Id), ref storage5.Get(entity.Identity.Id));
            }
        }

        public delegate void EntityRefAction<C1, C2, C3, C4, C5, C6>(Entity entity, ref C1 c1, ref C2 c2, ref C3 c3, ref C4 c4, ref C5 c5, ref C6 c6);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForEach<C1, C2, C3, C4, C5, C6>(this QueryCommands query, EntityRefAction<C1, C2, C3, C4, C5, C6> action)
            where C1 : struct
            where C2 : struct
            where C3 : struct
            where C4 : struct
            where C5 : struct
            where C6 : struct
        {
            var storage1 = query.World.GetStorage<C1>(Identity.None);
            var storage2 = query.World.GetStorage<C2>(Identity.None);
            var storage3 = query.World.GetStorage<C3>(Identity.None);
            var storage4 = query.World.GetStorage<C4>(Identity.None);
            var storage5 = query.World.GetStorage<C5>(Identity.None);
            var storage6 = query.World.GetStorage<C6>(Identity.None);

            foreach (var entity in query)
            {
                action(entity, ref storage1.Get(entity.Identity.Id), ref storage2.Get(entity.Identity.Id), ref storage3.Get(entity.Identity.Id), ref storage4.Get(entity.Identity.Id), ref storage5.Get(entity.Identity.Id), ref storage6.Get(entity.Identity.Id));
            }
        }

        public delegate void EntityRefAction<C1, C2, C3, C4, C5, C6, C7>(Entity entity, ref C1 c1, ref C2 c2, ref C3 c3, ref C4 c4, ref C5 c5, ref C6 c6, ref C7 c7);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForEach<C1, C2, C3, C4, C5, C6, C7>(this QueryCommands query, EntityRefAction<C1, C2, C3, C4, C5, C6, C7> action)
            where C1 : struct
            where C2 : struct
            where C3 : struct
            where C4 : struct
            where C5 : struct
            where C6 : struct
            where C7 : struct
        {
            var storage1 = query.World.GetStorage<C1>(Identity.None);
            var storage2 = query.World.GetStorage<C2>(Identity.None);
            var storage3 = query.World.GetStorage<C3>(Identity.None);
            var storage4 = query.World.GetStorage<C4>(Identity.None);
            var storage5 = query.World.GetStorage<C5>(Identity.None);
            var storage6 = query.World.GetStorage<C6>(Identity.None);
            var storage7 = query.World.GetStorage<C7>(Identity.None);

            foreach (var entity in query)
            {
                action(entity, ref storage1.Get(entity.Identity.Id), ref storage2.Get(entity.Identity.Id), ref storage3.Get(entity.Identity.Id), ref storage4.Get(entity.Identity.Id), ref storage5.Get(entity.Identity.Id), ref storage6.Get(entity.Identity.Id), ref storage7.Get(entity.Identity.Id));
            }
        }

        public delegate void EntityRefAction<C1, C2, C3, C4, C5, C6, C7, C8>(Entity entity, ref C1 c1, ref C2 c2, ref C3 c3, ref C4 c4, ref C5 c5, ref C6 c6, ref C7 c7, ref C8 c8);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForEach<C1, C2, C3, C4, C5, C6, C7, C8>(this QueryCommands query, EntityRefAction<C1, C2, C3, C4, C5, C6, C7, C8> action)
            where C1 : struct
            where C2 : struct
            where C3 : struct
            where C4 : struct
            where C5 : struct
            where C6 : struct
            where C7 : struct
            where C8 : struct
        {
            var storage1 = query.World.GetStorage<C1>(Identity.None);
            var storage2 = query.World.GetStorage<C2>(Identity.None);
            var storage3 = query.World.GetStorage<C3>(Identity.None);
            var storage4 = query.World.GetStorage<C4>(Identity.None);
            var storage5 = query.World.GetStorage<C5>(Identity.None);
            var storage6 = query.World.GetStorage<C6>(Identity.None);
            var storage7 = query.World.GetStorage<C7>(Identity.None);
            var storage8 = query.World.GetStorage<C8>(Identity.None);

            foreach (var entity in query)
            {
                action(entity, ref storage1.Get(entity.Identity.Id), ref storage2.Get(entity.Identity.Id), ref storage3.Get(entity.Identity.Id), ref storage4.Get(entity.Identity.Id), ref storage5.Get(entity.Identity.Id), ref storage6.Get(entity.Identity.Id), ref storage7.Get(entity.Identity.Id), ref storage8.Get(entity.Identity.Id));
            }
        }
    }
}