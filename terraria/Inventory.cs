using System.Collections.Generic;
using System.Linq;

namespace terraria
{
    public class Inventory
    {
        public Inventory() => inventory = new List<InventorySlot>();

        public enum TypeItem
        {
            Axe,
            Pick,
            Shovel,
            Shield,
            Wood,
            Rock,
            Dirt,
            HealingSlave,
            Nothing,
            SpeedSlave
        }

        private List<InventorySlot> inventory;
        private readonly int maxItemCount = 25;
        private InventorySlot SlotInArms = new InventorySlot();
        private readonly bool IsSlotInArms;

        public class InventorySlot
        {
            public TypeItem typeItem;
            public int ItemCount { get; set; }
            public Item Item { get; set; }
            public int SlotId { get; set; }
        }

        public class Item
        {
            public TypeItem type;
            public int Id { get; set; }
            public string Name { get; set; }
            public string LocalizedName { get; set; }
            // public Image Texture { get => Texture; set => Texture = value; }
            public Dictionary<string, double> ItemProperties { get; set; } = new Dictionary<string, double>();

            public Item(TypeItem type)
            {
                this.type = type;
                //LoadItem();
            }

            //public void LoadItem()
            //{
            //    this.localizedName = this.name;
            //}
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
            var slots = inventory.Where(p => (p.Item.Id == item.Id))
                .OrderBy(p => p.ItemCount).Select(p => p);

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

        public bool ItemFromInventoryExists(Item item, int count = 1)
        {
            return inventory.Where(p => (p.Item.Id == item.Id))
                .Sum(p => p.ItemCount) >= count;
        }

        public TypeItem GetInformationAboutWeapon()
        {
            if (IsSlotInArms)
                return SlotInArms.typeItem;
            return TypeItem.Nothing;
        }
    }
}
