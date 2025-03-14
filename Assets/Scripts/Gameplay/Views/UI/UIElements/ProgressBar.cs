using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Views.UI.UIElements
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image contentImage;
        
        public void SetProgress(float progress)
        {
            contentImage.rectTransform.sizeDelta 
                = new Vector2(backgroundImage.rectTransform.rect.width, contentImage.rectTransform.sizeDelta.y);
            
            float clampedProgress = Mathf.Clamp01(progress);
            contentImage.transform.localScale = new Vector3(clampedProgress, 1, 1);
        }
    }
}