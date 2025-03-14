using System.Collections.Generic;
using Gameplay.Views;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Views.UI.Features.Quests
{
    public class QuestPanelView : MonoBehaviour
    {
        [SerializeField] private GridLayoutGroup gridLayout;
        [SerializeField] private RectTransform contentRect;
        
        private readonly List<QuestView> _questViews = new List<QuestView>();
        
        public void AddQuestView(QuestView questView)
        {
            if (questView != null && !_questViews.Contains(questView))
            {
                questView.transform.SetParent(transform, false);
                _questViews.Add(questView);
            }
        }
        
        public void RemoveQuestView(QuestView questView)
        {
            if (questView != null && _questViews.Contains(questView))
            {
                _questViews.Remove(questView);
                Destroy(questView.gameObject);
            }
        }

        public void ArrangeQuests()
        {
            
        }
    }
}