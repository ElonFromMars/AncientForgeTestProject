namespace Gameplay.Models.Features.Quests
{
    public class QuestModel
    {
        public QuestId Id { get; }
        public int CurrentProgress { get; private set; }
        public int RequiredProgress { get; }
        public bool IsCompleted { get; private set; }
        
        public QuestModel(QuestId id, int requiredProgress)
        {
            Id = id;
            CurrentProgress = 0;
            RequiredProgress = requiredProgress;
            IsCompleted = false;
        }
        
        public void UpdateProgress(int increment = 1)
        {
            if (IsCompleted)
                return;
                
            CurrentProgress += increment;
            
            if (CurrentProgress >= RequiredProgress)
            {
                IsCompleted = true;
                CurrentProgress = RequiredProgress;
            }
        }
        
        public float GetProgressPercentage()
        {
            return (float)CurrentProgress / RequiredProgress;
        }
    }
}