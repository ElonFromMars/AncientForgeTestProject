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
            float sizeDeltaX = backgroundImage.rectTransform.sizeDelta.x * progress;
            float sizeDeltaY = contentImage.rectTransform.sizeDelta.y;
            contentImage.rectTransform.sizeDelta = new Vector2(sizeDeltaX, sizeDeltaY);
        }
    }
}