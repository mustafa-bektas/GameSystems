namespace InventorySystem;

public class InventoryItem
{
    public string Name { get; set; }
    public ItemType Type { get; set; }
    public int Quantity { get; private set; }
    
    public InventoryItem(string name, ItemType type, int quantity)
    {
        Name = name;
        Type = type;
        Quantity = quantity;
    }
    
    public void AddQuantity(int amount)
    {
        Quantity += amount;
    }
    
    public void RemoveQuantity(int amount)
    {
        Quantity -= amount;
        
        if (Quantity < 0)
        {
            Quantity = 0;
        }
    }
}