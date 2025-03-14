using System.Collections.Generic;
using Gameplay.Models.Features.Inventory;
using UnityEngine;

namespace Configs.Features.Inventory
{
    [CreateAssetMenu(fileName = nameof(StartingInventoryConfigSO), menuName = "ScriptableObjects/Configs/" + nameof(StartingInventoryConfigSO), order = 0)]
    public class StartingInventoryConfigSO : ScriptableObject
    {
        [System.Serializable]
        public class StartingItemRange
        {
            public ItemId ItemId;
            public int MinAmount;
            public int MaxAmount;
            [Range(0, 100)]
            public float ChanceToReceive = 100f;
        }

        [SerializeField] private List<StartingItemRange> startingItems = new List<StartingItemRange>();

        public List<StartingItemRange> StartingItems => startingItems;
    }
}
