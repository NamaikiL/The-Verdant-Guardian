using _Scripts.Scriptables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
    public class InventorySlotUI : MonoBehaviour
    {
        #region Variables

        [Header("Item Case Properties")] 
        [SerializeField] private Image itemIcon;
        [SerializeField] private TMP_Text itemText;
        [SerializeField] private Items currentItem;
        [SerializeField] private int currentQuantity;

        #endregion

        #region Properties

        public int CurrentQuantity
        {
            get => currentQuantity;
            set => currentQuantity = value;
        }
        public Items CurrentItem => currentItem;

        #endregion

        #region Custom Methods

        public void UpdateSlot(Items item, int quantity)
        {
            currentItem = item;
            currentQuantity = quantity;
            itemIcon.gameObject.SetActive(true);
            itemIcon.sprite = item.ItemImage;
            itemText.gameObject.SetActive(true);
            itemText.text = quantity.ToString();
        }


        public void ClearSlot()
        {
            currentItem = null;
            currentQuantity = 0;
            itemIcon.gameObject.SetActive(false);
            itemIcon.sprite = null;
            itemText.gameObject.SetActive(false);
            itemText.text = "";
        }


        public bool IsStackable(Items item)
        {
            return currentItem != null && currentItem == item && currentQuantity < item.MaxStack;
        }

        #endregion
    }
}
