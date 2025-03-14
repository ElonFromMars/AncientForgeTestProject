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
            quantityText.text = quantity > 1 ? quantity.ToString() : string.Empty;
        }
    }
}