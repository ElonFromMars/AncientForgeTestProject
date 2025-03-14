using System;
using System.Collections.Generic;
using Configs.Features.Quests;
using Gameplay.Models.Features.Crafting.Services;
using Gameplay.Models.Features.Machines.Services;

namespace Gameplay.Models.Features.Quests.Services
{
    public class QuestService
    {
        private readonly Dictionary<QuestId, QuestModel> _quests = new Dictionary<QuestId, QuestModel>();
        private QuestConfigHolderSO _questConfigHolderSo;
        private readonly CraftingService _craftingService;
        private readonly MachineService _machineService;

        public event Action<QuestId> OnQuestCompleted;
        public event Action<QuestId, int, int> OnQuestProgressUpdated;
        
        public QuestService(QuestConfigHolderSO questConfigHolderSo, CraftingService craftingService, MachineService machineService)
        {
            _questConfigHolderSo = questConfigHolderSo;
            _craftingService = craftingService;
            _machineService = machineService;
        }

        public void Initialize()
        {
            foreach (var questConfigIt in _questConfigHolderSo.Quests)
            {
                var quest = new QuestModel(
                    questConfigIt.id,
                    questConfigIt.requiredProgress,
                    questConfigIt.unlockedMachineId,
                    _craftingService,
                    _machineService,
                    questConfigIt.trackedItemIds
                );
                quest.Initialize();
                
                quest.OnProgressUpdated += HandleQuestProgressUpdated;
                quest.OnCompleted += HandleQuestCompleted;
                
                _quests.Add(questConfigIt.id, quest);
            }
        }
        
        public bool IsQuestCompleted(QuestId questId)
        {
            return _quests.TryGetValue(questId, out var quest) && quest.IsCompleted;
        }
        
        public QuestModel GetQuest(QuestId questId)
        {
            if (_quests.TryGetValue(questId, out var quest))
            {
                return quest;
            }
            
            return null;
        }
        
        public IEnumerable<QuestModel> GetAllQuests()
        {
            return _quests.Values;
        }
        
        private void HandleQuestProgressUpdated(QuestId questId, int currentProgress, int requiredProgress)
        {
            OnQuestProgressUpdated?.Invoke(questId, currentProgress, requiredProgress);
        }
        
        private void HandleQuestCompleted(QuestId questId)
        {
            OnQuestCompleted?.Invoke(questId);
        }
        
        public void Dispose()
        {
            foreach (var quest in _quests.Values)
            {
                quest.OnProgressUpdated -= HandleQuestProgressUpdated;
                quest.OnCompleted -= HandleQuestCompleted;
                quest.Dispose();
            }
        }
    }
}
