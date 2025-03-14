using Gameplay.Models.Features.Crafting;
using Gameplay.Models.Features.Machines;
using Gameplay.Models.Features.Quests;
using System.Collections.Generic;
using Gameplay.Models.Features.Inventory;
using UnityEngine.Serialization;

namespace Configs.Features.Quests
{
    [System.Serializable]
    public class QuestConfigData
    {
        public QuestId id;
        public string name;
        public string description;
        public int requiredProgress;
        [FormerlySerializedAs("UnlockedMachineId")] public MachineId unlockedMachineId;
        public List<ItemId> trackedItemIds = new List<ItemId>();
    }
}