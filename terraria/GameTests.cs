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
            var stringMap = @"
   
 P 
GGG";
            var world = World.Create(stringMap);
            var game = new Game(world);

            //game.UpdateOnKeyPress(Keys.Up);

            Assert.That(game.world.map[1, 0], Is.EqualTo(new Player()));
            Assert.That(game.world.map[1, 1], Is.EqualTo(new Air()));
        }

        [Test()]
        public void TestDigGrass()
        {
            var stringMap = @"
   
 P 
GGG";
            var world = World.Create(stringMap);
            var game = new Game(world);
            var item = new Inventory.Item(Inventory.TypeItem.Shovel);
            //game.world.player.inventory.AddItem(item);
            //game.world.player.inventory.SelectItem(game.world.player.inventory, item);
            //game.UpdateOnKeyPress(Keys.Down);

            //Assert.That(game.world.player.position, Is.EqualTo(new Point(1, 2)));
            Assert.That(game.world.map[1, 1], Is.EqualTo(new Air()));
        }

        [Test()]
        public void TestDigRock()
        {
            var stringMap = @"
   
 P 
RRR";
            var world = World.Create(stringMap);
            var game = new Game(world);

            //game.world.player.inventory.AddItem(new Inventory.Item(Inventory.TypeItem.Pick));
            ////TODO game.world.player.inventory.ChooseItem();
            //game.UpdateOnKeyPress(Keys.Down);

            //Assert.That(game.world.player.position, Is.EqualTo(new Point(1, 2)));
            Assert.That(game.world.map[1, 1], Is.EqualTo(new Air()));
        }

        [Test()]
        public void TestDigWood()
        {
            var stringMap = @"
   
 P 
TTT";
            var world = World.Create(stringMap);
            var game = new Game(world);

            //game.world.player.inventory.AddItem(new Inventory.Item(Inventory.TypeItem.Axe));
            ////TODO game.world.player.inventory.ChooseItem();
            //game.UpdateOnKeyPress(Keys.Down);

            //Assert.That(game.world.player.position, Is.EqualTo(new Point(1, 2)));
            Assert.That(game.world.map[1, 1], Is.EqualTo(new Air()));
        }

    }
}
