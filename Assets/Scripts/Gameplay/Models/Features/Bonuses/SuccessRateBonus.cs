using Gameplay.Models.Features.Inventory;
using Gameplay.Models.Features.Machines.Services;

namespace Gameplay.Models.Features.Bonuses
{
    public class SuccessRateBonus : BonusModel
    {
        private readonly float _successRateBonus;
        
        public SuccessRateBonus(ItemId itemId, float successRateBonus, MachineService machineService) : base(itemId, machineService)
        {
            _successRateBonus = successRateBonus;
        }

        protected override void ApplyBonus()
        {
            if (MachineService != null)
            {
                MachineService.ApplySuccessRateBonus(_successRateBonus);
            }
        }

        protected override void RemoveBonus()
        {
            if (MachineService != null)
            {
                MachineService.RemoveSuccessRateBonus(_successRateBonus);
            }
        }
    }
}
