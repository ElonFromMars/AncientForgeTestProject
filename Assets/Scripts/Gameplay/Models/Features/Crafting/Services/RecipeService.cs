using System.Collections.Generic;
using System.Linq;
using Configs;
using Configs.Common;
using Configs.Features.Crafting;
using Gameplay.Models.Features.Machines;

namespace Gameplay.Models.Features.Crafting.Services
{
    public class RecipeService
    {
        private List<RecipeModel> _recipes;
        private readonly RecipeConfigHolderSO _recipeConfigHolder;
        
        public RecipeService(ConfigHolderSO configHolder)
        {
            _recipeConfigHolder = configHolder.RecipeConfigHolder;
        }

        public void Initialize()
        {
            _recipes = new List<RecipeModel>();
            
            foreach (var recipeConfig in _recipeConfigHolder.Recipes)
            {
                var ingredients = new List<RecipeIngredient>();
                foreach (var ingredientData in recipeConfig.Ingredients)
                {
                    ingredients.Add(new RecipeIngredient(ingredientData.ItemId, ingredientData.Quantity));
                }
                
                _recipes.Add(new RecipeModel(
                    recipeId: recipeConfig.Id,
                    requiredMachine: recipeConfig.RequiredMachine,
                    ingredients: ingredients,
                    outputItemId: recipeConfig.OutputItemId,
                    baseCraftingTime: recipeConfig.BaseCraftingTime,
                    baseSuccessChance: recipeConfig.BaseSuccessChance
                ));
            }
        }

        public List<RecipeModel> GetRecipesForMachine(MachineId machineId)
        {
            return _recipes.Where(r => r.RequiredMachine == machineId).ToList();
        }

        public RecipeModel GetRecipe(RecipeId recipeId)
        {
            return _recipes.FirstOrDefault(r => r.RecipeId == recipeId);
        }
    }
}
