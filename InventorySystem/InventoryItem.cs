namespace InventorySystem;

public class InventoryItem
{
    public string Name { get; set; }
    public ItemType Type { get; set; }
    public int Quantity { get; private set; }
    public float Weight { get; set; }
    public int MaxStackSize { get; set; }
    
    public InventoryItem(string name, ItemType type, int quantity, float weight, int maxStackSize = 10)
    {
        Name = name;
        Type = type;
        Quantity = quantity;
        Weight = weight;
        MaxStackSize = maxStackSize;
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