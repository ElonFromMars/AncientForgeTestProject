using Gameplay.Models.Features.Inventory;
using UnityEngine;
using UnityEngine.Serialization;

namespace Configs.Features.Inventory
{
    [System.Serializable]
    public class StartingItemRange
    {
        [FormerlySerializedAs("ItemId")] public ItemId itemId;
        [FormerlySerializedAs("MinAmount")] public int minAmount;
        [FormerlySerializedAs("MaxAmount")] public int maxAmount;
        [FormerlySerializedAs("ChanceToReceive")] [Range(0f, 100f)]
        public float chanceToReceive = 100f;
    }
}