using _Scripts.Managers;
using _Scripts.Scriptables;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
    public class EquipableSlotUI : MonoBehaviour
    {
        #region Variables

        // Item.
        private Items _currentItem;
        
        // Components.
        private InventoryManager _inventoryManager;

        #endregion

        #region Built-In Methods

        void Start()
        {
            _inventoryManager = InventoryManager.Instance;
        }

        #endregion

        #region Equip Methods

        public void EquipItem(Items itemSO)
        {
            if (_currentItem)
                _inventoryManager.InventoryScriptable.AddItem(_currentItem, 1);
            _currentItem = itemSO;
            UpdateUISlot();
        }

        private void UpdateUISlot()
        {
            transform.GetChild(0).GetComponent<Image>().sprite = _currentItem.ItemImage;
        }

        #endregion
    }
}
