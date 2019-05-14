using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace terraria
{
    public interface IInventoryItem
    {
        string GetIconFileName();
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
        public class Slot
        {
            public IInventoryItem Item;
            public int Amount;

            public Slot(IInventoryItem item, int amount)
            {
                Item = item;
                Amount = amount;
            }

            public override string ToString()
            {
                return $"{Item} {Amount}";
            }
        }

        public const int maxSize = 10;
        public readonly Slot[] inventory = new Slot[maxSize];
        private readonly Stack<int> freeIndexes = new Stack<int>();

        public Inventory()
        {
            for (var i = 9; i >= 0; i--)
                freeIndexes.Push(i);
        }

        public Slot GetSelectedSlot => inventory[selected];
        private int selector = 0;
        public int selected
        {
            get { return selector; }
            set
            {
                if (value < 0)
                    value += maxSize;
                selector = value % (maxSize - freeIndexes.Count);
                if (selector >= maxSize || selector < 0)
                    throw new IndexOutOfRangeException();
            }
        }

        public Slot this[int i]
        {
            get { return inventory[i]; }
            set { inventory[i] = value; }
        }

        public override string ToString()
        {
            var str = "";
            foreach (var line in inventory)
            {
                str += line;
            }
            return str;
        }

        public bool TryPush(IInventoryItem item, int count)
        {
            if (!TryGetItemIndex(item, out var index))
            {
                if (!freeIndexes.Any())
                    return false;
                index = freeIndexes.Pop();
                inventory[index] = new Slot(item, count);
                return true;
            }
            inventory[index].Amount += count;
            return true;
        }

        public bool TryGetItemIndex(IInventoryItem item, out int index)
        {
            for (var i = 0; i < maxSize; i++)
            {
                var element = inventory[i];
                if (element != null)
                {
                    var type = element.Item.GetType().Name;
                    if (item.GetType().Name == type)
                    {
                        index = i;
                        return true;
                    }
                }
            }
            index = -1;
            return false;
        }

        public bool TryPopItem(IInventoryItem item, out IInventoryItem popItem, out int itemAmount)
        {
            if (TryGetItemIndex(item, out var index))
            {
                popItem = inventory[index].Item;
                itemAmount = inventory[index].Amount;
                return true;
            }
            itemAmount = 0;
            popItem = null;
            return false;
        }

        public bool TryPopSelectedItem(int amount, out IInventoryItem item)
        {
            if (amount == 0)
            {
                item = null;
                return false;
            }
            var slot = GetSelectedSlot;
            if (slot.Amount < amount)
            {
                item = null;
                return false;
            }
            slot.Amount -= amount;
            item = slot.Item;

            if (slot.Amount == 0)
            {
                inventory[selected] = null;
                freeIndexes.Push(selected);
            }
            return true;
        }
    }
}
