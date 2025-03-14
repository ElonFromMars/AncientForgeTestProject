using System.Collections.Generic;
using System.Linq;
using Configs.Common;
using Configs.Features.Machines;
using Gameplay.Models.Common.Services;
using UnityEngine;

namespace Gameplay.Models.Features.Machines.Services
{
    public class MachineService
    {
        private readonly Dictionary<MachineId, MachineModel> _machines;
        private readonly MachineConfigHolderSO _machineConfigHolder;
        
        private float _craftTimeReduction = 0f;
        private float _successRateBonus = 0f;
        
        private RandomService _randomService = new RandomService();

        public MachineService(ConfigHolderSO configHolder)
        {
            _machineConfigHolder = configHolder.MachineConfigHolder;
            _machines = InitializeMachines();
        }

        public MachineModel GetMachine(MachineId id)
        {
            return _machines.TryGetValue(id, out var machine) ? machine : null;
        }

        public List<MachineModel> GetAllMachines()
        {
            return _machines.Values.ToList();
        }

        public List<MachineModel> GetUnlockedMachines()
        {
            return _machines.Values.Where(m => m.IsUnlocked).ToList();
        }

        public void UnlockMachine(MachineId id)
        {
            if (_machines.TryGetValue(id, out var machine))
            {
                machine.Unlock();
            }
        }

        public void UpdateMachines(float deltaTime)
        {
            foreach (var machine in _machines.Values)
            {
                machine.UpdateCrafting(deltaTime);
            }
        }

        private Dictionary<MachineId, MachineModel> InitializeMachines()
        {
            var machines = new Dictionary<MachineId, MachineModel>();
            
            foreach (var machineConfig in _machineConfigHolder.Machines)
            {
                machines[machineConfig.Id] = new MachineModel(
                    _randomService,
                    machineConfig.Id,
                    machineConfig.UnlockedByDefault
                );
            }
            
            return machines;
        }

        public void ApplyCraftTimeReduction(float craftTimeReduction)
        {
            _craftTimeReduction += craftTimeReduction;
            
            foreach (var machine in _machines.Values)
            {
                machine.SetTimeBonus(_craftTimeReduction);
            }
        }

        public void RemoveCraftTimeReduction(float craftTimeReduction)
        {
            _craftTimeReduction -= craftTimeReduction;
            
            _craftTimeReduction = Mathf.Max(0f, _craftTimeReduction);
            
            foreach (var machine in _machines.Values)
            {
                machine.SetTimeBonus(_craftTimeReduction);
            }
        }

        public void ApplySuccessRateBonus(float successRateBonus)
        {
            _successRateBonus = successRateBonus;
            
            foreach (var machine in _machines.Values)
            {
                machine.SetChanceBonus(_successRateBonus);
            }
        }

        public void RemoveSuccessRateBonus(float successRateBonus)
        {
            _successRateBonus = successRateBonus;
            
            _successRateBonus = Mathf.Max(0f, _successRateBonus);
            
            foreach (var machine in _machines.Values)
            {
                machine.SetChanceBonus(_successRateBonus);
            }
        }
    }
}
