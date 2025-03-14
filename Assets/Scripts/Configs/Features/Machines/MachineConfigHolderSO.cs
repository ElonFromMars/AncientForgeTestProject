using System.Collections.Generic;
using Gameplay.Models.Features.Machines;
using UnityEngine;

namespace Configs.Features.Machines
{
    [CreateAssetMenu(fileName = nameof(MachineConfigHolderSO), menuName = "ScriptableObjects/Configs/" + nameof(MachineConfigHolderSO), order = 0)]
    public class MachineConfigHolderSO : ScriptableObject
    {
        [SerializeField] private List<MachineConfigData> machines = new List<MachineConfigData>();

        public List<MachineConfigData> Machines => machines;

        public MachineConfigData Get(MachineId machineId)
        {
            return machines.Find(m => m.Id == machineId);
        }
    }
}
