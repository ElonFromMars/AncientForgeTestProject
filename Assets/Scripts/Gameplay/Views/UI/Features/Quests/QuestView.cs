using Gameplay.Controllers;
using Gameplay.Models.Features.Quests;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Views
{
    public class QuestView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI questNameText;
        [SerializeField] private TextMeshProUGUI questDescriptionText;
        [SerializeField] private TextMeshProUGUI progressText;
        [SerializeField] private Slider progressSlider;
        [SerializeField] private GameObject completedBadge;
        
        private QuestId _questId;
        
        public QuestId QuestId => _questId;

        public void Construct(QuestId questId, string name, string description, int currentProgress, int requiredProgress, bool isCompleted)
        {
            _questId = questId;
            
            questNameText.text = name;
            questDescriptionText.text = description;
            
            UpdateProgressUI(currentProgress, requiredProgress);
            UpdateCompletionStatus(isCompleted);
        }
        
        public void UpdateProgressUI(int current, int required)
        {
            progressText.text = $"{current}/{required}";
            progressSlider.value = (float)current / required;
        }
        
        public void UpdateCompletionStatus(bool isCompleted)
        {
            completedBadge.SetActive(isCompleted);
        }
    }
}
