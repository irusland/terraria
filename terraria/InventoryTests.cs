﻿using NUnit.Framework;
using static terraria.Inventory;

namespace terraria
{
    [TestFixture()]
    class InventoryTests
    {
        [Test]
        public void TestAddItem()
        {
            var inventory = new Inventory();
            var item = new Item(TypeItem.Axe);
            inventory.AddItem(item);
            Assert.That(inventory.ItemFromInventoryExists(item), Is.True);
            
//        }

        [Test]
        public void TestAddManyItems()
        {
            var inventory = new Inventory();
            var item1 = new Item(TypeItem.Axe);
            var item2 = new Item(TypeItem.Dirt);
            var item3 = new Item(TypeItem.HealingSlave);

            inventory.AddItem(item1);
            inventory.AddItem(item2);
            inventory.AddItem(item3);

            Assert.That(inventory.ItemFromInventoryExists(item1)
                && inventory.ItemFromInventoryExists(item2) 
                && inventory.ItemFromInventoryExists(item3), Is.True);

        }

        [Test]
        public void TestRemoveItem()
        {
            
            var inventory = new Inventory();
            var item = new Item(TypeItem.Axe);
            inventory.RemoveItem(item);
            Assert.That(inventory.ItemFromInventoryExists(item), Is.False);

        }

        [Test]
        public void TestRemoveManyItems()
        {

            var inventory = new Inventory();
            var item1 = new Item(TypeItem.Axe);
            var item2 = new Item(TypeItem.Dirt);
            var item3 = new Item(TypeItem.HealingSlave);

            inventory.RemoveItem(item1);
            inventory.RemoveItem(item2);
            inventory.RemoveItem(item3);

            Assert.That(inventory.ItemFromInventoryExists(item1)
                || inventory.ItemFromInventoryExists(item2)
                || inventory.ItemFromInventoryExists(item3), Is.False);

//        }

        [Test]
        public void TestAddZeroNumber()
        {
            var inventory = new Inventory();
            var number = 0;
            var item = new Item(TypeItem.Axe);
            inventory.AddItem(item, number);
            Assert.That(inventory.ItemFromInventoryExists(item), Is.False);

//        }

        [Test]
        public void TestAddNegativeNumber()
        {
            var inventory = new Inventory();
            var number = -1;
            var item = new Item(TypeItem.Axe);
            inventory.AddItem(item, number);
            Assert.That(inventory.ItemFromInventoryExists(item), Is.False);

//        }

        [Test]
        public void TestRemoveZeroNumber()
        {
            var inventory = new Inventory();
            var number = 0;
            var item = new Item(TypeItem.Axe);
            inventory.RemoveItem(item, number);
            Assert.That(inventory.ItemFromInventoryExists(item, number), Is.True);
        }

        [Test]
        public void TestRemoveNegativeNumber()
        {
            var inventory = new Inventory();
            var number = -1;
            var item = new Item(TypeItem.Axe);
            inventory.RemoveItem(item, number);
            Assert.That(inventory.ItemFromInventoryExists(item, number), Is.True);

//        }
//    }
//}
