using System;
using System.Collections.Generic;
using AssetManagement.Configs;
using AssetManagement.Prefabs;
using AssetManagement.Sprites;
using Configs.Features.Crafting;
using Configs.Features.Inventory;
using Gameplay.Models.Features.Crafting;
using Gameplay.Models.Features.Crafting.Services;
using Gameplay.Models.Features.Machines;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Views.UI.Features.Machines
{
    public class RecipeView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI recipeTitleText;
        [SerializeField] private Button craftButton;
        [SerializeField] private Transform ingredientsContainer;
        [SerializeField] private TextMeshProUGUI successRateText;
        [SerializeField] private TextMeshProUGUI craftingTimeText;

        private RecipeConfigData _recipeConfig;
        private RecipeModel _recipe;
        private MachineModel _machine;
        private List<IngredientItemView> _ingredientItems = new List<IngredientItemView>();
        private UIViewPrefabHolderSO _prefabHolder;
        private SpriteHolderSO _spriteHolder;
        private ItemConfigHolderSO _itemConfigHolder;
        private CraftingService _craftingService;

        public event Action<RecipeModel> OnRecipeCraftRequested;

        public void Construct(
            UIViewPrefabHolderSO prefabHolder,
            SpriteHolderSO spriteHolder,
            ItemConfigHolderSO itemConfigHolder,
            RecipeConfigData recipeConfig, 
            RecipeModel recipe, 
            MachineModel machine,
            CraftingService craftingService
            )
        {
            _itemConfigHolder = itemConfigHolder;
            _prefabHolder = prefabHolder;
            _spriteHolder = spriteHolder;
            _recipeConfig = recipeConfig;
            _recipe = recipe;
            _machine = machine;
            _craftingService = craftingService;

            SetupUI();
            CreateIngredientItems();
            SetupListeners();
        }

        private void OnDestroy()
        {
            if (craftButton != null)
            {
                craftButton.onClick.RemoveListener(OnCraftButtonClicked);
            }
            
            ClearIngredientItems();
        }

        private void SetupUI()
        {
            if (recipeTitleText != null)
            {
                recipeTitleText.text = _recipeConfig.Name;
            }
            
            if (successRateText != null)
            {
                float successRate =_machine.GetSuccessChanceForRecipe(_recipe);
                string successRateBonusText = string.Empty;
                if (_machine.SuccessRateBonus > 0 && _recipe.BaseSuccessChance < 100f)
                {
                    successRateBonusText = $"\n(+{_machine.SuccessRateBonus:0.0}%)";
                }
                successRateText.text = $"sr: {successRate:0.0}%{successRateBonusText}";
            }
            
            if (craftingTimeText != null)
            {
                float recipeBaseCraftingTime = _machine.GetCraftingTimeForRecipe(_recipe);
                string craftingTimeBonusText = string.Empty;
                if(_machine.CraftTimeReduction > 0)
                {
                    craftingTimeBonusText = $"\n(-{_machine.CraftTimeReduction:0.0}s)";
                }
                craftingTimeText.text = $"t: {recipeBaseCraftingTime:0.0}s{craftingTimeBonusText}";
            }
            
            SetInteractable(!_machine.IsBusy);
        }

        private void CreateIngredientItems()
        {
            ClearIngredientItems();
            GameObject ingredientPrefab = _prefabHolder.GetPrefab(UIViewId.IngredientItem);
            
            foreach (var input in _recipe.Ingredients)
            {
                IngredientItemView ingredientItem = Instantiate(ingredientPrefab, ingredientsContainer)
                    .GetComponent<IngredientItemView>();
                var spriteIdIt = _itemConfigHolder.GetItemConfig(input.ItemId).SpriteId;
                ingredientItem.DesiredItemIcon.sprite = _spriteHolder.GetSprite(spriteIdIt);
                
                ingredientItem.Text.text = $"{input.ItemId}x{input.Quantity}";
                
                _ingredientItems.Add(ingredientItem);
            }
            
            IngredientItemView outputItem = Instantiate(ingredientPrefab, ingredientsContainer)
                .GetComponent<IngredientItemView>();
            var spriteId = _itemConfigHolder.GetItemConfig(_recipe.OutputItemId).SpriteId;
            outputItem.DesiredItemIcon.sprite = _spriteHolder.GetSprite(spriteId);
            var outputText = outputItem.GetComponentInChildren<TextMeshProUGUI>();
            if (outputText != null)
            {
                outputText.text = $"→{_recipe.OutputItemId}";
            }
            _ingredientItems.Add(outputItem);
        }

        private void ClearIngredientItems()
        {
            foreach (var item in _ingredientItems)
            {
                Destroy(item.gameObject);
            }
            _ingredientItems.Clear();
        }

        private void SetupListeners()
        {
            if (craftButton != null)
            {
                craftButton.onClick.AddListener(OnCraftButtonClicked);
            }
        }

        private void OnCraftButtonClicked()
        {
            OnRecipeCraftRequested?.Invoke(_recipe);
        }

        public void SetInteractable(bool interactable)
        {
            craftButton.interactable = interactable && _craftingService.CanCraftRecipe(_recipe);
        }
    }
}
