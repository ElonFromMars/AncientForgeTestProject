using System;
using Gameplay.Models.Features.Inventory.Services;
using Gameplay.Models.Features.Machines;
using Gameplay.Models.Features.Machines.Services;

namespace Gameplay.Models.Features.Crafting.Services
{
    public class CraftingService
    {
        private readonly RecipeService _recipeService;
        private readonly MachineService _machineService;
        private readonly InventoryService _inventoryService;
        
        public event Action<RecipeModel, bool> OnCraftingCompleted;
        
        public CraftingService(RecipeService recipeService, MachineService machineService, InventoryService inventoryService)
        {
            _recipeService = recipeService;
            _machineService = machineService;
            _inventoryService = inventoryService;
            
            foreach (var machine in _machineService.GetAllMachines())
            {
                machine.OnCraftingCompleted += HandleMachineCraftingCompleted;
            }
        }
        
        public bool CanCraftRecipe(RecipeModel recipe)
        {
            return recipe != null && _inventoryService.HasItems(recipe.Ingredients);
        }
        
        public bool StartCrafting(MachineId machineId, RecipeId recipeId)
        {
            var recipe = _recipeService.GetRecipe(recipeId);
            if (recipe == null)
            {
                return false;
            }
            
            var machine = _machineService.GetMachine(machineId);
            if (machine == null || !machine.IsUnlocked || machine.IsBusy)
            {
                return false;
            }
            
            if (!CanCraftRecipe(recipe))
            {
                return false;
            }
            
            _inventoryService.RemoveItems(recipe.Ingredients);
            
            
            machine.StartCrafting(recipe);
            
            return true;
        }
        
        private void HandleMachineCraftingCompleted(MachineModel machine, RecipeModel recipeModel, bool isSuccess)
        {
            if (isSuccess)
            {
                _inventoryService.AddItem(recipeModel.OutputItemId);
            }
            
            OnCraftingCompleted?.Invoke(recipeModel, isSuccess);
        }
        
        public void Update(float deltaTime)
        {
            _machineService.UpdateMachines(deltaTime);
        }
    }
}
