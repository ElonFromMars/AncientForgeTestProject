using System;
using Gameplay.Models.Features.Inventory;
using UnityEngine;

namespace Configs.Features.Bonuses
{
    [Serializable]
    public class BonusConfigData
    {
        [SerializeField] private ItemId itemId;
        [SerializeField] private float successRateBonus;
        [SerializeField] private float craftTimeReduction;
        
        public ItemId ItemId => itemId;
        public float SuccessRateBonus => successRateBonus;
        public float CraftTimeReduction => craftTimeReduction;
    }
}
