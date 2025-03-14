using System.Collections.Generic;
using Gameplay.Models.Features.Quests;
using UnityEngine;

namespace Configs.Features.Quests
{
    [CreateAssetMenu(fileName = "QuestConfig", menuName = "Game/Configs/Quest Config")]
    public class QuestConfigHolderSO : ScriptableObject
    {
        [SerializeField] private List<QuestConfigData> quests;
        public List<QuestConfigData> Quests => quests;
        
        public QuestConfigData Get(QuestId id)
        {
            return quests.Find(m => m.id == id);
        }
    }
}
