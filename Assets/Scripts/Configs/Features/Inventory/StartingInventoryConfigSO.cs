using System.Collections.Generic;
using UnityEngine;

namespace Configs.Features.Inventory
{
    [CreateAssetMenu(fileName = nameof(StartingInventoryConfigSO), menuName = "ScriptableObjects/Configs/" + nameof(StartingInventoryConfigSO), order = 0)]
    public class StartingInventoryConfigSO : ScriptableObject
    {
        [SerializeField] private List<StartingItemRange> startingItems = new List<StartingItemRange>();

        public List<StartingItemRange> StartingItems => startingItems;
    }
}
