using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace terraria
{
    class Inventory
    {
        public enum TypeItem
        {
            Axe,
            Pick,
            Shovel,
            Shield
        }

        private List<InventorySlot> inventory = new List<InventorySlot>();
        private readonly int maxItemCount = 25;

        public class InventorySlot
        {
            private int itemCount;
            private Item item;
            private int slotId;
            public TypeItem
            public int ItemCount { get => itemCount; set => itemCount = value; }
            public Item Item { get => item; set => item = value; }
            public int SlotId { get => slotId; set => slotId = value; }
        }

        public class Item
        {
            private int id;
            private string stringId;
            private string name;
            private string localizedName;
            // private Image texture;
            private Dictionary<string, double> itemProperties = new Dictionary<string, double>();

            public Item(int id, string name, string strId)
            {
                this.id = id;
                this.name = name;
                this.stringId = strId;
                LoadItem();
            }

            public void LoadItem()
            {
                this.localizedName = this.name;
            }

            public int Id { get => id; set => id = value; }
            public string StringId { get => stringId; set => stringId = value; }
            public string Name { get => name; set => name = value; }
            public string LocalizedName { get => localizedName; set => localizedName = value; }
            // public Image Texture { get => Texture; set => Texture = value; }
            public Dictionary<string, double> ItemProperties { get => itemProperties; set => itemProperties = value; }
        }

        public void AddItem(Item item, int number = 1)
        {
            var slots = inventory.Where(p => (p.Item == item) && (p.ItemCount < maxItemCount)).Select(p => p);
            foreach (var slot in slots)
            {
                var freePosition = maxItemCount - slot.ItemCount;

                if (number > freePosition)
                {
                    slot.ItemCount = maxItemCount;
                }
                else
                {
                    slot.ItemCount += number;
                }

                number -= freePosition;
                if (number <= 0)
                    break;
            }
            while (number > 0)
            {
                inventory.Add(new InventorySlot()
                {
                    Item = item,
                    ItemCount = (number >= maxItemCount ? maxItemCount : number),
                    SlotId = inventory.Count
                });

                number -= number >= maxItemCount ? maxItemCount : number;
            }
            // Game.IsInventoryUpdate = true;
        }

        public void RemoveItem(Item item, int number = 1)
        {
            var slots = inventory.Where(p => (p.Item.Id == item.Id)).OrderBy(p => p.ItemCount).Select(p => p);
            if (ItemFromInventoryExists(item, number))
            {
                foreach (var slot in slots)
                {
                    var count = slot.ItemCount;
                    if (number > slot.ItemCount)
                    {
                        slot.ItemCount = 0;
                    }
                    else
                    {
                        slot.ItemCount -= number;
                    }

                    number -= count;
                    if (number <= 0)
                        break;
                }

                for (var i = 0; i < inventory.Count; i++)
                {
                    if (inventory[i].ItemCount == 0)
                        inventory.Remove(inventory[i]);
                }
            }
            // Game.IsInventoryUpdate = true;
        }

        private bool ItemFromInventoryExists(Item item, int count = 1)
        {
            return inventory.Where(p => (p.Item.Id == item.Id)).Sum(p => p.ItemCount) >= count;
        }

        private void GetInformationAboutWeapon()
        {
            return
        }
    }
}
