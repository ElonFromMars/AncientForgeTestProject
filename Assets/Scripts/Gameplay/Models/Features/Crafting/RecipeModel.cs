using System.Collections.Generic;
using Gameplay.Models.Features.Inventory;
using Gameplay.Models.Features.Machines;

namespace Gameplay.Models.Features.Crafting
{
    public class RecipeModel
    {
        public RecipeId RecipeId { get; private set; }
        public MachineId RequiredMachine { get; private set; }
        public List<RecipeIngredient> Ingredients { get; private set; }
        public ItemId OutputItemId { get; private set; }
        public float BaseCraftingTime { get; private set; }
        public float BaseSuccessChance { get; private set; }
        
        public RecipeModel(RecipeId recipeId, MachineId requiredMachine, List<RecipeIngredient> ingredients, 
                          ItemId outputItemId, float baseCraftingTime, float baseSuccessChance)
        {
            RecipeId = recipeId;
            RequiredMachine = requiredMachine;
            Ingredients = ingredients;
            OutputItemId = outputItemId;
            BaseCraftingTime = baseCraftingTime;
            BaseSuccessChance = baseSuccessChance;
        }
    }
}
