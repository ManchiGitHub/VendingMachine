namespace VendingMachine
{
    using System.Collections.Generic;

    public class Inventory<T>
    {
        private Dictionary<T, int> inventory = new Dictionary<T, int>();

        public int getQuantity(T i_item)
        {
            inventory.TryGetValue(i_item, out int quantity);

            return quantity;
        }

        public void AddItem(T i_item)
        {
            if (inventory.TryGetValue(i_item, out int quantity))
            {
                inventory[i_item]++;
            }
            else
            {
                inventory.Add(i_item, 1);
            }
        }

        public void RemoveItem(T i_item)
        {
            inventory[i_item] -= 1;
        }

        public void ClearInventory()
        {
            inventory.Clear();
        }
    }
}
