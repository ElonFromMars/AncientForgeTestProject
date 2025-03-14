using Configs.Features.Bonuses;
using Gameplay.Models.Features.Crafting.Services;
using Gameplay.Models.Features.Inventory;
using Gameplay.Models.Features.Machines.Services;

namespace Gameplay.Models.Features.Bonuses
{
    public class BonusFactory
    {
        private readonly MachineService _machineService;

        public BonusFactory(MachineService machineService)
        {
            _machineService = machineService;
        }
        
        public BonusModel CreateBonus(BonusConfigData bonusConfig)
        {
            if (bonusConfig.ItemId == ItemId.LuckyCharm)
            {
                return new SuccessRateBonus(bonusConfig.ItemId, bonusConfig.SuccessRateBonus, _machineService);
            }
            else if (bonusConfig.ItemId == ItemId.TimeAmulet)
            {
                return new CraftTimeReduction(bonusConfig.ItemId, bonusConfig.CraftTimeReduction, _machineService);
            }

            return null;
        }
    }
}
