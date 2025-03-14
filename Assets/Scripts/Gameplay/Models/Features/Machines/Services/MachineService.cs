using System.Collections.Generic;
using System.Linq;
using Configs.Common;
using Configs.Features.Crafting;
using Configs.Features.Machines;
using UnityEngine;

namespace Gameplay.Models.Features.Machines.Services
{
    public class MachineService
    {
        private readonly Dictionary<MachineId, MachineModel> _machines;
        private readonly MachineConfigHolderSO _machineConfigHolder;
        
        private float _totalCraftTimeReduction = 0f;
        private float _totalSuccessRateBonus = 0f;

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
                    machineConfig.Id,
                    machineConfig.UnlockedByDefault
                );
            }
            
            return machines;
        }

        public void ApplyCraftTimeReduction(float craftTimeReduction)
        {
            _totalCraftTimeReduction += craftTimeReduction;
            
            foreach (var machine in _machines.Values)
            {
                machine.SetTimeBonus(_totalCraftTimeReduction);
            }
        }

        public void RemoveCraftTimeReduction(float craftTimeReduction)
        {
            _totalCraftTimeReduction -= craftTimeReduction;
            
            _totalCraftTimeReduction = Mathf.Max(0f, _totalCraftTimeReduction);
            
            foreach (var machine in _machines.Values)
            {
                machine.SetTimeBonus(_totalCraftTimeReduction);
            }
        }

        public void ApplySuccessRateBonus(float successRateBonus)
        {
            _totalSuccessRateBonus += successRateBonus;
            
            foreach (var machine in _machines.Values)
            {
                machine.SetChanceBonus(_totalSuccessRateBonus);
            }
        }

        public void RemoveSuccessRateBonus(float successRateBonus)
        {
            _totalSuccessRateBonus -= successRateBonus;
            
            _totalSuccessRateBonus = Mathf.Max(0f, _totalSuccessRateBonus);
            
            foreach (var machine in _machines.Values)
            {
                machine.SetChanceBonus(_totalSuccessRateBonus);
            }
        }
    }
}
