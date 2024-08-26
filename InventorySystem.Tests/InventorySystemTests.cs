using NUnit.Framework;
using System.Collections.Generic;
using System.IO;

namespace InventorySystem.Tests
{
    [TestFixture]
    public class InventorySystemTests
    {
        private Inventory _inventory;
        private const string TestFilePath = "test_inventory.xml";

        [SetUp]
        public void Setup()
        {
            _inventory = new Inventory();
        }

        [TearDown]
        public void Cleanup()
        {
            if (File.Exists(TestFilePath))
            {
                File.Delete(TestFilePath);
            }
        }

        [Test]
        public void AddItem_NewItem_ShouldBeAddedToInventory()
        {
            // Arrange
            string itemName = "Health Potion";
            ItemType itemType = ItemType.Consumable;
            int itemQuantity = 5;
            float itemWeight = 0.5f;
            int itemDurability = 10;

            // Act
            _inventory.AddItem(itemName, itemType, itemQuantity, itemWeight, itemDurability);

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
            float itemWeight = 0.5f;
            int itemDurability = 10;

            // Act
            _inventory.AddItem(itemName, itemType, initialQuantity, itemWeight, itemDurability);
            _inventory.AddItem(itemName, itemType, additionalQuantity, itemWeight, itemDurability);

            // Assert
            Assert.That(_inventory.HasItem(itemName, itemType, initialQuantity + additionalQuantity), Is.True);
        }

        [Test]
        public void AddItem_ExceedsMaxWeight_ShouldNotAddItem()
        {
            // Arrange
            _inventory = new Inventory(1); // Set a low max weight capacity
            string itemName = "Heavy Sword";
            ItemType itemType = ItemType.Weapon;
            int itemQuantity = 1;
            float itemWeight = 2.0f; // Exceeds max weight
            int itemDurability = 100;

            // Act
            _inventory.AddItem(itemName, itemType, itemQuantity, itemWeight, itemDurability);

            // Assert
            Assert.That(_inventory.HasItem(itemName, itemType, itemQuantity), Is.False);
        }

        [Test]
        public void RemoveItem_ShouldDecreaseQuantity()
        {
            // Arrange
            string itemName = "Health Potion";
            ItemType itemType = ItemType.Consumable;
            int initialQuantity = 5;
            int removeQuantity = 3;
            float itemWeight = 0.5f;
            int itemDurability = 10;

            // Act
            _inventory.AddItem(itemName, itemType, initialQuantity, itemWeight, itemDurability);
            _inventory.RemoveItem(itemName, itemType, removeQuantity);

            // Assert
            Assert.That(_inventory.HasItem(itemName, itemType, initialQuantity - removeQuantity), Is.True);
        }

        [Test]
        public void RemoveItem_RemoveAll_ShouldRemoveItemFromInventory()
        {
            // Arrange
            string itemName = "Health Potion";
            ItemType itemType = ItemType.Consumable;
            int initialQuantity = 5;
            float itemWeight = 0.5f;
            int itemDurability = 10;

            // Act
            _inventory.AddItem(itemName, itemType, initialQuantity, itemWeight, itemDurability);
            _inventory.RemoveItem(itemName, itemType, initialQuantity);

            // Assert
            Assert.That(_inventory.HasItem(itemName, itemType, 1), Is.False);
        }

        [Test]
        public void EquipItem_ShouldMoveItemToEquippedItems()
        {
            // Arrange
            string itemName = "Sword";
            ItemType itemType = ItemType.Weapon;
            int itemQuantity = 1;
            float itemWeight = 2.0f;
            int itemDurability = 100;

            // Act
            _inventory.AddItem(itemName, itemType, itemQuantity, itemWeight, itemDurability);
            _inventory.EquipItem(itemName);

            // Assert
            Assert.That(_inventory.HasItem(itemName, itemType, 1), Is.False);
            Assert.That(_inventory.EquippedItems.ContainsKey(itemName), Is.True);
        }

        [Test]
        public void EquipItem_ItemNotEquippable_ShouldNotEquipItem()
        {
            // Arrange
            string itemName = "Health Potion";
            ItemType itemType = ItemType.Consumable;
            int itemQuantity = 1;
            float itemWeight = 0.5f;
            int itemDurability = 10;

            // Act
            _inventory.AddItem(itemName, itemType, itemQuantity, itemWeight, itemDurability);
            _inventory.EquipItem(itemName);

            // Assert
            Assert.That(_inventory.HasItem(itemName, itemType, 1), Is.True);
            Assert.That(_inventory.EquippedItems.ContainsKey(itemName), Is.False);
        }

        [Test]
        public void UnequipItem_ShouldMoveItemBackToInventory()
        {
            // Arrange
            string itemName = "Sword";
            ItemType itemType = ItemType.Weapon;
            int itemQuantity = 1;
            float itemWeight = 2.0f;
            int itemDurability = 100;

            // Act
            _inventory.AddItem(itemName, itemType, itemQuantity, itemWeight, itemDurability);
            _inventory.EquipItem(itemName);
            _inventory.UnequipItem(itemName);

            // Assert
            Assert.That(_inventory.HasItem(itemName, itemType, 1), Is.True);
            Assert.That(_inventory.EquippedItems.ContainsKey(itemName), Is.False);
        }

        [Test]
        public void CraftItem_ValidRecipe_ShouldCreateNewItem()
        {
            // Arrange
            var craftedItem = new InventoryItem("Iron Sword", ItemType.Weapon, 1, 5.0f, 100);
            var requiredItems = new List<InventoryItem>
            {
                new InventoryItem("Iron Ingot", ItemType.Material, 2, 3.0f, 100),
                new InventoryItem("Wood", ItemType.Material, 1, 2.0f, 100)
            };
            _inventory.CraftingRecipes.Add(craftedItem, requiredItems);

            // Add required items to inventory
            _inventory.AddItem("Iron Ingot", ItemType.Material, 2, 3.0f, 100);
            _inventory.AddItem("Wood", ItemType.Material, 1, 2.0f, 100);

            // Act
            _inventory.CraftItem("Iron Sword");

            // Assert
            Assert.That(_inventory.HasItem("Iron Sword", ItemType.Weapon, 1), Is.True);
        }

        [Test]
        public void CraftItem_MissingIngredients_ShouldNotCreateNewItem()
        {
            // Arrange
            var craftedItem = new InventoryItem("Iron Sword", ItemType.Weapon, 1, 5.0f, 100);
            var requiredItems = new List<InventoryItem>
            {
                new InventoryItem("Iron Ingot", ItemType.Material, 2, 3.0f, 100),
                new InventoryItem("Wood", ItemType.Material, 1, 2.0f, 100)
            };
            _inventory.CraftingRecipes.Add(craftedItem, requiredItems);

            // Add only one of the required items
            _inventory.AddItem("Iron Ingot", ItemType.Material, 1, 3.0f, 100);

            // Act
            _inventory.CraftItem("Iron Sword");

            // Assert
            Assert.That(_inventory.HasItem("Iron Sword", ItemType.Weapon, 1), Is.False);
        }

        [Test]
        public void SaveAndLoadInventory_ShouldPersistInventoryItems()
        {
            // Arrange
            string itemName = "Health Potion";
            ItemType itemType = ItemType.Consumable;
            int itemQuantity = 5;
            float itemWeight = 0.5f;
            int itemDurability = 10;

            // Act
            _inventory.AddItem(itemName, itemType, itemQuantity, itemWeight, itemDurability);
            _inventory.SaveInventory(TestFilePath);

            var loadedInventory = new Inventory();
            loadedInventory.LoadInventory(TestFilePath);

            // Assert
            Assert.That(loadedInventory.HasItem(itemName, itemType, itemQuantity), Is.True);
        }

        [Test]
        public void SortInventoryByName_ShouldSortItemsAlphabetically()
        {
            // Arrange
            _inventory.AddItem("Cherry", ItemType.Consumable, 2, 0.5f, 100);
            _inventory.AddItem("Apple", ItemType.Consumable, 3, 0.5f, 100);
            _inventory.AddItem("Banana", ItemType.Consumable, 5, 0.5f, 100);

            // Act
            _inventory.SortInventoryByName();
            var sortedItems = _inventory.GetAllItems(); // Empty name returns all items

            // Assert
            Assert.That(sortedItems[0].Name, Is.EqualTo("Apple"));
            Assert.That(sortedItems[1].Name, Is.EqualTo("Banana"));
            Assert.That(sortedItems[2].Name, Is.EqualTo("Cherry"));
        }

        [Test]
        public void SortInventoryByQuantity_ShouldSortItemsByQuantity()
        {
            // Arrange
            _inventory.AddItem("Apple", ItemType.Consumable, 3, 0.5f, 100);
            _inventory.AddItem("Banana", ItemType.Consumable, 5, 0.5f, 100);
            _inventory.AddItem("Cherry", ItemType.Consumable, 2, 0.5f, 100);

            // Act
            _inventory.SortInventoryByQuantity();
            var sortedItems = _inventory.GetAllItems(); // Empty name returns all items

            // Assert
            Assert.That(sortedItems[0].Quantity, Is.EqualTo(2));
            Assert.That(sortedItems[1].Quantity, Is.EqualTo(3));
            Assert.That(sortedItems[2].Quantity, Is.EqualTo(5));
        }

        [Test]
        public void SortInventoryByType_ShouldSortItemsByType()
        {
            // Arrange
            _inventory.AddItem("Apple", ItemType.Consumable, 3, 0.5f, 100);
            _inventory.AddItem("Sword", ItemType.Weapon, 1, 2.0f, 100);
            _inventory.AddItem("Armor", ItemType.Armor, 1, 10.0f, 100);

            // Act
            _inventory.SortInventoryByType();
            var sortedItems = _inventory.GetAllItems(); // Empty name returns all items

            // Assert
            Assert.That(sortedItems[0].Type, Is.EqualTo(ItemType.Consumable));
            Assert.That(sortedItems[1].Type, Is.EqualTo(ItemType.Weapon));
            Assert.That(sortedItems[2].Type, Is.EqualTo(ItemType.Armor));
        }
    }
}
