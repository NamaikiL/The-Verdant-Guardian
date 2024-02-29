using System;
using System.Collections.Generic;
using _Scripts.Gameplay;
using _Scripts.Scriptables;
using UnityEngine;

namespace _Scripts.Managers
{

    [Serializable]
    public class InventoryItem
    {
        #region Variables

        [Header("Item")] 
        [SerializeField] private Items item;
        [SerializeField] private int itemQuantity;

        #endregion

        #region Properties

        public Items Item
        {
            get => item;
            set => item = value;
        }
        
        public int ItemQuantity
        {
            get => itemQuantity;
            set => itemQuantity = value;
        }

        #endregion

        #region Methods

        public InventoryItem(Items items, int quantity)
        {
            item = items;
            itemQuantity = quantity;
        }

        #endregion
    }
    
    /**
     * <summary>
     * Handler of the inventory.
     * </summary>
     */
    public class InventoryManager : MonoBehaviour
    {
        #region Variables

        [Header("Inventory Item Properties")] 
        [SerializeField] private int inventoryItemCapacity = 30;
        
        // Inventory Item Handler.
        private int _currentItemCount;
        private List<InventoryItem> _inventoryItems = new List<InventoryItem>();
        
        // Components.
        private UIManager _uiManager;
        
        // Singleton.
        private static InventoryManager _instance;

        #endregion

        #region Properties

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
        }

        #endregion

        #region Inventory Item Management

        /**
         * <summary>
         * Update the item inventory.
         * </summary>
         * <param name="item">The item.</param>
         * <param name="itemScriptable">The scriptable data of the item.</param>
         * <param name="itemType">The item type.</param>
         * <param name="playerController">The controller of the player.</param>
         */
        public void AddItemToInventory(Item item, Items itemScriptable, ItemType itemType, PlayerController playerController)
        {
            if (_inventoryItems.Count < inventoryItemCapacity)
            {   // If the item inventory capacity isn't full.
                if (item.IsQuestConnected)
                {   // If the item is a quest item.
                    foreach (Quest quest in playerController.PlayerQuestsList)
                    {   // If one of the quest the player have has the item required in its objectives.
                        foreach (Objectives objective in quest.QuestObjectives)
                        {   
                            if (!objective.IsComplete 
                                && objective.ActualObjectiveType == ObjectiveType.Collect 
                                && objective.ActualItemType == itemType)
                            {   // Check the objective condtions.
                                objective.NbCollected++;
                                
                                if (objective.NbCollected == objective.NbToCollect)
                                {   // When the number to collect is the one required.
                                    objective.CompleteObjective();
                                }

                                return;
                            }
                        }
                    }
                }
                
                // Inventory Management.
                _currentItemCount++;
                _inventoryItems.Add(new InventoryItem(item.ItemScriptable, 1));
                _uiManager.AddItemToUI(item.ItemScriptable, 1);
                
                // GameObject handler.
                Destroy(item.gameObject);
            }
        }


        /**
         * <summary>
         * Remove an item from the player inventory.
         * </summary>
         * <param name="item">The actual item.</param>
         */
        public void RemoveItemFromInventory(Items item, int quantity)
        {
            if (_inventoryItems[_inventoryItems.FindIndex(inventoryItem => inventoryItem.Item == item)].ItemQuantity >= quantity)
            {
                for (int i = 0; i <= _inventoryItems.Count; i++)
                {
                    if (_inventoryItems[i].Item == item)
                    {
                        _inventoryItems[i].ItemQuantity -= quantity;
                        
                        if (_inventoryItems[i].ItemQuantity <= 0)
                        {
                            _inventoryItems.RemoveAt(i);
                            _uiManager.RemoveItemToUI(item, quantity);
                        }
                        
                        _uiManager.RemoveItemToUI(item, quantity);

                        break;
                    }
                }
            }
            else
            {
                Debug.Log("No.");
            }
        }

        #endregion
    }
}
