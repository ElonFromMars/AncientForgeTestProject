using System;
using Gameplay.Models.Features.Crafting;
using UnityEngine;

namespace Gameplay.Models.Features.Machines
{
    public class MachineModel
    {
        public MachineId Id { get; private set; }
        public bool IsUnlocked { get; private set; }
        public bool IsBusy { get; private set; }
        public float CraftingProgress { get; private set; }
        public float CraftingDuration { get; private set; }
        public RecipeModel CurrentRecipe { get; private set; }
        public bool HasLuckyCharm { get; private set; }
        
        private float _successRateBonus = 0f;
        private float _craftTimeReduction = 0f;
        
        public event Action<MachineModel> OnCraftingStarted;
        public event Action<MachineModel, bool> OnCraftingCompleted;
        public event Action<MachineModel> OnStatusChanged;
        public event Action<MachineModel> OnProgressUpdated;
        
        public MachineModel(MachineId id, bool isUnlocked = false)
        {
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
            float craftingTime = recipe.BaseCraftingTime;
            craftingTime = Mathf.Max(0f, craftingTime - _craftTimeReduction);
            
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
            var hasLuckyCharm = HasLuckyCharm;
            
            float successChance = recipe?.BaseSuccessChance ?? 0f;
            if (hasLuckyCharm)
            {
                successChance = Mathf.Min(1.0f, successChance + _successRateBonus);
            }
            
            bool isSuccess = UnityEngine.Random.Range(0f, 1f) <= successChance;
            
            IsBusy = false;
            CraftingProgress = 0f;
            CraftingDuration = 0f;
            CurrentRecipe = null;
            HasLuckyCharm = false;
            
            OnCraftingCompleted?.Invoke(this, isSuccess);
            OnStatusChanged?.Invoke(this);
        }
        
        public float GetProgressNormalized()
        {
            if (CraftingDuration <= 0f) return 0f;
            return CraftingProgress / CraftingDuration;
        }
        
        public void SetChanceBonus(float successRateBonus)
        {
            _successRateBonus = successRateBonus;
        }
        
        public void SetTimeBonus(float craftTimeReduction)
        {
            _craftTimeReduction = craftTimeReduction;
        }
        
        public void SetLuckyCharm(bool hasLuckyCharm)
        {
            HasLuckyCharm = hasLuckyCharm;
            OnStatusChanged?.Invoke(this);
        }
    }
}