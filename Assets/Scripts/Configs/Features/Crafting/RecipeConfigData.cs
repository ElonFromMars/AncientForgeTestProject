using System.Collections.Generic;
using Gameplay.Models.Features.Crafting;
using Gameplay.Models.Features.Inventory;
using Gameplay.Models.Features.Machines;
using UnityEngine;

namespace Configs.Features.Crafting
{
    [System.Serializable]
    public class RecipeConfigData
    {
        [SerializeField] private RecipeId id;
        [SerializeField] private MachineId requiredMachine;
        [SerializeField] private List<RecipeIngredientData> ingredients;
        [SerializeField] private ItemId outputItemId;
        [SerializeField] private float baseCraftingTime;
        [SerializeField] private float baseSuccessChance;

        public RecipeId Id => id;
        public MachineId RequiredMachine => requiredMachine;
        public List<RecipeIngredientData> Ingredients => ingredients;
        public ItemId OutputItemId => outputItemId;
        public float BaseCraftingTime => baseCraftingTime;
        public float BaseSuccessChance => baseSuccessChance;
        public string Name => id.ToString();
    }
}