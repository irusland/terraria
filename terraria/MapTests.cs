using NUnit.Framework;
using System;
using terraria;

namespace tests
{
    [TestFixture]
    public class MapTests
    {
        [Test]
        public void TestMapCreation()
        {
            var strMap = @"
P 
RR";

            var map = World.Create(strMap).map;

            Assert.That(map.GetLength(0), Is.EqualTo(2));
            Assert.That(map.GetLength(1), Is.EqualTo(2));
            Assert.That(map[0, 0], Is.EqualTo(new Player()));
            Assert.That(map[1, 0], Is.EqualTo(new Air()));
            Assert.That(map[0, 1], Is.EqualTo(new Rock()));
            Assert.That(map[1, 1], Is.EqualTo(new Rock()));
        }

        [Test]
        public void TestMapOneByOne()
        {
            var strMap = @"P";

            var map = World.Create(strMap).map;

            Assert.That(map.GetLength(0), Is.EqualTo(1));
            Assert.That(map.GetLength(1), Is.EqualTo(1));
            Assert.That(map[0, 0], Is.EqualTo(new Player()));
        }

        [Test]
        public void TestMapOneByZero()
        {
            var strMap = @"";

            var map = World.Create(strMap).map;

            Assert.That(map.GetLength(0), Is.EqualTo(0));
            Assert.That(map.GetLength(1), Is.EqualTo(1));
        }

        [Test]
        public void TestMapCreationFailure()
        {
            var strMap = @"
F 
  ";

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