using RelEcs;
using Xunit;

namespace RelEcs.Tests
{
    public class QueryTests
    {
        private sealed class Position
        {
            public float X;
            public float Y;
        }

        private sealed class Velocity
        {
            public float X;
            public float Y;
        }

        private sealed class Health
        {
            public int Value;
        }

        [Fact]
        public void SingleComponentQuery_ReturnsComponents()
        {
            var world = new World();

            world.Spawn().Add(new Position { X = 1, Y = 2 });
            world.Spawn().Add(new Position { X = 3, Y = 4 });

            var query = world.Query<Position>().Build();

            var components = new System.Collections.Generic.List<Position>();
            foreach (var position in query)
            {
                components.Add(position);
            }

            Assert.Equal(2, components.Count);
            Assert.Contains(components, p => p.X == 1 && p.Y == 2);
            Assert.Contains(components, p => p.X == 3 && p.Y == 4);
        }

        [Fact]
        public void MultiComponentQuery_ReturnsTuples()
        {
            var world = new World();

            var e1 = world.Spawn()
                .Add(new Position { X = 1, Y = 2 })
                .Add(new Velocity { X = 10, Y = 20 })
                .Id();

            world.Spawn().Add(new Position { X = 3, Y = 4 });

            var query = world.Query<Position, Velocity>().Build();

            Assert.True(query.Has(e1));

            var (pos, vel) = query.Get(e1);
            Assert.Equal(1, pos.X);
            Assert.Equal(2, pos.Y);
            Assert.Equal(10, vel.X);
            Assert.Equal(20, vel.Y);
        }

        [Fact]
        public void QueryEnumerator_IteratesAllMatchingEntities()
        {
            var world = new World();

            world.Spawn().Add(new Position { X = 1, Y = 2 });
            world.Spawn().Add(new Position { X = 3, Y = 4 });
            world.Spawn().Add(new Health { Value = 100 });

            var query = world.Query<Position>().Build();

            var count = 0;
            foreach (var position in query)
            {
                Assert.IsType<Position>(position);
                count++;
            }

            Assert.Equal(2, count);
        }
    }
}
