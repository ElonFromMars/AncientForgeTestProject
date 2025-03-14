using Configs.Features.Bonuses;
using Configs.Features.Crafting;
using Configs.Features.Inventory;
using Configs.Features.Machines;
using Configs.Features.Quests;
using UnityEngine;

namespace Configs.Common
{
    [CreateAssetMenu(fileName = nameof(ConfigHolderSO), menuName = "ScriptableObjects/Configs/" + nameof(ConfigHolderSO), order = 0)]
    public class ConfigHolderSO : ScriptableObject
    {
        [SerializeField] private MachineConfigHolderSO machineConfigHolder;
        [SerializeField] private RecipeConfigHolderSO recipeConfigHolder;
        [SerializeField] private QuestConfigHolderSO questConfigHolder;
        [SerializeField] private BonusConfigHolderSO bonusConfigHolder;
        [SerializeField] private StartingInventoryConfigSO startingInventoryConfig;
        [SerializeField] private ItemConfigHolderSO itemConfigHolder;

        public MachineConfigHolderSO MachineConfigHolder => machineConfigHolder;
        public RecipeConfigHolderSO RecipeConfigHolder => recipeConfigHolder;
        public QuestConfigHolderSO QuestConfigHolder => questConfigHolder;
        public BonusConfigHolderSO BonusConfigHolder => bonusConfigHolder;
        public StartingInventoryConfigSO StartingInventoryConfig => startingInventoryConfig;
        public ItemConfigHolderSO ItemConfigHolder => itemConfigHolder;
    }
}