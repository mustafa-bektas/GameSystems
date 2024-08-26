namespace InventorySystem;

public class InventoryItem
{
    public string Name { get; set; }
    public ItemType Type { get; set; }
    public int Quantity { get; set; }
    public float Weight { get; set; }
    public int MaxStackSize { get; set; }
    public int Durability { get; set; }
    public int MaxDurability { get; set; }
    public bool IsUsable { get; set; }
    
    public InventoryItem(
        string name,
        ItemType type,
        int quantity,
        float weight,
        int maxDurability,
        int maxStackSize = 10)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Type = type;
        Quantity = Math.Max(0, quantity); 
        Weight = weight;
        MaxStackSize = Math.Max(1, maxStackSize);
        Durability = Math.Max(0, maxDurability);
        MaxDurability = maxDurability;
        IsUsable = true;
    }

    public InventoryItem()
    {
        
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
    
    public void DecreaseDurability(int amount)
    {
        Durability = Math.Max(0, Durability - amount);
        if (Durability == 0)
        {
            BreakItem();
        }
    }


    public void RepairItem()
    {
        Durability = MaxDurability;
    }

    private void BreakItem()
    {
        IsUsable = false;
    }
    
}