using System.Collections.Generic;
using _Scripts.Scriptables;
using _Scripts.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Managers
{
    
    /**
     * <summary>
     * Manager of the UI in general.
     * </summary>
     */
    public class UIManager : MonoBehaviour
    {
        #region Variables

        [Header("Quest UI.")]
        [SerializeField] private Transform panQuestHolder;
        [SerializeField] private GameObject panQuest;
        
        [Header("Inventory")]
        // Panels.
        [SerializeField] private GameObject panInventory;
        // Items UI Assets.
        [SerializeField] private List<InventorySlotUI> inventorySlots;

        // Inventory Variables.
        private bool _inventoryShowed;
        
        // Components.
        private InventoryManager _inventoryManager;
        
        // Singleton.
        private static UIManager _instance;

        #endregion

        #region Properties

        // Inventory Management.
        public List<InventorySlotUI> InventorySlot => inventorySlots;
        
        // Singleton Property.
        public static UIManager Instance => _instance;

        #endregion

        #region Built-In Methods

        /**
         * <summary>
         * Awake is called when an enabled script instance is being loaded.
         * </summary>
         */
        void Awake()
        {
            // Singleton.
            if (_instance) Destroy(this);
            _instance = this;
        }


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

        #region Quests

        /**
         * <summary>
         * Add quest to the quest holder.
         * </summary>
         * <param name="title">The title of the quest.</param>
         * <param name="description">The description of the quest.</param>
         */
        public void AddNewQuest(string title, string description)
        {
            GameObject quest = Instantiate(panQuest, panQuestHolder);
            quest.transform.GetChild(0).GetComponent<TMP_Text>().text = title;
            quest.transform.GetChild(1).GetComponent<TMP_Text>().text = description;
        }


        /**
         * <summary>
         * Remove the active quest from the quest holder.
         * </summary>
         */
        public void RemoveQuest()
        {
            Destroy(panQuestHolder.transform.GetChild(0).gameObject);
        }

        #endregion

        #region Inventory

        /**
         * <summary>
         * Manage the inventory.
         * </summary>
         */
        public void ManageInventory()
        {
            if (!_inventoryShowed)
            {
                panInventory.SetActive(true);
            }
            else
            {
                panInventory.SetActive(false);
            }

            _inventoryShowed = !_inventoryShowed;
        }

        
        /**
         * <summary>
         * Manage the information of the item.
         * </summary>
         * <param name="inventoryItems">The list of items.</param>
         */
        public void AddItemToUI(Items item, int quantity)
        {
            foreach (InventorySlotUI slot in inventorySlots)
            {
                if (slot.IsStackable(item))
                {
                    slot.CurrentQuantity += quantity;
                    slot.UpdateSlot(slot.CurrentItem, slot.CurrentQuantity);
                    return;
                }
            }
            
            foreach (InventorySlotUI slot in inventorySlots)
            {
                if (slot.CurrentItem == null)
                {
                    slot.UpdateSlot(item, quantity);
                    break;
                }
            }
        }
        
        
        /**
         * <summary>
         * Manage the information of the item.
         * </summary>
         * <param name="inventoryItems">The list of items.</param>
         */
        public void RemoveItemToUI(Items item, int quantity)
        {
            foreach (InventorySlotUI slot in inventorySlots)
            {
                if (slot.CurrentItem == item)
                {
                    if (slot.CurrentQuantity > quantity)
                    {
                        slot.CurrentQuantity -= quantity;
                        slot.UpdateSlot(slot.CurrentItem, slot.CurrentQuantity);
                        return;
                    }
                    else
                    {
                        quantity -= slot.CurrentQuantity;
                        slot.ClearSlot();

                        if (quantity == 0) return;
                    }
                }
            }
        }

        
        /**
         * <summary>
         * Drop the item from the player inventory.
         * </summary>
         * <param name="item">The actual item.</param>
         */
        private void DropItem(Items item, int quantity)
        {
            _inventoryManager.RemoveItemFromInventory(item, quantity);
        } 

        #endregion

    }
}
