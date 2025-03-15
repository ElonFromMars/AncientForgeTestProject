using System.Collections.Generic;
using Configs.Common;
using Configs.Features.Inventory;
using Gameplay.Models.Common.Services;

namespace Gameplay.Models.Features.Inventory.Services
{
    public class StartingInventoryService
    {
        private readonly StartingInventoryConfigSO _startingInventoryConfig;
        private RandomService _randomService = new RandomService();
        
        public StartingInventoryService(ConfigHolderSO configHolder)
        {
            _startingInventoryConfig = configHolder.StartingInventoryConfig;
        }
        
        public InventoryModel GenerateStartingInventory()
        {
            var startingInventory = new Dictionary<ItemId, int>();
            
            foreach (var itemConfig in _startingInventoryConfig.StartingItems)
            {
                if (_randomService.GetRandomFloat(0f, 100f) <= itemConfig.chanceToReceive)
                {
                    int amount = _randomService.GetRandomInt(itemConfig.minAmount, itemConfig.maxAmount + 1);
                    if (amount > 0)
                    {
                        startingInventory[itemConfig.itemId] = amount;
                    }
                }
            }
            
            return new InventoryModel(startingInventory);
        }
    }
}
