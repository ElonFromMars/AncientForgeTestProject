using System.Collections.Generic;
using AssetManagement.Configs;
using AssetManagement.Prefabs;
using AssetManagement.Sprites;
using Configs.Features.Crafting;
using Configs.Features.Machines;
using Gameplay.Models.Features.Crafting.Services;
using Gameplay.Models.Features.Machines;
using Gameplay.Models.Features.Machines.Services;
using Gameplay.Views.UI.Features.Machines;
using UnityEngine;

namespace Gameplay.Controllers.Features
{
    public class MachinesController
    {
        private readonly MachineService _machineService;
        private RecipeService _recipeService;
        
        private readonly UIViewPrefabHolderSO _prefabHolder;
        private readonly RecipeConfigHolderSO _recipeConfigHolder;
        private readonly MachineConfigHolderSO _machineConfigHolder;

        private readonly Dictionary<MachineId, MachineView> _machineViews = new Dictionary<MachineId, MachineView>();
        private MachinesPanelView _machinesPanelView;
        private SpriteHolderSO _spriteHolder;

        public MachinesController(
            MachineService machineService,
            RecipeService recipeService,
            UIViewPrefabHolderSO prefabHolder,
            SpriteHolderSO spriteHolder,
            RecipeConfigHolderSO recipeConfigHolder,
            MachineConfigHolderSO machineConfigHolder
            )
        {
            _recipeService = recipeService;
            _machineService = machineService;
            _prefabHolder = prefabHolder;
            _spriteHolder = spriteHolder;
            _recipeConfigHolder = recipeConfigHolder;
            _machineConfigHolder = machineConfigHolder;
        }
        
        public void Initialize()
        {
            CreateMachinesPanelView();
            RefreshMachinesView();
            SubscribeToMachineEvents();
        }

        public void Dispose()
        {
            UnsubscribeFromMachineEvents();
        }

        public void Update(float deltaTime)
        {
            foreach (var machineView in _machineViews.Values)
            {
                machineView.UpdateProgress();
            }
        }

        public void CreateMachinesPanelView(Transform parent = null)
        {
            var prefab = _prefabHolder.GetPrefab(UIViewId.MachinePanel);
            
            var panelObject = Object.Instantiate(prefab, parent);
            var panelView = panelObject.GetComponent<MachinesPanelView>();
            
            _machinesPanelView = panelView;
        }

        private void SubscribeToMachineEvents()
        {
            foreach (var machine in _machineService.GetAllMachines())
            {
                machine.OnStatusChanged += OnMachineStatusChanged;
                machine.OnCraftingStarted += OnMachineCraftingStarted;
                machine.OnCraftingCompleted += OnMachineCraftingCompleted;
                machine.OnProgressUpdated += OnMachineProgressUpdated;
            }
        }
        
        private void UnsubscribeFromMachineEvents()
        {
            foreach (var machine in _machineService.GetAllMachines())
            {
                machine.OnStatusChanged -= OnMachineStatusChanged;
                machine.OnCraftingStarted -= OnMachineCraftingStarted;
                machine.OnCraftingCompleted -= OnMachineCraftingCompleted;
                machine.OnProgressUpdated -= OnMachineProgressUpdated;
            }
        }

        private void RefreshMachinesView()
        {
            if (_machinesPanelView == null) return;
            
            foreach (var machine in _machineService.GetAllMachines())
            {
                if (machine.IsUnlocked)
                {
                    CreateMachineView(machine);
                }
            }
            
            _machinesPanelView.ArrangeMachines();
        }
        
        private void OnMachineStatusChanged(MachineModel machine)
        {
            if (_machinesPanelView == null) return;
            
            if (machine.IsUnlocked)
            {
                if (!_machineViews.ContainsKey(machine.Id))
                {
                    CreateMachineView(machine);
                    _machinesPanelView.ArrangeMachines();
                }
                else
                {
                    UpdateMachineView(machine);
                }
            }
        }
        
        private void OnMachineCraftingStarted(MachineModel machine)
        {
            if (_machineViews.TryGetValue(machine.Id, out var machineView))
            {
                machineView.StartCrafting();
            }
        }
        
        private void OnMachineCraftingCompleted(MachineModel machine, bool success)
        {
            if (_machineViews.TryGetValue(machine.Id, out var machineView))
            {
                machineView.CompleteCrafting(success);
            }
        }
        
        private void OnMachineProgressUpdated(MachineModel machine)
        {
            if (_machineViews.TryGetValue(machine.Id, out var machineView))
            {
                machineView.UpdateProgress();
            }
        }
        
        private void CreateMachineView(MachineModel machine)
        {
            if (_machinesPanelView == null) return;
            
            if (_machineViews.ContainsKey(machine.Id)) return;
            
            var machineViewPrefab = _prefabHolder.GetPrefab(UIViewId.Machine);
            if (machineViewPrefab == null)
            {
                return;
            }
            
            var machineViewObject = Object.Instantiate(machineViewPrefab, _machinesPanelView.transform);
            var machineView = machineViewObject.GetComponent<MachineView>();
            
            var machineConfig = _machineConfigHolder.Get(machine.Id);
            var machineRecipes = _recipeService.GetRecipesForMachine(machine.Id);
            
            machineView.Construct(_spriteHolder, _recipeConfigHolder, machine, machineConfig, machineRecipes);
            _machineViews[machine.Id] = machineView;
        }
        
        private void UpdateMachineView(MachineModel machine)
        {
            if (_machineViews.TryGetValue(machine.Id, out var machineView))
            {
                machineView.Refresh();
            }
        }
    }
}