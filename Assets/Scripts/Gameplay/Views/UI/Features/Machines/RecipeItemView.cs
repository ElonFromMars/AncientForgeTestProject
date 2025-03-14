using System;
using System.Collections.Generic;
using Configs.Features.Crafting;
using Gameplay.Models.Features.Crafting;
using Gameplay.Models.Features.Machines;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Views.UI.Features.Machines
{
    public class RecipeItemView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _recipeTitleText;
        [SerializeField] private Button _craftButton;
        [SerializeField] private Transform _ingredientsContainer;
        [SerializeField] private GameObject _ingredientItemPrefab;
        [SerializeField] private TextMeshProUGUI _successRateText;
        [SerializeField] private TextMeshProUGUI _craftingTimeText;

        private RecipeConfigData _recipeConfig;
        private RecipeModel _recipe;
        private MachineModel _machine;
        private List<GameObject> _ingredientItems = new List<GameObject>();

        public event Action<RecipeModel> OnRecipeCraftRequested;

        public void Construct(RecipeConfigData recipeConfig, RecipeModel recipe, MachineModel machine)
        {
            _recipeConfig = recipeConfig;
            _recipe = recipe;
            _machine = machine;
            
            SetupUI();
            CreateIngredientItems();
            SetupListeners();
        }

        private void OnDestroy()
        {
            if (_craftButton != null)
            {
                _craftButton.onClick.RemoveListener(OnCraftButtonClicked);
            }
            
            ClearIngredientItems();
        }

        private void SetupUI()
        {
            if (_recipeTitleText != null)
            {
                _recipeTitleText.text = _recipeConfig.Name;
            }
            
            if (_successRateText != null)
            {
                float successRate = _recipe.BaseSuccessChance * 100f;
                _successRateText.text = $"Success: {successRate}%";
            }
            
            if (_craftingTimeText != null)
            {
                _craftingTimeText.text = $"Time: {_recipe.BaseCraftingTime}s";
            }
            
            SetInteractable(!_machine.IsBusy);
        }

        private void CreateIngredientItems()
        {
            ClearIngredientItems();
            
            if (_ingredientsContainer == null || _ingredientItemPrefab == null)
                return;
                
            // Add input items
            foreach (var input in _recipe.Ingredients)
            {
                GameObject ingredientItem = Instantiate(_ingredientItemPrefab, _ingredientsContainer);
                
                var ingredientText = ingredientItem.GetComponentInChildren<TextMeshProUGUI>();
                if (ingredientText != null)
                {
                    ingredientText.text = $"{input.ItemId} x{input.Quantity}";
                }
                
                _ingredientItems.Add(ingredientItem);
            }
            
            // Add output item
            GameObject outputItem = Instantiate(_ingredientItemPrefab, _ingredientsContainer);
            var outputText = outputItem.GetComponentInChildren<TextMeshProUGUI>();
            if (outputText != null)
            {
                outputText.text = $"→ {_recipe.OutputItemId}";
            }
            _ingredientItems.Add(outputItem);
        }

        private void ClearIngredientItems()
        {
            foreach (var item in _ingredientItems)
            {
                Destroy(item);
            }
            _ingredientItems.Clear();
        }

        private void SetupListeners()
        {
            if (_craftButton != null)
            {
                _craftButton.onClick.AddListener(OnCraftButtonClicked);
            }
        }

        private void OnCraftButtonClicked()
        {
            OnRecipeCraftRequested?.Invoke(_recipe);
        }

        public void SetInteractable(bool interactable)
        {
            if (_craftButton != null)
            {
                _craftButton.interactable = interactable;
            }
        }
    }
}
