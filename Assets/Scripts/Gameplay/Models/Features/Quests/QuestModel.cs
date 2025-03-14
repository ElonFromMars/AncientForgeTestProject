using System;
using System.Collections.Generic;
using Gameplay.Models.Features.Crafting;
using Gameplay.Models.Features.Inventory;
using Gameplay.Models.Features.Crafting.Services;
using Gameplay.Models.Features.Machines;
using Gameplay.Models.Features.Machines.Services;

namespace Gameplay.Models.Features.Quests
{
    public class QuestModel
    {
        public QuestId Id { get; }
        public int CurrentProgress { get; private set; }
        public int RequiredProgress { get; }
        public bool IsCompleted { get; private set; }
        public MachineId UnlockedMachineId { get; }
        
        private readonly HashSet<ItemId> _trackedItemIds = new HashSet<ItemId>();
        private CraftingService _craftingService;
        private MachineService _machineService;
        
        public event Action<QuestId, int, int> OnProgressUpdated;
        public event Action<QuestId> OnCompleted;
        
        public QuestModel(
            QuestId id, 
            int requiredProgress, 
            MachineId unlockedMachineId, 
            CraftingService craftingService, 
            MachineService machineService, 
            IEnumerable<ItemId> trackedItemIds
            )
        {
            Id = id;
            CurrentProgress = 0;
            RequiredProgress = requiredProgress;
            IsCompleted = false;
            UnlockedMachineId = unlockedMachineId;
            
            _craftingService = craftingService;
            _machineService = machineService;
            
            if (trackedItemIds != null)
            {
                foreach (var itemId in trackedItemIds)
                {
                    _trackedItemIds.Add(itemId);
                }
            }
        }
        
        public void Initialize()
        {
            _craftingService.OnCraftingCompleted += HandleCraftingCompleted;
        }
        
        private void HandleCraftingCompleted(RecipeModel recipe, bool isSuccess)
        {
            if (isSuccess && !IsCompleted && ShouldTrackRecipe(recipe.OutputItemId))
            {
                UpdateProgress();
            }
        }
        
        public void UpdateProgress(int increment = 1)
        {
            if (IsCompleted)
                return;
                
            CurrentProgress += increment;
            
            OnProgressUpdated?.Invoke(Id, CurrentProgress, RequiredProgress);
            
            if (CurrentProgress >= RequiredProgress)
            {
                IsCompleted = true;
                CurrentProgress = RequiredProgress;
                
                _machineService.UnlockMachine(UnlockedMachineId);
                
                OnCompleted?.Invoke(Id);
            }
        }
        
        public float GetProgressPercentage()
        {
            return (float)CurrentProgress / RequiredProgress;
        }
        
        public bool ShouldTrackRecipe(ItemId itemId)
        {
            if (_trackedItemIds.Count == 0)
                return true;
                
            return _trackedItemIds.Contains(itemId);
        }
        
        public void Dispose()
        {
            if (_craftingService != null)
            {
                _craftingService.OnCraftingCompleted -= HandleCraftingCompleted;
                _craftingService = null;
            }
            
            _machineService = null;
        }
    }
}