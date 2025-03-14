using AssetManagement.Sprites;
using Gameplay.Models.Features.Machines;
using UnityEngine;

namespace Configs.Features.Machines
{
    [System.Serializable]
    public class MachineConfigData
    {
        [SerializeField] private MachineId id;
        [SerializeField] private string name;
        [SerializeField] private string description;
        [SerializeField] private bool unlockedByDefault;
        [SerializeField] private SpriteId spriteId;
        
        public MachineId Id => id;
        public string Name => name;
        public string Description => description;
        public bool UnlockedByDefault => unlockedByDefault;
        public SpriteId SpriteId => spriteId;
    }
}