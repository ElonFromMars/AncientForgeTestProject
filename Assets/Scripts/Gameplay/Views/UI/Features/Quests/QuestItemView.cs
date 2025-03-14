using Gameplay.Models.Features.Quests;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Views.UI.Features.Quests
{
    public class QuestItemView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI questTitleText;
        [SerializeField] private TextMeshProUGUI progressText;
        [SerializeField] private Slider progressSlider;
        [SerializeField] private GameObject completedIcon;
        [SerializeField] private Button detailsButton;
        
        private QuestId _questId;
        
        public QuestId QuestId => _questId;
        
        public void Initialize(QuestId questId, string title, int current, int required, bool isCompleted)
        {
            _questId = questId;
            questTitleText.text = title;
            UpdateProgress(current, required);
            UpdateCompletionStatus(isCompleted);
        }
        
        public void UpdateProgress(int current, int required)
        {
            progressText.text = $"{current}/{required}";
            progressSlider.value = (float)current / required;
        }
        
        public void UpdateCompletionStatus(bool isCompleted)
        {
            completedIcon.SetActive(isCompleted);
        }
        
        public void SetOnDetailsButtonClicked(System.Action<QuestId> callback)
        {
            detailsButton.onClick.RemoveAllListeners();
            detailsButton.onClick.AddListener(() => callback?.Invoke(_questId));
        }
    }
}
