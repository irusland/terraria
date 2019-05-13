using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
        public class Slot
        {
            public IInventoryItem Item;
            public int Amount;

            public Slot(IInventoryItem item, int amount)
            {
                Item = item;
                Amount = amount;
            }
        }

        private const int maxSize = 10;
        private readonly Slot[] inventory = new Slot[maxSize];
        private readonly Stack<int> freeIndexes = new Stack<int>();

        public Inventory()
        {
            for (var i = 0; i < 10; i++)
                freeIndexes.Push(i);
        }

        public Slot GetSelectedSlot => inventory[selected];
        public int selected
        {
            get { return selected; }
            set
            {
                if (value < maxSize && value >= 0)
                    selected = value;
                else
                    throw new IndexOutOfRangeException();
            }
        }


        public override string ToString()
        {
            return inventory.ToString();
        }

        public bool TryPush(IInventoryItem item, int count)
        {
            if (!TryGetItemIndex(item, out var index))
            {
                if (freeIndexes.Any())
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
            for (var i = 0; i < inventory.Length; i++)
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
    }
}
