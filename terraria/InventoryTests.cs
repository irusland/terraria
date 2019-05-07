using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static terraria.Inventory;

namespace terraria
{
    [TestFixture()]
    class InventoryTests
    {
        public void TestAddItem()
        {
            var inventory = new List<InventorySlot>();
            var number = 1;
            var Item = new Item(1, "Axe", "01");
            
        }

        public void TestRemoveItem()
        {
            var inventory = new List<InventorySlot>();
            var number = 1;
            var Item = new Item(1, "Axe", "01");

        }

        public void TestAddZeroNumber()
        {
            var inventory = new List<InventorySlot>();
            var number = 0;
            var Item = new Item(1, "Axe", "01");

        }

        public void TestAddNegativeNumber()
        {
            var inventory = new List<InventorySlot>();
            var number = -1;
            var Item = new Item(1, "Axe", "01");

        }


        public void TestRemoveZeroNumber()
        {
            var inventory = new List<InventorySlot>();
            var number = 0;
            var Item = new Item(1, "Axe", "01");

        }

        public void TestRemoveNegativeNumber()
        {
            var inventory = new List<InventorySlot>();
            var number = -1;
            var Item = new Item(1, "Axe", "01");

        }
    }
}
