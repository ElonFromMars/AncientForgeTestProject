using System.Collections.Generic;
using AssetManagement.Configs;
using AssetManagement.Prefabs;
using AssetManagement.Sprites;
using Configs.Features.Inventory;
using Gameplay.Models.Features.Inventory;
using Gameplay.Models.Features.Inventory.Services;
using Gameplay.Views.UI.Features.Inventory;
using UnityEngine;

namespace Gameplay.Controllers.Features
{
    public class InventoryController
    {
        private readonly InventoryService _inventoryService;
        private readonly SpriteHolderSO _spriteHolder;
        private readonly UIViewPrefabHolderSO _prefabHolder;
        private readonly ItemConfigHolderSO _itemConfigHolder;

        private readonly Dictionary<ItemId, ItemView> _itemViews = new Dictionary<ItemId, ItemView>();
        private InventoryPanelView _inventoryPanelView;

        public InventoryController(
            InventoryService inventoryService,
            SpriteHolderSO spriteHolder,
            UIViewPrefabHolderSO prefabHolder,
            ItemConfigHolderSO itemConfigHolder)
        {
            _inventoryService = inventoryService;
            _spriteHolder = spriteHolder;
            _prefabHolder = prefabHolder;
            _itemConfigHolder = itemConfigHolder;
        }
        
        public void Initialize()
        {
            CreateInventoryPanelView();
            RefreshInventoryView();
            SubscribeToInventoryEvents();
        }

        public void Dispose()
        {
            UnsubscribeFromInventoryEvents();
        }

        public void CreateInventoryPanelView(Transform parent = null)
        {
            var prefab = _prefabHolder.GetPrefab(UIViewId.InventoryPanelView);
            
            var panelObject = Object.Instantiate(prefab, parent);
            var panelView = panelObject.GetComponent<InventoryPanelView>();
            
             _inventoryPanelView = panelView;
        }

        private void SubscribeToInventoryEvents()
        {
            _inventoryService.OnItemAdded += OnItemAdded;
            _inventoryService.OnItemRemoved += OnItemRemoved;
            _inventoryService.OnInventoryChanged += RefreshInventoryView;
        }
        
        private void UnsubscribeFromInventoryEvents()
        {
            _inventoryService.OnItemAdded -= OnItemAdded;
            _inventoryService.OnItemRemoved -= OnItemRemoved;
            _inventoryService.OnInventoryChanged -= RefreshInventoryView;
        }

        private void RefreshInventoryView()
        {
            if (_inventoryPanelView == null) return;
            
            ClearInventoryView();
            
            foreach ((ItemId, int) itemId in _inventoryService.GetItems())
            {
                var quantity = itemId.Item2;
                if (quantity > 0)
                {
                    CreateOrUpdateItemView(itemId.Item1, quantity);
                }
            }
            
            _inventoryPanelView.ArrangeItems();
        }
        
        private void OnItemAdded(ItemId itemId, int quantity)
        {
            if (_inventoryPanelView == null) return;
            
            CreateOrUpdateItemView(itemId, _inventoryService.GetItemQuantity(itemId));
            _inventoryPanelView.ArrangeItems();
        }
        
        private void OnItemRemoved(ItemId itemId, int quantity)
        {
            if (_inventoryPanelView == null) return;
            
            int remainingQuantity = _inventoryService.GetItemQuantity(itemId);
            
            if (remainingQuantity <= 0)
            {
                if (_itemViews.TryGetValue(itemId, out var itemView))
                {
                    Object.Destroy(itemView.gameObject);
                    _itemViews.Remove(itemId);
                }
            }
            else
            {
                CreateOrUpdateItemView(itemId, remainingQuantity);
            }
            
            _inventoryPanelView.ArrangeItems();
        }
        
        private void CreateOrUpdateItemView(ItemId itemId, int quantity)
        {
            if (_inventoryPanelView == null) return;
            
            if (_itemViews.TryGetValue(itemId, out var existingItemView))
            {
                existingItemView.UpdateQuantity(quantity);
                return;
            }
            
            var itemViewPrefab = _prefabHolder.GetPrefab(UIViewId.ItemSlot);
            if (itemViewPrefab == null)
            {
                return;
            }
            
            var itemViewObject = Object.Instantiate(itemViewPrefab, _inventoryPanelView.transform);
            var itemView = itemViewObject.GetComponent<ItemView>();
            
            var spriteId = GetSpriteIdForItem(itemId);
            var sprite = _spriteHolder.GetSprite(spriteId);
            
            itemView.Construct(itemId, sprite, quantity);
            _itemViews[itemId] = itemView;
        }
        
        private void ClearInventoryView()
        {
            foreach (var itemView in _itemViews.Values)
            {
                Object.Destroy(itemView.gameObject);
            }
            
            _itemViews.Clear();
        }
        
        private SpriteId GetSpriteIdForItem(ItemId itemId)
        {
            var itemConfig = _itemConfigHolder.GetItemConfig(itemId);
            return itemConfig.SpriteId;
        }
    }
}