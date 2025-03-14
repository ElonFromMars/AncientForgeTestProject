using System;
using System.Collections.Generic;
using Configs.Features.Quests;

namespace Gameplay.Models.Features.Quests.Services
{
    public class QuestService
    {
        private readonly Dictionary<QuestId, QuestModel> _quests = new Dictionary<QuestId, QuestModel>();
        private QuestConfigHolderSO _questConfigHolderSo;

        public event Action<QuestId> OnQuestCompleted;
        public event Action<QuestId, int, int> OnQuestProgressUpdated;
        
        public QuestService(QuestConfigHolderSO questConfigHolderSo)
        {
            _questConfigHolderSo = questConfigHolderSo;
        }

        public void Initialize()
        {
            foreach (var questConfigIt in _questConfigHolderSo.Quests)
            {
                var quest = new QuestModel(
                    questConfigIt.id,
                    questConfigIt.requiredProgress
                );
                
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
    }
}
