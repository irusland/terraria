using System.Collections.Generic;
using System.Linq;

namespace terraria
{
    public interface IInventoryItem
    {
        // Объекты на карте и в интвентаре будут одини и теми же
        // Например камень он будет ICharacter и IInventoryItem
        // Будет реализовать 2 интерфеса, для меня (Карты) и для тебя (Инвентаря)
        // Накидай себе нужных позиций в интерфейс, которыми будешь пользоваться
        // 
        // От тебя мне нужно чтобы в инвентарь состоял из объектов реализующих IInventoryItem
        // (В том числе и тех, которые есть на карте, т.е. : ICharacter)
        // Давай сменим логику и будем добавлять блоки и предметы как я сделал (загляни в Characters.cs)
        // то есть у нас 
        // НЕ будет енумов и всяких айдишников 
        //  А будут классы предметов реализующих нужный тебе интерфейс, а которые еще и на карте могут появится
        // реализуют мой интерфейс
    }

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
        private Item SlotInArms = new Item(TypeItem.Nothing);

        public bool CheckSlotInArms(Item SlotInArms)
        {
            return SlotInArms.type != TypeItem.Nothing;
        }

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

        public Item SelectItem(Inventory inventory, Item item)
        {
            if (ItemFromInventoryExists(item))
                return item;
            return new Item(TypeItem.Nothing);

        }

        public bool ItemFromInventoryExists(Item item, int count = 1)
        {
            return inventory.Where(p => (p.Item.Id == item.Id))
                .Sum(p => p.ItemCount) >= count;
        }

        public TypeItem GetInformationAboutWeapon()
        {
            if (CheckSlotInArms(SlotInArms))
                return SlotInArms.type;
            return TypeItem.Nothing;
        }
    }
}
