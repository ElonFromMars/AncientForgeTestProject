using Gameplay.Models.Features.Inventory;
using Gameplay.Models.Features.Machines.Services;

namespace Gameplay.Models.Features.Bonuses
{
    public class BonusModel
    {
        protected MachineService MachineService;
        
        public ItemId ItemId { get; private set; }
        public bool IsActive { get; private set; }
        
        public BonusModel(ItemId itemId, MachineService machineService)
        {
            MachineService = machineService;
            ItemId = itemId;
            IsActive = false;
        }
        
        public void SetActive(bool active)
        {
            if (IsActive == active)
                return;
            
            if (active)
            {
                ApplyBonus();
            }
            else
            {
                RemoveBonus();
            }
            
            IsActive = active;
        }

        protected virtual void ApplyBonus() { }

        protected virtual void RemoveBonus() { }
    }
}
