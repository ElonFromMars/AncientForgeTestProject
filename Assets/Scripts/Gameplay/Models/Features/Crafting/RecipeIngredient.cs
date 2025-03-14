using Gameplay.Models.Features.Inventory;

namespace Gameplay.Models.Features.Crafting
{
    public class RecipeIngredient
    {
        public ItemId ItemId { get; private set; }
        public int Quantity { get; private set; }
        
        public RecipeIngredient(ItemId itemId, int quantity = 1)
        {
            ItemId = itemId;
            Quantity = quantity;
        }
    }
}
