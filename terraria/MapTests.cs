using NUnit.Framework;
using System;

namespace terraria
{
    [TestFixture]
    public class MapTests
    {
        [Test()]
        public void TestMapCreation()
        {
            var strMap = new[]{
                "P ",
                "RR"
            };

            var map = World.Create(strMap).map;

            Assert.That(map.GetLength(0), Is.EqualTo(2));
            Assert.That(map.GetLength(1), Is.EqualTo(2));
            Assert.That(map[0, 0], Is.EqualTo(World.Block.Player));
            Assert.That(map[1, 0], Is.EqualTo(World.Block.Air));
            Assert.That(map[0, 1], Is.EqualTo(World.Block.Rock));
            Assert.That(map[1, 1], Is.EqualTo(World.Block.Rock));
        }

        [Test()]
        public void TestMapOneByOne()
        {
            var strMap = new[]{
                "P"
            };

            var map = World.Create(strMap).map;

            Assert.That(map.GetLength(0), Is.EqualTo(1));
            Assert.That(map.GetLength(1), Is.EqualTo(1));
            Assert.That(map[0, 0], Is.EqualTo(World.Block.Player));
        }

        [Test()]
        public void TestMapOneByZero()
        {
            var strMap = new[]{
                ""
            };

            var map = World.Create(strMap).map;

            Assert.That(map.GetLength(0), Is.EqualTo(0));
            Assert.That(map.GetLength(1), Is.EqualTo(1));
        }

        [Test()]
        public void TestMapZeroByZero()
        {
            var strMap = new string[0];

            var map = World.Create(strMap).map;

            Assert.That(map.GetLength(0), Is.EqualTo(0));
            Assert.That(map.GetLength(1), Is.EqualTo(0));
        }

        [Test()]
        public void TestMapCreationFailure()
        {
            var strMap = new[]{
                "F ",
                "  "
            };

            try
            {
                var map = World.Create(strMap).map;
            }
            catch (FormatException e)
            {
                Assert.True(true);
                return;
            }
            Assert.True(false);
        }
    }
}
