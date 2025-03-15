using System.Collections.Generic;
using AssetManagement.Configs;
using AssetManagement.Prefabs;
using Configs.Features.Quests;
using Gameplay.Models.Features.Quests;
using Gameplay.Models.Features.Quests.Services;
using Gameplay.Views.UI.Features.Quests;
using Gameplay.Views;
using Gameplay.Views.UI.Common;
using UnityEngine;

namespace Gameplay.Controllers.Features
{
    public class QuestController
    {
        private readonly QuestService _questService;
        private readonly UIViewPrefabHolderSO _prefabHolder;
        private readonly QuestConfigHolderSO _questConfigHolder;
        
        private readonly Dictionary<QuestId, QuestView> _questViews = new Dictionary<QuestId, QuestView>();
        private QuestPanelView _questPanelView;
        private HudView _hudView;

        public QuestController(QuestService questService,
            UIViewPrefabHolderSO prefabHolder,
            QuestConfigHolderSO questConfigHolder, 
            HudView hudView
            )
        {
            _hudView = hudView;
            _questService = questService;
            _prefabHolder = prefabHolder;
            _questConfigHolder = questConfigHolder;
        }
        
        public void Initialize()
        {
            CreateQuestPanelView();
            RefreshQuestView();
            SubscribeToQuestEvents();
        }
        
        public void Dispose()
        {
            UnsubscribeFromQuestEvents();
        }
        
        public void CreateQuestPanelView()
        {
            var prefab = _prefabHolder.GetPrefab(UIViewId.QuestPanel);
            
            var panelObject = Object.Instantiate(prefab, _hudView.transform);
            var panelView = panelObject.GetComponent<QuestPanelView>();
            
            _questPanelView = panelView;
        }
        
        private void SubscribeToQuestEvents()
        {
            _questService.OnQuestProgressUpdated += OnQuestProgressUpdated;
            _questService.OnQuestCompleted += OnQuestCompleted;
        }
        
        private void UnsubscribeFromQuestEvents()
        {
            _questService.OnQuestProgressUpdated -= OnQuestProgressUpdated;
            _questService.OnQuestCompleted -= OnQuestCompleted;
        }
        
        private void RefreshQuestView()
        {
            if (_questPanelView == null) return;
            
            ClearQuestViews();
            
            foreach (var quest in _questService.GetAllQuests())
            {
                CreateOrUpdateQuestView(quest);
            }
        }
        
        private void OnQuestProgressUpdated(QuestId questId, int current, int required)
        {
            if (_questPanelView == null) return;
            
            var quest = _questService.GetQuest(questId);
            if (quest != null)
            {
                if (_questViews.TryGetValue(questId, out var questView))
                {
                    questView.UpdateProgressUI(current, required);
                }
                else
                {
                    CreateOrUpdateQuestView(quest);
                }
            }
        }
        
        private void OnQuestCompleted(QuestId questId)
        {
            if (_questPanelView == null) return;
            
            if (_questViews.TryGetValue(questId, out var questView))
            {
                questView.UpdateCompletionStatus(true);
            }
        }
        
        private void CreateOrUpdateQuestView(QuestModel quest)
        {
            if (_questPanelView == null) return;
            
            if (_questViews.TryGetValue(quest.Id, out var existingQuestView))
            {
                existingQuestView.UpdateProgressUI(quest.CurrentProgress, quest.RequiredProgress);
                existingQuestView.UpdateCompletionStatus(quest.IsCompleted);
                return;
            }
            
            var questViewPrefab = _prefabHolder.GetPrefab(UIViewId.Quest);
            if (questViewPrefab == null)
            {
                return;
            }
            
            var questViewObject = Object.Instantiate(questViewPrefab, _questPanelView.transform);
            var questView = questViewObject.GetComponent<QuestView>();
            
            var questConfig = _questConfigHolder.Get(quest.Id);
            if (questConfig == null) return;
            
            questView.Construct(
                quest.Id, 
                questConfig.name, 
                questConfig.description, 
                quest.CurrentProgress, 
                quest.RequiredProgress,
                quest.IsCompleted
            );
            
            _questViews[quest.Id] = questView;
        }
        
        private void ClearQuestViews()
        {
            foreach (var questView in _questViews.Values)
            {
                Object.Destroy(questView.gameObject);
            }
            
            _questViews.Clear();
        }
    }
}