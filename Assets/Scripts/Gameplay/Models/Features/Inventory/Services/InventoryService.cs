using System;
using System.Collections.Generic;
using Gameplay.Models.Features.Crafting;
using Configs.Common;

namespace Gameplay.Models.Features.Inventory.Services
{
    public class InventoryService
    {
        private readonly StartingInventoryService _startingInventoryService;
        private InventoryModel _inventoryModel;
        
        public InventoryService(ConfigHolderSO configHolder)
        {
            _startingInventoryService = new StartingInventoryService(configHolder);
        }
        
        public void InitializeStartingInventory()
        {
            _inventoryModel = _startingInventoryService.GenerateStartingInventory();
        }
        
        public event Action<ItemId, int> OnItemAdded
        {
            add => _inventoryModel.OnItemAdded += value;
            remove => _inventoryModel.OnItemAdded -= value;
        }
        
        public event Action<ItemId, int> OnItemRemoved
        {
            add => _inventoryModel.OnItemRemoved += value;
            remove => _inventoryModel.OnItemRemoved -= value;
        }
        
        public event Action OnInventoryChanged
        {
            add => _inventoryModel.OnInventoryChanged += value;
            remove => _inventoryModel.OnInventoryChanged -= value;
        }
        
        // Delegate methods to the model
        public int GetItemQuantity(ItemId itemId) => _inventoryModel.GetItemQuantity(itemId);
        
        public bool HasItem(ItemId itemId, int quantity = 1) => _inventoryModel.HasItem(itemId, quantity);
        
        public bool HasItems(IEnumerable<RecipeIngredient> ingredients) => _inventoryModel.HasItems(ingredients);
        
        public void AddItem(ItemId itemId, int quantity = 1) => _inventoryModel.AddItem(itemId, quantity);
        
        public bool RemoveItem(ItemId itemId, int quantity = 1) => _inventoryModel.RemoveItem(itemId, quantity);
        
        public bool RemoveItems(IEnumerable<RecipeIngredient> ingredients) => _inventoryModel.RemoveItems(ingredients);

        public IEnumerable<(ItemId, int)> GetItems() => _inventoryModel.GetItems();
    }
}
