using System.Collections.Generic;
using System.Linq;
using Gameplay.Models.Features.Inventory;
using UnityEngine;

namespace Configs.Features.Inventory
{
    [CreateAssetMenu(fileName = nameof(ItemConfigHolderSO), menuName = "ScriptableObjects/Configs/" + nameof(ItemConfigHolderSO), order = 0)]
    public class ItemConfigHolderSO : ScriptableObject
    {
        [SerializeField] private List<ItemConfigData> items = new List<ItemConfigData>();

        public List<ItemConfigData> Items => items;

        public ItemConfigData GetItemConfig(ItemId itemId)
        {
            return items.FirstOrDefault(item => item.ItemId == itemId);
        }

        public bool TryGetItemConfig(ItemId itemId, out ItemConfigData itemConfig)
        {
            itemConfig = GetItemConfig(itemId);
            return itemConfig != null;
        }
    }
}
