namespace InventorySystem;

public class Inventory
{
    private List<InventoryItem> Items { get; set; }
    
    public Inventory()
    {
        Items = new List<InventoryItem>();
    }

    public void AddItem(string name, ItemType type, int quantity)
    {
        var item = Items.Find(i => i.Name == name && i.Type == type);
        
        if (item != null)
        {
            item.AddQuantity(quantity);
        }
        else
        {
            Items.Add(new InventoryItem(name, type, quantity));
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
}