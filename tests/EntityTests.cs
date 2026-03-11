using RelEcs;
using Xunit;

namespace RelEcs.Tests
{
    public class EntityTests
    {
        [Fact]
        public void Identity_Equality_Works()
        {
            var id1 = new Identity(1, 2);
            var id2 = new Identity(1, 2);
            var id3 = new Identity(2, 2);

            Assert.True(id1 == id2);
            Assert.False(id1 != id2);
            Assert.False(id1 == id3);
            Assert.True(id1 != id3);
            Assert.Equal(id1.GetHashCode(), id2.GetHashCode());
        }

        [Fact]
        public void Entity_Equality_Works()
        {
            var identity = new Identity(1, 1);
            var e1 = new Entity(identity);
            var e2 = new Entity(identity);
            var e3 = new Entity(new Identity(2, 1));

            Assert.True(e1 == e2);
            Assert.False(e1 != e2);
            Assert.False(e1 == e3);
            Assert.True(e1 != e3);
            Assert.Equal(e1.GetHashCode(), e2.GetHashCode());
        }

        [Fact]
        public void Entity_StaticInstances_HaveExpectedFlags()
        {
            Assert.True(Entity.None.IsNone);
            Assert.False(Entity.None.IsAny);

            Assert.True(Entity.Any.IsAny);
            Assert.False(Entity.Any.IsNone);
        }
    }
}
