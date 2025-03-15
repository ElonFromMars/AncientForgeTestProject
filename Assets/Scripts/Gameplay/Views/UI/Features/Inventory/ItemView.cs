using Gameplay.Models.Features.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Views.UI.Features.Inventory
{
    public class ItemView : MonoBehaviour
    {
        [SerializeField] private Image itemIcon;
        [SerializeField] private TextMeshProUGUI quantityText;
        [SerializeField] private RectTransform quantityContainer;
        
        private ItemId _itemId;
        private int _quantity;
        
        public ItemId ItemId => _itemId;
        
        public void Construct(ItemId itemId, Sprite icon, int quantity)
        {
            _itemId = itemId;
            itemIcon.sprite = icon;
            UpdateQuantity(quantity);
        }
        
        public void UpdateQuantity(int quantity)
        {
            _quantity = quantity;
            if (quantity > 1)
            {
                quantityText.text = quantity.ToString();
                quantityContainer?.gameObject.SetActive(true);
            }
            else
            {
                quantityText.text = string.Empty;
                quantityContainer?.gameObject.SetActive(false);
            }
        }
    }
}