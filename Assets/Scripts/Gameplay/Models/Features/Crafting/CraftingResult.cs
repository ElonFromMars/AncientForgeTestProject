using Gameplay.Models.Features.Inventory;

namespace Gameplay.Models.Features.Crafting
{
    public class CraftingResult
    {
        public bool IsSuccess { get; set; }
        public ItemId OutputItemId { get; set; }
    }
}
