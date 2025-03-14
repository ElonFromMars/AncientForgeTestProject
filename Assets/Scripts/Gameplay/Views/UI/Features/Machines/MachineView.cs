using System.Collections.Generic;
using AssetManagement.Sprites;
using Configs.Features.Crafting;
using Configs.Features.Machines;
using Gameplay.Models.Features.Crafting;
using Gameplay.Models.Features.Machines;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Views.UI.Features.Machines
{
    public class MachineView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _machineTitleText;
        [SerializeField] private Image _machineIcon;
        [SerializeField] private GameObject _craftingPanel;
        [SerializeField] private Slider _progressBar;
        [SerializeField] private TextMeshProUGUI _statusText;
        [SerializeField] private Transform _recipesContainer;
        [SerializeField] private GameObject _recipeItemPrefab;

        private MachineModel _machineModel;
        private MachineConfigData _machineConfig;
        private List<RecipeModel> _availableRecipes = new List<RecipeModel>();
        private List<RecipeItemView> _recipeViews = new List<RecipeItemView>();
        private SpriteHolderSO _spriteHolder;
        private RecipeConfigHolderSO _recipeConfigHolder;

        public void Construct(
            SpriteHolderSO spriteHolder,
            RecipeConfigHolderSO recipeConfigHolder,
            MachineModel machine, 
            MachineConfigData machineConfig, 
            List<RecipeModel> recipes
            )
        {
            _recipeConfigHolder = recipeConfigHolder;
            _spriteHolder = spriteHolder;
            _machineModel = machine;
            _machineConfig = machineConfig;
            _availableRecipes = recipes;

            SetupUI();
            CreateRecipeViews();
            UpdateProgressBar();
            UpdateStatus();
        }

        private void SetupUI()
        {
            if (_machineTitleText != null)
            {
                _machineTitleText.text = _machineConfig.Name;
            }
           
            _machineIcon.sprite =  _spriteHolder.GetSprite(_machineConfig.SpriteId);
        }

        private void CreateRecipeViews()
        {
            ClearRecipeViews();
            
            if (_recipesContainer == null || _recipeItemPrefab == null)
                return;
                
            foreach (RecipeModel recipe in _availableRecipes)
            {
                GameObject recipeItemObject = Instantiate(_recipeItemPrefab, _recipesContainer);
                var recipeItemView = recipeItemObject.GetComponent<RecipeItemView>();
                
                if (recipeItemView != null)
                {
                    var recipeConfig = _recipeConfigHolder.Get(recipe.RecipeId);
                    recipeItemView.Construct(recipeConfig, recipe, _machineModel);
                    recipeItemView.OnRecipeCraftRequested += OnRecipeCraftRequested;
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
                    recipeView.OnRecipeCraftRequested -= OnRecipeCraftRequested;
                    Destroy(recipeView.gameObject);
                }
            }
            
            _recipeViews.Clear();
        }

        private void OnRecipeCraftRequested(RecipeModel recipe)
        {
            
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
            if (_progressBar != null)
            {
                _progressBar.value = _machineModel.GetProgressNormalized();
                _progressBar.gameObject.SetActive(_machineModel.IsBusy);
            }
        }

        private void UpdateStatus()
        {
            if (_statusText != null)
            {
                if (_machineModel.IsBusy)
                {
                    float timeRemaining = _machineModel.CraftingDuration - _machineModel.CraftingProgress;
                    _statusText.text = $"Crafting... {timeRemaining:F1}s";
                }
                else
                {
                    _statusText.text = "Ready";
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
            
            if (_statusText != null)
            {
                _statusText.text = success ? "Success!" : "Failed!";
            }
            
            UpdateProgressBar();
            
            // Reset status message after a delay
            Invoke(nameof(ResetStatus), 2f);
        }

        private void ResetStatus()
        {
            if (_statusText != null)
            {
                _statusText.text = "Ready";
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