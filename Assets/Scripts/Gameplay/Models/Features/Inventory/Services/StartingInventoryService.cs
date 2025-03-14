using System.Collections.Generic;
using Configs.Common;
using Configs.Features.Inventory;
using UnityEngine;

namespace Gameplay.Models.Features.Inventory.Services
{
    public class StartingInventoryService
    {
        private readonly StartingInventoryConfigSO _startingInventoryConfig;
        
        public StartingInventoryService(ConfigHolderSO configHolder)
        {
            _startingInventoryConfig = configHolder.StartingInventoryConfig;
        }
        
        public InventoryModel GenerateStartingInventory()
        {
            var startingInventory = new Dictionary<ItemId, int>();
            
            foreach (var itemConfig in _startingInventoryConfig.StartingItems)
            {
                if (Random.value <= itemConfig.ChanceToReceive)
                {
                    int amount = Random.Range(itemConfig.MinAmount, itemConfig.MaxAmount + 1);
                    if (amount > 0)
                    {
                        startingInventory[itemConfig.ItemId] = amount;
                    }
                }
            }
            
            return new InventoryModel(startingInventory);
        }
    }
}
