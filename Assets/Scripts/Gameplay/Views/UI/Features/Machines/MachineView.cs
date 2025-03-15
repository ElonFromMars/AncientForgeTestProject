using System;
using System.Collections.Generic;
using AssetManagement.Configs;
using AssetManagement.Prefabs;
using AssetManagement.Sprites;
using Configs.Features.Crafting;
using Configs.Features.Inventory;
using Configs.Features.Machines;
using Gameplay.Models.Features.Crafting;
using Gameplay.Models.Features.Crafting.Services;
using Gameplay.Models.Features.Machines;
using Gameplay.Views.UI.UIElements;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Views.UI.Features.Machines
{
    public class MachineView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI machineTitleText;
        [SerializeField] private Image machineIcon;
        [SerializeField] private ProgressBar progressBar;
        [SerializeField] private TextMeshProUGUI statusText;
        [SerializeField] private Transform recipesContainer;

        private MachineModel _machineModel;
        private MachineConfigData _machineConfig;
        private List<RecipeModel> _availableRecipes = new List<RecipeModel>();
        private List<RecipeView> _recipeViews = new List<RecipeView>();
        private SpriteHolderSO _spriteHolder;
        private RecipeConfigHolderSO _recipeConfigHolder;
        private UIViewPrefabHolderSO _prefabHolder;
        private ItemConfigHolderSO _itemConfigHolder;
        private CraftingService _craftingService;
        public event Action<MachineId, RecipeId> OnRecipeCraftRequested;

        public void Construct(
            SpriteHolderSO spriteHolder,
            RecipeConfigHolderSO recipeConfigHolder,
            UIViewPrefabHolderSO prefabHolder,
            ItemConfigHolderSO itemConfigHolder,
            MachineModel machine, 
            MachineConfigData machineConfig, 
            List<RecipeModel> recipes,
            CraftingService craftingService
            )
        {
            _itemConfigHolder = itemConfigHolder;
            _recipeConfigHolder = recipeConfigHolder;
            _spriteHolder = spriteHolder;
            _prefabHolder = prefabHolder;
            _machineModel = machine;
            _machineConfig = machineConfig;
            _availableRecipes = recipes;
            _craftingService = craftingService;

            SetupUI();
            CreateRecipeViews();
            UpdateProgressBar();
            UpdateStatus();
        }

        private void SetupUI()
        {
            if (machineTitleText != null)
            {
                machineTitleText.text = _machineConfig.Name;
            }
           
            machineIcon.sprite =  _spriteHolder.GetSprite(_machineConfig.SpriteId);
        }

        private void CreateRecipeViews()
        {
            ClearRecipeViews();
            
            var recipeItemPrefab = _prefabHolder.GetPrefab(UIViewId.Recipe);
                
            foreach (RecipeModel recipe in _availableRecipes)
            {
                GameObject recipeItemObject = Instantiate(recipeItemPrefab, recipesContainer);
                var recipeItemView = recipeItemObject.GetComponent<RecipeView>();
                
                if (recipeItemView != null)
                {
                    var recipeConfig = _recipeConfigHolder.Get(recipe.RecipeId);
                    recipeItemView.Construct(
                        _prefabHolder, 
                        _spriteHolder, 
                        _itemConfigHolder, 
                        recipeConfig, 
                        recipe, 
                        _machineModel, 
                        _craftingService
                    );
                    recipeItemView.OnRecipeCraftRequested += HandleRecipeCraftRequested;
                    _recipeViews.Add(recipeItemView);
                }
            }
        }

        private void ClearRecipeViews()
        {
            foreach (var recipeView in _recipeViews)
            {
                if (recipeView != null)
                {
                    recipeView.OnRecipeCraftRequested -= HandleRecipeCraftRequested;
                    Destroy(recipeView.gameObject);
                }
            }
            
            _recipeViews.Clear();
        }

        private void HandleRecipeCraftRequested(RecipeModel recipe)
        {
            OnRecipeCraftRequested?.Invoke(_machineModel.Id, recipe.RecipeId);
        }

        public void StartCrafting()
        {
            UpdateRecipeViewsInteractability(false);
            UpdateStatus();
        }

        public void UpdateProgress()
        {
            UpdateProgressBar();
            UpdateStatus();
        }

        private void UpdateProgressBar()
        {
            if (progressBar != null)
            {
                progressBar.SetProgress(_machineModel.GetProgressNormalized());
                progressBar.gameObject.SetActive(_machineModel.IsBusy);
            }
        }

        private void UpdateStatus()
        {
            if (statusText != null)
            {
                if (_machineModel.IsBusy)
                {
                    float timeRemaining = _machineModel.CraftingDuration - _machineModel.CraftingProgress;
                    statusText.text = $"Crafting... {timeRemaining:F1}s";
                }
                else
                {
                    statusText.text = "Ready";
                }
            }
        }

        private void UpdateRecipeViewsInteractability(bool interactable)
        {
            foreach (var recipeView in _recipeViews)
            {
                recipeView.SetInteractable(interactable);
            }
        }

        public void CompleteCrafting(bool success)
        {
            UpdateRecipeViewsInteractability(true);
            
            if (statusText != null)
            {
                statusText.text = success ? "Success!" : "Failed!";
            }
            
            UpdateProgressBar();
            
            // Reset status message after a delay
            Invoke(nameof(ResetStatus), 2f);//TODO rewrite to coroutines or UniTask
        }

        private void ResetStatus()
        {
            if (statusText != null)
            {
                statusText.text = "Ready";
            }
        }

        public void Refresh()
        {
            UpdateRecipeViewsInteractability(!_machineModel.IsBusy);
            UpdateProgressBar();
            UpdateStatus();
        }

        private void OnDestroy()
        {
            ClearRecipeViews();
        }
    }
}