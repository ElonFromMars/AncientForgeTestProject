using System.Collections.Generic;
using Gameplay.Models.Features.Bonuses;
using Gameplay.Models.Features.Inventory;
using UnityEngine;

namespace Configs.Features.Bonuses
{
    [CreateAssetMenu(fileName = nameof(BonusConfigHolderSO), menuName = "ScriptableObjects/Configs/" + nameof(BonusConfigHolderSO), order = 0)]
    public class BonusConfigHolderSO : ScriptableObject
    {
        [SerializeField] private List<BonusConfigData> bonuses = new List<BonusConfigData>();

        public List<BonusConfigData> Bonuses => bonuses;

        
        public BonusConfigData GetBonusConfigByItemId(ItemId itemId)
        {
            return bonuses.Find(b => b.ItemId == itemId);
        }
    }
}
