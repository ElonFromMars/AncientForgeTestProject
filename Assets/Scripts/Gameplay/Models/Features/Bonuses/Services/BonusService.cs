using System.Collections.Generic;
using Configs.Common;
using Configs.Features.Bonuses;
using Gameplay.Models.Features.Crafting.Services;
using Gameplay.Models.Features.Inventory;
using Gameplay.Models.Features.Inventory.Services;
using Gameplay.Models.Features.Machines.Services;

namespace Gameplay.Models.Features.Bonuses.Services
{
    public class BonusService
    {
        private readonly BonusConfigHolderSO _bonusConfigHolder;
        private readonly InventoryService _inventoryService;
        private readonly MachineService _machineService;
        
        private Dictionary<ItemId, BonusModel> _bonuses;

        public BonusService(ConfigHolderSO configHolder, InventoryService inventoryService, MachineService machineService)
        {
            _bonusConfigHolder = configHolder.BonusConfigHolder;
            _inventoryService = inventoryService;
            _machineService = machineService;
        }

        public void Initialize()
        {
            _bonuses = InitializeBonuses();
            
            _inventoryService.OnItemAdded += HandleItemAdded;
            _inventoryService.OnItemRemoved += HandleItemRemoved;
            
            UpdateAllBonusStates();
        }

        public void Dispose()
        {
            if (_inventoryService != null)
            {
                _inventoryService.OnItemAdded -= HandleItemAdded;
                _inventoryService.OnItemRemoved -= HandleItemRemoved;
            }
        }

        private Dictionary<ItemId, BonusModel> InitializeBonuses()
        {
            var bonuses = new Dictionary<ItemId, BonusModel>();
            var bonusFactory = new BonusFactory(_machineService);
            foreach (var bonusConfig in _bonusConfigHolder.Bonuses)
            {
                var bonus = bonusFactory.CreateBonus(bonusConfig);
                bonuses[bonusConfig.ItemId] = bonus;
            }
            
            return bonuses;
        }

        private void HandleItemAdded(ItemId itemId, int quantity)
        {
            UpdateBonusStateForItem(itemId);
        }
        
        private void HandleItemRemoved(ItemId itemId, int quantity)
        {
            UpdateBonusStateForItem(itemId);
        }
        
        private void UpdateBonusStateForItem(ItemId itemId)
        {
            var bonusConfig = _bonusConfigHolder.GetBonusConfigByItemId(itemId);
            if (bonusConfig != null && _bonuses.TryGetValue(bonusConfig.ItemId, out var bonus))
            {
                bool hasItem = _inventoryService.HasItem(itemId);
                bonus.SetActive(hasItem);
            }
        }
        
        private void UpdateAllBonusStates()
        {
            foreach (var bonus in _bonuses.Values)
            {
                bool hasItem = _inventoryService.HasItem(bonus.ItemId);
                bonus.SetActive(hasItem);
            }
        }
    }
}
