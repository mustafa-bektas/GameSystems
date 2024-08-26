namespace InventorySystem;

public class Inventory
{
    private List<InventoryItem> Items { get; set; }
    private int _maxWeightCapacity = 100;
    
    public Inventory()
    {
        Items = new List<InventoryItem>();
    }

    public void AddItem(string name, ItemType type, int quantity, float weight)
    {
        var item = Items.Find(i => i.Name == name && i.Type == type);
        
        if (item != null && item.Quantity + quantity <= item.MaxStackSize)
        {
            item.AddQuantity(quantity);
        }
        else
        {
            Items.Add(new InventoryItem(name, type, quantity, weight));
        }
    }
    
    public void RemoveItem(string name, ItemType type, int quantity)
    {
        var item = Items.Find(i => i.Name == name && i.Type == type);
        
        if (item != null)
        {
            item.RemoveQuantity(quantity);
            
            if (item.Quantity == 0)
            {
                Items.Remove(item);
            }
        }
    }
    
    public bool HasItem(string name, ItemType type, int quantity)
    {
        var item = Items.Find(i => i.Name == name && i.Type == type);
        
        if (item != null)
        {
            return item.Quantity >= quantity;
        }
        
        return false;
    }
    
    public void DisplayInventory()
    {
        foreach (var item in Items)
        {
            Console.WriteLine($"{item.Name} ({item.Type}): {item.Quantity}");
        }
    }
    
    public List<InventoryItem> GetItemsByType(ItemType type)
    {
        return Items.FindAll(i => i.Type == type);
    }
    
    public List<InventoryItem> GetItemsByName(string name)
    {
        return Items.FindAll(i => i.Name == name);
    }

    private float GetCurrentWeight()
    {
        return Items.Sum(i => i.Weight * i.Quantity);
    }

    public bool CanAddItem(InventoryItem item, int quantity)
    {
        return GetCurrentWeight() + item.Weight * quantity <= _maxWeightCapacity;
    }
}