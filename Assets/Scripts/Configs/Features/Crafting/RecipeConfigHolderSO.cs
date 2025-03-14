using System.Collections.Generic;
using Gameplay.Models.Features.Crafting;
using UnityEngine;

namespace Configs.Features.Crafting
{
    [CreateAssetMenu(fileName = nameof(RecipeConfigHolderSO), menuName = "ScriptableObjects/Configs/" + nameof(RecipeConfigHolderSO), order = 1)]
    public class RecipeConfigHolderSO : ScriptableObject
    {
        [SerializeField] private List<RecipeConfigData> recipes = new List<RecipeConfigData>();

        public List<RecipeConfigData> Recipes => recipes;

        public RecipeConfigData Get(RecipeId recipeId)
        {
            return recipes.Find(r => r.Id == recipeId);
        }
    }
}