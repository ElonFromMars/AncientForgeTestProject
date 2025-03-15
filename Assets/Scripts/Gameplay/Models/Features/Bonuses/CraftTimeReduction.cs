using Gameplay.Models.Features.Inventory;
using Gameplay.Models.Features.Machines.Services;

namespace Gameplay.Models.Features.Bonuses
{
    public class CraftTimeReduction : BonusModel
    {
        private readonly float _craftTimeReduction;

        public CraftTimeReduction(ItemId itemId, float craftTimeReduction, MachineService machineService) : base(itemId, machineService)
        {
            _craftTimeReduction = craftTimeReduction;
        }

        protected override void ApplyBonus()
        {
            if (MachineService != null)
            {
                MachineService.ApplyCraftTimeReduction(_craftTimeReduction);
            }
        }

        protected override void RemoveBonus()
        {
            if (MachineService != null)
            {
                MachineService.RemoveCraftTimeReduction(_craftTimeReduction);
            }
        }
    }
}
