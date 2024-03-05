using _Scripts.Gameplay;
using _Scripts.Managers;
using _Scripts.Scriptables;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
    /**
     * <summary>
     * Handle the Equipments slots.
     * </summary>
     */
    public class EquipmentSlotUI : MonoBehaviour
    {
        #region Variables

        // Item.
        private Items _currentItem;
        
        // Components.
        private InventoryManager _inventoryManager;

        #endregion

        #region Built-In Methods

        /**
         * <summary>
         * Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.
         * </summary>
         */
        void Start()
        {
            _inventoryManager = InventoryManager.Instance;
        }

        #endregion

        #region Initialize Methods

        /**
         * <summary>
         * Initialize the equipped items.
         * </summary>
         * <param name="itemSO">The item data.</param>
         */
        public void InitializeEquippedItems(Items itemSO)
        {
            if (GameObject.FindGameObjectWithTag("Player").TryGetComponent(out PlayerStats playerStats))
            {
                playerStats.InitializePlayerEquipment(itemSO);
                EquipItem(itemSO);
            }
            
        }

        #endregion
        
        #region Equip Methods

        /**
         * <summary>
         * Equip the Item given.
         * </summary>
         * <param name="itemSO">The item data.</param>
         */
        public void EquipItem(Items itemSO)
        {
            if (_currentItem)
                _inventoryManager.InventoryScriptable.AddItem(_currentItem, 1);
            _currentItem = itemSO;
            UpdateUISlot();
        }

        
        /**
         * <summary>
         * Update the UI.
         * </summary>
         */
        private void UpdateUISlot()
        {
            transform.GetChild(0).GetComponent<Image>().sprite = _currentItem.ItemImage;
        }

        #endregion
    }
}
