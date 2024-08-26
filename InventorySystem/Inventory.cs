using System.Xml.Serialization;

namespace InventorySystem;

public class Inventory
{
    private List<InventoryItem> Items { get; set; }
    private readonly int _maxWeightCapacity = 100;
    public Dictionary<string, InventoryItem> EquippedItems { get; set; }
    public Dictionary<InventoryItem, List<InventoryItem>> CraftingRecipes { get; set; }
    
    public Inventory()
    {
        Items = new List<InventoryItem>();
        EquippedItems = new Dictionary<string, InventoryItem>();
        CraftingRecipes = new Dictionary<InventoryItem, List<InventoryItem>>();
    }
    
    public Inventory(int maxWeightCapacity)
    {
        Items = new List<InventoryItem>();
        EquippedItems = new Dictionary<string, InventoryItem>();
        _maxWeightCapacity = maxWeightCapacity;
        CraftingRecipes = new Dictionary<InventoryItem, List<InventoryItem>>();
    }
    
    public void EquipItem(string name)
    {
        var item = Items.Find(i => i.Name == name && (i.Type == ItemType.Weapon || i.Type == ItemType.Armor));
    
        if (item != null)
        {
            EquippedItems[name] = item;
            Items.Remove(item);
        }
        else
        {
            Console.WriteLine("Item cannot be equipped or does not exist.");
        }
    }
    
    public void UnequipItem(string name)
    {
        if (EquippedItems.ContainsKey(name))
        {
            Items.Add(EquippedItems[name]);
            EquippedItems.Remove(name);
        }
    }

    public void AddItem(string name, ItemType type, int quantity, float weight, int durability)
    {
        var item = Items.Find(i => i.Name == name && i.Type == type);

        if (item != null && item.Quantity + quantity <= item.MaxStackSize)
        {
            if (CanAddItem(item, quantity))
            {
                item.AddQuantity(quantity);
            }
            else
            {
                Console.WriteLine("Cannot add item. Exceeds weight capacity.");
            }
        }
        else
        {
            var newItem = new InventoryItem(name, type, quantity, weight, durability);
            if (CanAddItem(newItem, quantity))
            {
                Items.Add(newItem);
            }
            else
            {
                Console.WriteLine("Cannot add item. Exceeds weight capacity.");
            }
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

    public List<InventoryItem> GetAllItems()
    {
        return Items;
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

    public void SortInventoryByName()
    {
        Items.Sort((a, b) => String.Compare(a.Name, b.Name, StringComparison.Ordinal));
    }
    
    public void SortInventoryByQuantity()
    {
        Items.Sort((a, b) => a.Quantity.CompareTo(b.Quantity));
    }
    
    public void SortInventoryByType()
    {
        Items.Sort((a, b) => a.Type.CompareTo(b.Type));
    }

    public void CraftItem(string resultItemName)
    {
        var key = CraftingRecipes.Keys.FirstOrDefault(item => item.Name == resultItemName);

        if (key != null)
        {
            var itemsNeeded = CraftingRecipes[key];
        
            foreach (var item in itemsNeeded)
            {
                if (!HasItem(item.Name, item.Type, item.Quantity))
                {
                    Console.WriteLine("Missing required items for crafting.");
                    return;
                }
            }

            foreach (var item in itemsNeeded)
            {
                RemoveItem(item.Name, item.Type, item.Quantity);
            }
        
            if (CanAddItem(key, 1))
            {
                AddItem(key.Name, key.Type, 1, key.Weight, key.Durability);
                Console.WriteLine($"{resultItemName} crafted successfully!");
            }
            else
            {
                Console.WriteLine("Cannot craft item. Exceeds weight capacity.");
            }
        }
        else
        {
            Console.WriteLine("Crafting recipe not found.");
        }
    }

    public void SaveInventory(string path)
    {
        using var writer = new StreamWriter(path);
        var serializer = new XmlSerializer(typeof(List<InventoryItem>));
        serializer.Serialize(writer, Items);
    }
    
    public void LoadInventory(string path)
    {
        using var reader = new StreamReader(path);
        var serializer = new XmlSerializer(typeof(List<InventoryItem>));
        Items = (List<InventoryItem>)serializer.Deserialize(reader)!;
    }
}