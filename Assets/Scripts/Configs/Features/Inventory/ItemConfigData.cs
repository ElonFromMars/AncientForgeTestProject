using AssetManagement.Sprites;
using Gameplay.Models.Features.Inventory;
using UnityEngine;

namespace Configs.Features.Inventory
{
    [System.Serializable]
    public class ItemConfigData
    {
        [SerializeField] private ItemId itemId;
        [SerializeField] private string itemName;
        [SerializeField] private ItemType itemType;
        [SerializeField] private SpriteId spriteId;
        
        public ItemId ItemId => itemId;
        public string ItemName => itemName;
        public ItemType ItemType => itemType;
        public SpriteId SpriteId => spriteId;
    }
}
