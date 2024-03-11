using System.Collections.Generic;
using _Scripts.Gameplay.CharactersController.Player;
using _Scripts.Scriptables;
using UnityEngine;

namespace _Scripts.Managers
{
    /**
     * <summary>
     * Handler of the inventory.
     * </summary>
     */
    public class InventoryManager : MonoBehaviour
    {
        #region Variables

        [Header("Inventory Item Properties")] 
        [SerializeField] private Inventory inventoryScriptable;
        
        // Components.
        private UIManager _uiManager;
        private PlayerInputs _playerInputs;
        
        // Singleton.
        private static InventoryManager _instance;

        #endregion

        #region Properties

        // Scriptable Objects.
        public Inventory InventoryScriptable => inventoryScriptable;
        
        // Singleton Property.
        public static InventoryManager Instance => _instance;

        #endregion

        #region Built-In Methods

        /**
         * <summary>
         * Unity calls Awake when an enabled script instance is being loaded.
         * </summary>
         */
        void Awake()
        {
            if(_instance) Destroy(this);
            _instance = this;
        }


        /**
         * <summary>
         * Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.
         * </summary>
         */
        void Start()
        {
            // Components
            _uiManager = UIManager.Instance;
            _playerInputs = PlayerInputs.Instance;
            
            // Events.
            _uiManager.OnStartDragging += HandleDragging;
            _uiManager.OnSwapItems += HandleSwapItems;

            // Inventory Update Event.
            inventoryScriptable.OnInventoryUpdated += UpdateInventoryUI;
        }

        
        /**
         * <summary>
         * Update is called every frame, if the MonoBehaviour is enabled.
         * </summary>
         */
        void Update()
        {
            if (_playerInputs.Inventory)
            {
                if (!_uiManager.InventoryShowed)
                {
                    _uiManager.ManageInventory();
                    foreach (var inventoryItem in inventoryScriptable.GetCurrentInventoryState())
                    {   // Apply inventory content on UI.
                        _uiManager.UpdateInventorySlotUI(
                            inventoryItem.Key, 
                            inventoryItem.Value.item, 
                            inventoryItem.Value.quantity
                            );
                    }
                }
                else
                    _uiManager.ManageInventory();
            }
        }

        #endregion

        #region UI Integration Methods

        /**
         * <summary>
         * Update the Inventory UI.
         * </summary>
         * <param name="inventoryState">Get the inventory state from the inventory data.</param>
         */
        private void UpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
        {
            _uiManager.ResetAllItems();     // Reset the inventory.
            
            foreach (var item in inventoryState)
            {   // Apply Inventory actual content.
                _uiManager.UpdateInventorySlotUI(item.Key, item.Value.item, item.Value.quantity);
            }
        }

        #endregion

        #region Events Methods
        
        /**
         * <summary>
         * Handle the dragging item.
         * </summary>
         * <param name="itemInventorySlotIndex">The actual item ID.</param>
         */
        private void HandleDragging(int itemInventorySlotIndex)
        {
            InventoryItem inventoryItem = inventoryScriptable.GetItemAt(itemInventorySlotIndex);
            if (inventoryItem.IsEmpty) return;
            
            _uiManager.CreateDraggedItem(inventoryItem.item, inventoryItem.quantity);
        }

        
        /**
         * <summary>
         * Swap the two items between the one dragged and the slot hovered.
         * </summary>
         * <param name="itemInventorySlotIndex1">The item dragged.</param>
         * <param name="itemInventorySlotIndex2">The item hovered.</param>
         */
        private void HandleSwapItems(int itemInventorySlotIndex1, int itemInventorySlotIndex2)
        {
            inventoryScriptable.SwapItems(itemInventorySlotIndex1, itemInventorySlotIndex2);
        }

        #endregion
    }
}
