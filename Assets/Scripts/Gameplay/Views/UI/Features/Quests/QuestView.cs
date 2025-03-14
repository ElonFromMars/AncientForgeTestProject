using Gameplay.Models.Features.Quests;
using Gameplay.Views.UI.UIElements;
using TMPro;
using UnityEngine;

namespace Gameplay.Views.UI.Features.Quests
{
    public class QuestView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI questNameText;
        [SerializeField] private TextMeshProUGUI questDescriptionText;
        [SerializeField] private TextMeshProUGUI progressText;
        [SerializeField] private ProgressBar progressBar;
        [SerializeField] private GameObject completedBadge;
        
        private QuestId _questId;
        
        public QuestId QuestId => _questId;

        public void Construct(QuestId questId, string questName, string description, int currentProgress, int requiredProgress, bool isCompleted)
        {
            _questId = questId;
            
            questNameText.text = questName;
            questDescriptionText.text = description;
            
            UpdateProgressUI(currentProgress, requiredProgress);
            UpdateCompletionStatus(isCompleted);
        }
        
        public void UpdateProgressUI(int current, int required)
        {
            progressText.text = $"{current}/{required}";
            progressBar.SetProgress((float)current / required);
        }
        
        public void UpdateCompletionStatus(bool isCompleted)
        {
            completedBadge.SetActive(isCompleted);
        }
    }
}
