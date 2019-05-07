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
        public void TestEmptyInventory()
        {
            var inventory = new List<InventorySlot>();
            var number = 1;
            var Item = new Item(1, "Axe", "01");

        }
    }
}
