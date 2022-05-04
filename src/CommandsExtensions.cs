using System.Runtime.CompilerServices;

namespace RelEcs
{
    public static class CommandsExtensions
    {
        public delegate void RefAction<C>(ref C c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForEach<C>(this Commands commands, RefAction<C> action)
            where C : struct
        {
            var query = commands.Query().Has<C>();

            var storage = commands.GetStorage<C>(Identity.None);

            foreach (var entity in query)
            {
                action(ref storage.Get(entity.Identity.Id));
            }
        }

        public delegate void RefAction<C1, C2>(ref C1 c1, ref C2 c2);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForEach<C1, C2>(this Commands commands, RefAction<C1, C2> action)
            where C1 : struct
            where C2 : struct
        {
            var query = commands.Query().Has<C1>().Has<C2>();

            var storage1 = commands.GetStorage<C1>(Identity.None);
            var storage2 = commands.GetStorage<C2>(Identity.None);

            foreach (var entity in query)
            {
                action(ref storage1.Get(entity.Identity.Id), ref storage2.Get(entity.Identity.Id));
            }
        }

        public delegate void RefAction<C1, C2, C3>(ref C1 c1, ref C2 c2, ref C3 c3);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForEach<C1, C2, C3>(this Commands commands, RefAction<C1, C2, C3> action)
            where C1 : struct
            where C2 : struct
            where C3 : struct
        {
            var query = commands.Query().Has<C1>().Has<C2>().Has<C3>();

            var storage1 = commands.GetStorage<C1>(Identity.None);
            var storage2 = commands.GetStorage<C2>(Identity.None);
            var storage3 = commands.GetStorage<C3>(Identity.None);

            foreach (var entity in query)
            {
                action(ref storage1.Get(entity.Identity.Id), ref storage2.Get(entity.Identity.Id), ref storage3.Get(entity.Identity.Id));
            }
        }

        public delegate void RefAction<C1, C2, C3, C4>(ref C1 c1, ref C2 c2, ref C3 c3, ref C4 c4);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForEach<C1, C2, C3, C4>(this Commands commands, RefAction<C1, C2, C3, C4> action)
            where C1 : struct
            where C2 : struct
            where C3 : struct
            where C4 : struct
        {
            var query = commands.Query().Has<C1>().Has<C2>().Has<C3>().Has<C4>();

            var storage1 = commands.GetStorage<C1>(Identity.None);
            var storage2 = commands.GetStorage<C2>(Identity.None);
            var storage3 = commands.GetStorage<C3>(Identity.None);
            var storage4 = commands.GetStorage<C4>(Identity.None);

            foreach (var entity in query)
            {
                action(ref storage1.Get(entity.Identity.Id), ref storage2.Get(entity.Identity.Id), ref storage3.Get(entity.Identity.Id), ref storage4.Get(entity.Identity.Id));
            }
        }

        public delegate void RefAction<C1, C2, C3, C4, C5>(ref C1 c1, ref C2 c2, ref C3 c3, ref C4 c4, ref C5 c5);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForEach<C1, C2, C3, C4, C5>(this Commands commands, RefAction<C1, C2, C3, C4, C5> action)
            where C1 : struct
            where C2 : struct
            where C3 : struct
            where C4 : struct
            where C5 : struct
        {
            var query = commands.Query().Has<C1>().Has<C2>().Has<C3>().Has<C4>().Has<C5>();

            var storage1 = commands.GetStorage<C1>(Identity.None);
            var storage2 = commands.GetStorage<C2>(Identity.None);
            var storage3 = commands.GetStorage<C3>(Identity.None);
            var storage4 = commands.GetStorage<C4>(Identity.None);
            var storage5 = commands.GetStorage<C5>(Identity.None);

            foreach (var entity in query)
            {
                action(ref storage1.Get(entity.Identity.Id), ref storage2.Get(entity.Identity.Id), ref storage3.Get(entity.Identity.Id), ref storage4.Get(entity.Identity.Id), ref storage5.Get(entity.Identity.Id));
            }
        }

        public delegate void RefAction<C1, C2, C3, C4, C5, C6>(ref C1 c1, ref C2 c2, ref C3 c3, ref C4 c4, ref C5 c5, ref C6 c6);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForEach<C1, C2, C3, C4, C5, C6>(this Commands commands, RefAction<C1, C2, C3, C4, C5, C6> action)
            where C1 : struct
            where C2 : struct
            where C3 : struct
            where C4 : struct
            where C5 : struct
            where C6 : struct
        {
            var query = commands.Query().Has<C1>().Has<C2>().Has<C3>().Has<C4>().Has<C5>().Has<C6>();

            var storage1 = commands.GetStorage<C1>(Identity.None);
            var storage2 = commands.GetStorage<C2>(Identity.None);
            var storage3 = commands.GetStorage<C3>(Identity.None);
            var storage4 = commands.GetStorage<C4>(Identity.None);
            var storage5 = commands.GetStorage<C5>(Identity.None);
            var storage6 = commands.GetStorage<C6>(Identity.None);

            foreach (var entity in query)
            {
                action(ref storage1.Get(entity.Identity.Id), ref storage2.Get(entity.Identity.Id), ref storage3.Get(entity.Identity.Id), ref storage4.Get(entity.Identity.Id), ref storage5.Get(entity.Identity.Id), ref storage6.Get(entity.Identity.Id));
            }
        }

        public delegate void RefAction<C1, C2, C3, C4, C5, C6, C7>(ref C1 c1, ref C2 c2, ref C3 c3, ref C4 c4, ref C5 c5, ref C6 c6, ref C7 c7);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForEach<C1, C2, C3, C4, C5, C6, C7>(this Commands commands, RefAction<C1, C2, C3, C4, C5, C6, C7> action)
            where C1 : struct
            where C2 : struct
            where C3 : struct
            where C4 : struct
            where C5 : struct
            where C6 : struct
            where C7 : struct
        {
            var query = commands.Query().Has<C1>().Has<C2>().Has<C3>().Has<C4>().Has<C5>().Has<C6>().Has<C7>();

            var storage1 = commands.GetStorage<C1>(Identity.None);
            var storage2 = commands.GetStorage<C2>(Identity.None);
            var storage3 = commands.GetStorage<C3>(Identity.None);
            var storage4 = commands.GetStorage<C4>(Identity.None);
            var storage5 = commands.GetStorage<C5>(Identity.None);
            var storage6 = commands.GetStorage<C6>(Identity.None);
            var storage7 = commands.GetStorage<C7>(Identity.None);

            foreach (var entity in query)
            {
                action(ref storage1.Get(entity.Identity.Id), ref storage2.Get(entity.Identity.Id), ref storage3.Get(entity.Identity.Id), ref storage4.Get(entity.Identity.Id), ref storage5.Get(entity.Identity.Id), ref storage6.Get(entity.Identity.Id), ref storage7.Get(entity.Identity.Id));
            }
        }

        public delegate void RefAction<C1, C2, C3, C4, C5, C6, C7, C8>(ref C1 c1, ref C2 c2, ref C3 c3, ref C4 c4, ref C5 c5, ref C6 c6, ref C7 c7, ref C8 c8);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForEach<C1, C2, C3, C4, C5, C6, C7, C8>(this Commands commands, RefAction<C1, C2, C3, C4, C5, C6, C7, C8> action)
            where C1 : struct
            where C2 : struct
            where C3 : struct
            where C4 : struct
            where C5 : struct
            where C6 : struct
            where C7 : struct
            where C8 : struct
        {
            var query = commands.Query().Has<C1>().Has<C2>().Has<C3>().Has<C4>().Has<C5>().Has<C6>().Has<C7>().Has<C8>();

            var storage1 = commands.GetStorage<C1>(Identity.None);
            var storage2 = commands.GetStorage<C2>(Identity.None);
            var storage3 = commands.GetStorage<C3>(Identity.None);
            var storage4 = commands.GetStorage<C4>(Identity.None);
            var storage5 = commands.GetStorage<C5>(Identity.None);
            var storage6 = commands.GetStorage<C6>(Identity.None);
            var storage7 = commands.GetStorage<C7>(Identity.None);
            var storage8 = commands.GetStorage<C8>(Identity.None);

            foreach (var entity in query)
            {
                action(ref storage1.Get(entity.Identity.Id), ref storage2.Get(entity.Identity.Id), ref storage3.Get(entity.Identity.Id), ref storage4.Get(entity.Identity.Id), ref storage5.Get(entity.Identity.Id), ref storage6.Get(entity.Identity.Id), ref storage7.Get(entity.Identity.Id), ref storage8.Get(entity.Identity.Id));
            }
        }

        public delegate void EntityRefAction(Entity entity);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForEach(this Commands commands, EntityRefAction action)
        {
            var query = commands.Query();

            foreach (var entity in query)
            {
                action(entity);
            }
        }

        public delegate void EntityRefAction<C>(Entity entity, ref C c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForEach<C>(this Commands commands, EntityRefAction<C> action)
            where C : struct
        {
            var query = commands.Query().Has<C>();

            var storage = commands.GetStorage<C>(Identity.None);

            foreach (var entity in query)
            {
                action(entity, ref storage.Get(entity.Identity.Id));
            }
        }

        public delegate void EntityRefAction<C1, C2>(Entity entity, ref C1 c1, ref C2 c2);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForEach<C1, C2>(this Commands commands, EntityRefAction<C1, C2> action)
            where C1 : struct
            where C2 : struct
        {
            var query = commands.Query().Has<C1>().Has<C2>();

            var storage1 = commands.GetStorage<C1>(Identity.None);
            var storage2 = commands.GetStorage<C2>(Identity.None);

            foreach (var entity in query)
            {
                action(entity, ref storage1.Get(entity.Identity.Id), ref storage2.Get(entity.Identity.Id));
            }
        }

        public delegate void EntityRefAction<C1, C2, C3>(Entity entity, ref C1 c1, ref C2 c2, ref C3 c3);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForEach<C1, C2, C3>(this Commands commands, EntityRefAction<C1, C2, C3> action)
            where C1 : struct
            where C2 : struct
            where C3 : struct
        {
            var query = commands.Query().Has<C1>().Has<C2>().Has<C3>();

            var storage1 = commands.GetStorage<C1>(Identity.None);
            var storage2 = commands.GetStorage<C2>(Identity.None);
            var storage3 = commands.GetStorage<C3>(Identity.None);

            foreach (var entity in query)
            {
                action(entity, ref storage1.Get(entity.Identity.Id), ref storage2.Get(entity.Identity.Id), ref storage3.Get(entity.Identity.Id));
            }
        }

        public delegate void EntityRefAction<C1, C2, C3, C4>(Entity entity, ref C1 c1, ref C2 c2, ref C3 c3, ref C4 c4);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForEach<C1, C2, C3, C4>(this Commands commands, EntityRefAction<C1, C2, C3, C4> action)
            where C1 : struct
            where C2 : struct
            where C3 : struct
            where C4 : struct
        {
            var query = commands.Query().Has<C1>().Has<C2>().Has<C3>().Has<C4>();

            var storage1 = commands.GetStorage<C1>(Identity.None);
            var storage2 = commands.GetStorage<C2>(Identity.None);
            var storage3 = commands.GetStorage<C3>(Identity.None);
            var storage4 = commands.GetStorage<C4>(Identity.None);

            foreach (var entity in query)
            {
                action(entity, ref storage1.Get(entity.Identity.Id), ref storage2.Get(entity.Identity.Id), ref storage3.Get(entity.Identity.Id), ref storage4.Get(entity.Identity.Id));
            }
        }

        public delegate void EntityRefAction<C1, C2, C3, C4, C5>(Entity entity, ref C1 c1, ref C2 c2, ref C3 c3, ref C4 c4, ref C5 c5);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForEach<C1, C2, C3, C4, C5>(this Commands commands, EntityRefAction<C1, C2, C3, C4, C5> action)
            where C1 : struct
            where C2 : struct
            where C3 : struct
            where C4 : struct
            where C5 : struct
        {
            var query = commands.Query().Has<C1>().Has<C2>().Has<C3>().Has<C4>().Has<C5>();

            var storage1 = commands.GetStorage<C1>(Identity.None);
            var storage2 = commands.GetStorage<C2>(Identity.None);
            var storage3 = commands.GetStorage<C3>(Identity.None);
            var storage4 = commands.GetStorage<C4>(Identity.None);
            var storage5 = commands.GetStorage<C5>(Identity.None);

            foreach (var entity in query)
            {
                action(entity, ref storage1.Get(entity.Identity.Id), ref storage2.Get(entity.Identity.Id), ref storage3.Get(entity.Identity.Id), ref storage4.Get(entity.Identity.Id), ref storage5.Get(entity.Identity.Id));
            }
        }

        public delegate void EntityRefAction<C1, C2, C3, C4, C5, C6>(Entity entity, ref C1 c1, ref C2 c2, ref C3 c3, ref C4 c4, ref C5 c5, ref C6 c6);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForEach<C1, C2, C3, C4, C5, C6>(this Commands commands, EntityRefAction<C1, C2, C3, C4, C5, C6> action)
            where C1 : struct
            where C2 : struct
            where C3 : struct
            where C4 : struct
            where C5 : struct
            where C6 : struct
        {
            var query = commands.Query().Has<C1>().Has<C2>().Has<C3>().Has<C4>().Has<C5>().Has<C6>();

            var storage1 = commands.GetStorage<C1>(Identity.None);
            var storage2 = commands.GetStorage<C2>(Identity.None);
            var storage3 = commands.GetStorage<C3>(Identity.None);
            var storage4 = commands.GetStorage<C4>(Identity.None);
            var storage5 = commands.GetStorage<C5>(Identity.None);
            var storage6 = commands.GetStorage<C6>(Identity.None);

            foreach (var entity in query)
            {
                action(entity, ref storage1.Get(entity.Identity.Id), ref storage2.Get(entity.Identity.Id), ref storage3.Get(entity.Identity.Id), ref storage4.Get(entity.Identity.Id), ref storage5.Get(entity.Identity.Id), ref storage6.Get(entity.Identity.Id));
            }
        }

        public delegate void EntityRefAction<C1, C2, C3, C4, C5, C6, C7>(Entity entity, ref C1 c1, ref C2 c2, ref C3 c3, ref C4 c4, ref C5 c5, ref C6 c6, ref C7 c7);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForEach<C1, C2, C3, C4, C5, C6, C7>(this Commands commands, EntityRefAction<C1, C2, C3, C4, C5, C6, C7> action)
            where C1 : struct
            where C2 : struct
            where C3 : struct
            where C4 : struct
            where C5 : struct
            where C6 : struct
            where C7 : struct
        {
            var query = commands.Query().Has<C1>().Has<C2>().Has<C3>().Has<C4>().Has<C5>().Has<C6>().Has<C7>();

            var storage1 = commands.GetStorage<C1>(Identity.None);
            var storage2 = commands.GetStorage<C2>(Identity.None);
            var storage3 = commands.GetStorage<C3>(Identity.None);
            var storage4 = commands.GetStorage<C4>(Identity.None);
            var storage5 = commands.GetStorage<C5>(Identity.None);
            var storage6 = commands.GetStorage<C6>(Identity.None);
            var storage7 = commands.GetStorage<C7>(Identity.None);

            foreach (var entity in query)
            {
                action(entity, ref storage1.Get(entity.Identity.Id), ref storage2.Get(entity.Identity.Id), ref storage3.Get(entity.Identity.Id), ref storage4.Get(entity.Identity.Id), ref storage5.Get(entity.Identity.Id), ref storage6.Get(entity.Identity.Id), ref storage7.Get(entity.Identity.Id));
            }
        }

        public delegate void EntityRefAction<C1, C2, C3, C4, C5, C6, C7, C8>(Entity entity, ref C1 c1, ref C2 c2, ref C3 c3, ref C4 c4, ref C5 c5, ref C6 c6, ref C7 c7, ref C8 c8);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForEach<C1, C2, C3, C4, C5, C6, C7, C8>(this Commands commands, EntityRefAction<C1, C2, C3, C4, C5, C6, C7, C8> action)
            where C1 : struct
            where C2 : struct
            where C3 : struct
            where C4 : struct
            where C5 : struct
            where C6 : struct
            where C7 : struct
            where C8 : struct
        {
            var query = commands.Query().Has<C1>().Has<C2>().Has<C3>().Has<C4>().Has<C5>().Has<C6>().Has<C7>().Has<C8>();

            var storage1 = commands.GetStorage<C1>(Identity.None);
            var storage2 = commands.GetStorage<C2>(Identity.None);
            var storage3 = commands.GetStorage<C3>(Identity.None);
            var storage4 = commands.GetStorage<C4>(Identity.None);
            var storage5 = commands.GetStorage<C5>(Identity.None);
            var storage6 = commands.GetStorage<C6>(Identity.None);
            var storage7 = commands.GetStorage<C7>(Identity.None);
            var storage8 = commands.GetStorage<C8>(Identity.None);

            foreach (var entity in query)
            {
                action(entity, ref storage1.Get(entity.Identity.Id), ref storage2.Get(entity.Identity.Id), ref storage3.Get(entity.Identity.Id), ref storage4.Get(entity.Identity.Id), ref storage5.Get(entity.Identity.Id), ref storage6.Get(entity.Identity.Id), ref storage7.Get(entity.Identity.Id), ref storage8.Get(entity.Identity.Id));
            }
        }
    }
}