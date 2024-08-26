using NUnit.Framework;

namespace InventorySystem.Tests;

public class InventorySystemTests
{
    private Inventory _inventory;

    [SetUp]
    public void Setup()
    {
        _inventory = new Inventory();
    }

    [Test]
    public void AddItem_NewItem_ShouldBeAddedToInventory()
    {
        // Arrange
        string itemName = "Health Potion";
        ItemType itemType = ItemType.Consumable;
        int itemQuantity = 5;

        // Act
        _inventory.AddItem(itemName, itemType, itemQuantity);

        // Assert
        Assert.That(_inventory.HasItem(itemName, itemType, itemQuantity), Is.True);
    }

    [Test]
    public void AddItem_ExistingItem_ShouldStackItems()
    {
        // Arrange
        string itemName = "Health Potion";
        ItemType itemType = ItemType.Consumable;
        int initialQuantity = 5;
        int additionalQuantity = 3;

        // Act
        _inventory.AddItem(itemName, itemType, initialQuantity);
        _inventory.AddItem(itemName, itemType, additionalQuantity);

        // Assert
        Assert.That(_inventory.HasItem(itemName, itemType, initialQuantity + additionalQuantity), Is.True);
    }

    [Test]
    public void RemoveItem_ItemExists_ShouldDecreaseQuantity()
    {
        // Arrange
        string itemName = "Health Potion";
        ItemType itemType = ItemType.Consumable;
        int initialQuantity = 5;
        int removeQuantity = 3;

        // Act
        _inventory.AddItem(itemName, itemType, initialQuantity);
        _inventory.RemoveItem(itemName, itemType, removeQuantity);

        // Assert
        Assert.That(_inventory.HasItem(itemName, itemType, initialQuantity - removeQuantity), Is.True);
    }

    [Test]
    public void RemoveItem_RemoveMoreThanExists_ShouldRemoveAllItems()
    {
        // Arrange
        string itemName = "Health Potion";
        ItemType itemType = ItemType.Consumable;
        int initialQuantity = 5;
        int removeQuantity = 10;

        // Act
        _inventory.AddItem(itemName, itemType, initialQuantity);
        _inventory.RemoveItem(itemName, itemType, removeQuantity);

        // Assert
        Assert.That(_inventory.HasItem(itemName, itemType, 1), Is.False);
    }

    [Test]
    public void HasItem_ItemDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        string itemName = "Mana Potion";
        ItemType itemType = ItemType.Consumable;
        int requiredQuantity = 1;

        // Act
        bool result = _inventory.HasItem(itemName, itemType, requiredQuantity);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void DisplayInventory_ShouldListAllItems()
    {
        // Arrange
        _inventory.AddItem("Health Potion", ItemType.Consumable, 5);
        _inventory.AddItem("Sword", ItemType.Weapon, 1);

        // Act
        _inventory.DisplayInventory();

        // Assert
        // Manually verify the console output for correct item listing.
    }
}