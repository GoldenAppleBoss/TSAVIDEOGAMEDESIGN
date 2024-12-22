using System.Collections.Generic;
namespace GlobalClasses
{
    public class Inventory
    {
        public List<InventorySlot> slots = new List<InventorySlot>();

        public void AddItem(int Id, Item item, int Count)
        {
            InventorySlot slot = new InventorySlot(Id, item, Count);
            slots.Add(slot);
        }

        public bool RemoveItem(Item item)
        {
            InventorySlot slot = slots.Find(s => s.item.Name == item.Name);
            if (slot != null)
            {
                slots.Remove(slot);
                return true;
            }
            return false;
        }
    }

    public class InventorySlot
    {
        public int Id;
        public Item item;
        public int Count;

        public InventorySlot(int Id, Item item, int Count)
        {
            this.Id = Id;
            this.item = item;
            this.Count = Count;
        }
    }
}