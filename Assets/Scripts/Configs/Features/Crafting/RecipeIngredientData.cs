using Gameplay.Models.Features.Inventory;
using UnityEngine;

namespace Configs.Features.Crafting
{
    [System.Serializable]
    public class RecipeIngredientData
    {
        [SerializeField] private ItemId itemId;
        [SerializeField] private int quantity = 1;

        public ItemId ItemId => itemId;
        public int Quantity => quantity;
    }
}