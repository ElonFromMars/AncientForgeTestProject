using Gameplay.Models.Features.Crafting;
using Gameplay.Models.Features.Machines;
using Gameplay.Models.Features.Quests;

namespace Configs.Features.Quests
{
    [System.Serializable]
    public class QuestConfigData
    {
        public QuestId id;
        public string name;
        public string description;
        public int requiredProgress;
        public MachineId UnlockedMachineId;
    }
}