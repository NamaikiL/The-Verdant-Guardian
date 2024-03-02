using System.Collections.Generic;
using _Scripts.Gameplay;
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
            _uiManager = UIManager.Instance;

            _uiManager.OnSwapItems += HandleSwapItems;
            _uiManager.OnStartDragging += HandleDragging;
            _uiManager.OnItemActionRequested += HandleItemActionRequest;
            
            //inventoryScriptable.InitializeInventory();

            inventoryScriptable.OnInventoryUpdated += UpdateInventoryUI;
        }

        private void UpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
        {
            _uiManager.ResetAllItems();
            foreach (var item in inventoryState)
            {
                _uiManager.UpdateInventorySlotUI(item.Key, item.Value.item, item.Value.quantity);
            }
        }


        void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                if (!_uiManager.InventoryShowed)
                {
                    _uiManager.ManageInventory();
                    foreach (var inventoryItem in inventoryScriptable.GetCurrentInventoryState())
                    {
                        _uiManager.UpdateInventorySlotUI(
                            inventoryItem.Key, 
                            inventoryItem.Value.item, 
                            inventoryItem.Value.quantity
                            );
                    }
                }
                else
                {
                    _uiManager.ManageInventory();
                }
            }
        }

        #endregion

        #region Events

        private void HandleItemActionRequest(int itemIndex)
        {
            
        }

        
        private void HandleDragging(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryScriptable.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty) return;
            _uiManager.CreateDraggedItem(inventoryItem.item, inventoryItem.quantity);
        }

        
        private void HandleSwapItems(int itemIndex1, int itemIndex2)
        {
            inventoryScriptable.SwapItems(itemIndex1, itemIndex2);
        }

        #endregion
    }
}
