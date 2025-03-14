using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Views.UI.Features.Machines
{
    public class IngredientItemView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Image desiredItemIcon;
        
        public TextMeshProUGUI Text => text;
        public Image DesiredItemIcon => desiredItemIcon;
    }
}