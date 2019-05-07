using System;

namespace terraria
{
    interface ISlot // На всякий пожарный!
    {
        IItem Item { get; set; }
        void Put<T>(T item) where T : IItem;
    }

    class SlotArmor<I> : ISlot where I : Armor
    {
        public IItem Item { get; set; }
        public void Put<T>(T item) where T : IItem
        {
            Item = item as I;
            Console.WriteLine(Item != null ? Item.ToString() : "null");
        }
    }

    class SlotWeapon<I> : ISlot where I : Weapon
    {
        public IItem Item { get; set; }
        public void Put<T>(T item) where T : IItem
        {
            Item = item as I;
            Console.WriteLine(Item != null ? Item.ToString() : "null");
        }
    }

    interface IItem { }
    class Armor : IItem { }
    class Weapon : IItem { }
}
