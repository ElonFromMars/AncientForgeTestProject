using Gameplay.Models.Features.Inventory;
using UnityEngine;

namespace Configs.Features.Inventory
{
    [System.Serializable]
    public class StartingItemRange
    {
        public ItemId ItemId;
        public int MinAmount;
        public int MaxAmount;
        [Range(0f, 100f)]
        public float ChanceToReceive = 100f;
    }
}