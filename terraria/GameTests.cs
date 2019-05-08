using NUnit.Framework;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace terraria
{
    [TestFixture()]
    public class GameTests
    {
        [Test()]
        public void TestJump()
        {
            var stringMap = new[]{
                "   ",
                " P ",
                "GGG",
            };
            var world = World.Create(stringMap);
            var game = new Game(world);

            game.UpdateOnKeyPress(Keys.Up);

            Assert.That(game.world.map[1, 0], Is.EqualTo(World.Block.Player));
            Assert.That(game.world.map[1, 1], Is.EqualTo(World.Block.Air));
        }

        [Test()]
        public void TestDigGrass()
        {
            var stringMap = new[]{
                "   ",
                " P ",
                "GGG",
            };
            var world = World.Create(stringMap);
            var game = new Game(world);

            game.world.player.inventory.AddItem(new Inventory.Item(Inventory.TypeItem.Shovel));
            //TODO game.world.player.inventory.ChooseItem();
            game.UpdateOnKeyPress(Keys.Down);

            Assert.That(game.world.player.position, Is.EqualTo(new Point(1, 2)));
            Assert.That(game.world.map[1, 1], Is.EqualTo(World.Block.Air));
        }

        [Test()]
        public void TestDigRock()
        {
            var stringMap = new[]{
                "   ",
                " P ",
                "RRR",
            };
            var world = World.Create(stringMap);
            var game = new Game(world);

            game.world.player.inventory.AddItem(new Inventory.Item(Inventory.TypeItem.Pick));
            //TODO game.world.player.inventory.ChooseItem();
            game.UpdateOnKeyPress(Keys.Down);

            Assert.That(game.world.player.position, Is.EqualTo(new Point(1, 2)));
            Assert.That(game.world.map[1, 1], Is.EqualTo(World.Block.Air));
        }

        [Test()]
        public void TestDigWood()
        {
            var stringMap = new[]{
                "   ",
                " P ",
                "TTT",
            };
            var world = World.Create(stringMap);
            var game = new Game(world);

            game.world.player.inventory.AddItem(new Inventory.Item(Inventory.TypeItem.Axe));
            //TODO game.world.player.inventory.ChooseItem();
            game.UpdateOnKeyPress(Keys.Down);

            Assert.That(game.world.player.position, Is.EqualTo(new Point(1, 2)));
            Assert.That(game.world.map[1, 1], Is.EqualTo(World.Block.Air));
        }

    }
}
