using System;
using Gameplay.Models.Common.Services;
using Gameplay.Models.Common.Utils;
using Gameplay.Models.Features.Crafting;

namespace Gameplay.Models.Features.Machines
{
    public class MachineModel
    {
        private RandomService _randomService;
        
        public MachineId Id { get; private set; }
        public bool IsUnlocked { get; private set; }
        public bool IsBusy { get; private set; }
        public float CraftingProgress { get; private set; }
        public float CraftingDuration { get; private set; }
        public RecipeModel CurrentRecipe { get; private set; }

        public float CraftTimeReduction { get; private set; } = 0f;
        public float SuccessRateBonus { get; private set; } = 0f;

        public event Action<MachineModel> OnCraftingStarted;
        public event Action<MachineModel, RecipeModel, bool> OnCraftingCompleted;
        public event Action<MachineModel> OnStatusChanged;
        public event Action<MachineModel> OnProgressUpdated;
        
        public MachineModel(RandomService randomService, MachineId id, bool isUnlocked = false)
        {
            _randomService = randomService;
            Id = id;
            IsUnlocked = isUnlocked;
            IsBusy = false;
            CraftingProgress = 0f;
            CraftingDuration = 0f;
        }
        
        public void Unlock()
        {
            IsUnlocked = true;
            OnStatusChanged?.Invoke(this);
        }
        
        public void StartCrafting(RecipeModel recipe)
        {
            float craftingTime = GetCraftingTimeForRecipe(recipe);
            
            IsBusy = true;
            CraftingProgress = 0f;
            CraftingDuration = craftingTime;
            CurrentRecipe = recipe;
            
            OnCraftingStarted?.Invoke(this);
            OnStatusChanged?.Invoke(this);
        }
        
        public void UpdateCrafting(float deltaTime)
        {
            if (!IsBusy) return;
            
            CraftingProgress += deltaTime;
            OnProgressUpdated?.Invoke(this);
            
            if (CraftingProgress >= CraftingDuration)
            {
                CompleteCrafting();
            }
        }
        
        public void CompleteCrafting()
        {
            if (!IsBusy) return;
            
            var recipe = CurrentRecipe;

            float successChance = GetSuccessChanceForRecipe(recipe);
            
            bool isSuccess = _randomService.GetRandomFloat(0f, 100f) <= successChance;
            
            IsBusy = false;
            CraftingProgress = 0f;
            CraftingDuration = 0f;
            CurrentRecipe = null;
            
            OnCraftingCompleted?.Invoke(this, recipe, isSuccess);
            OnStatusChanged?.Invoke(this);
        }
        
        public float GetProgressNormalized()
        {
            if (CraftingDuration <= 0f) return 0f;
            return CraftingProgress / CraftingDuration;
        }
        
        public void SetChanceBonus(float successRateBonus)
        {
            SuccessRateBonus = successRateBonus;
        }
        
        public void SetTimeBonus(float craftTimeReduction)
        {
            CraftTimeReduction = craftTimeReduction;
        }
        
        public float GetSuccessChanceForRecipe(RecipeModel recipe)
        {
            if (recipe == null) return 0f;
            
            float successChance = recipe.BaseSuccessChance;
            successChance = MathUtils.Min(100f, successChance + SuccessRateBonus);
            
            return successChance;
        }

        public float GetCraftingTimeForRecipe(RecipeModel recipe)
        {
            return MathUtils.Max(0f, recipe.BaseCraftingTime - CraftTimeReduction);
        }
    }
}