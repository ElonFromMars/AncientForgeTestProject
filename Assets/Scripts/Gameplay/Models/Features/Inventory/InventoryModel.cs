using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Models.Features.Crafting;

namespace Gameplay.Models.Features.Inventory
{
    public class InventoryModel
    {
        private Dictionary<ItemId, int> _itemCountById;
        
        public event Action<ItemId, int> OnItemAdded;
        public event Action<ItemId, int> OnItemRemoved;
        public event Action OnInventoryChanged;

        public InventoryModel()
        {
            _itemCountById = new Dictionary<ItemId, int>();
        }

        public InventoryModel(Dictionary<ItemId, int> initialInventory)
        {
            _itemCountById = new Dictionary<ItemId, int>(initialInventory);
        }
        
        public int GetItemQuantity(ItemId itemId)
        {
            return _itemCountById.TryGetValue(itemId, out int quantity) ? quantity : 0;
        }

        public bool HasItem(ItemId itemId, int quantity = 1)
        {
            return GetItemQuantity(itemId) >= quantity;
        }

        public bool HasItems(IEnumerable<RecipeIngredient> ingredients)
        {
            foreach (var ingredient in ingredients)
            {
                if (!HasItem(ingredient.ItemId, ingredient.Quantity))
                {
                    return false;
                }
            }
            return true;
        }

        public void AddItem(ItemId itemId, int quantity = 1)
        {
            if (!_itemCountById.ContainsKey(itemId))
            {
                _itemCountById[itemId] = 0;
            }
            _itemCountById[itemId] += quantity;
            
            OnItemAdded?.Invoke(itemId, quantity);
            OnInventoryChanged?.Invoke();
        }

        public bool RemoveItem(ItemId itemId, int quantity = 1)
        {
            if (!HasItem(itemId, quantity))
            {
                return false;
            }

            _itemCountById[itemId] -= quantity;
            if (_itemCountById[itemId] <= 0)
            {
                _itemCountById.Remove(itemId);
            }
            
            OnItemRemoved?.Invoke(itemId, quantity);
            OnInventoryChanged?.Invoke();
            return true;
        }

        public bool RemoveItems(IEnumerable<RecipeIngredient> ingredients)
        {
            if (!HasItems(ingredients))
            {
                return false;
            }

            foreach (var ingredient in ingredients)
            {
                RemoveItem(ingredient.ItemId, ingredient.Quantity);
            }
            return true;
        }

        public IEnumerable<(ItemId, int)> GetItems()
        {
            return _itemCountById.Select(kvp => (kvp.Key, kvp.Value));
        }
    }
}