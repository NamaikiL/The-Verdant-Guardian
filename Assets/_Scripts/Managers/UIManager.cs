using System;
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
        // Mouse Controller
        [SerializeField] private MouseFollower mouseFollower;

        // Inventory Variables.
        private int _currentlyDraggedItemIndex = -1;
        private bool _inventoryShowed;
        
        // Inventory Events.
        public event Action<int> OnItemActionRequested, OnStartDragging; 
        public event Action<int, int> OnSwapItems;
        
        // Components.
        private InventoryManager _inventoryManager;
        
        // Singleton.
        private static UIManager _instance;

        #endregion

        #region Properties

        // Inventory Management.
        public bool InventoryShowed => _inventoryShowed;
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
        
        
        void OnEnable()
        {
            foreach (InventorySlotUI inventorySlotUI in inventorySlots)
            {
                inventorySlotUI.OnItemBeginDrag += HandleBeginDrag;
                inventorySlotUI.OnItemDroppedOn += HandleSwap;
                inventorySlotUI.OnItemEndDrag += HandleEndDrag;
                inventorySlotUI.OnItemRightClicked += HandleShowItemsActions;
            }
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

        #region Inventory Item Management

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
        public void UpdateInventorySlotUI(int id, Items itemScriptable, int quantity)
        {
            if (inventorySlots.Count > id)
            {
                inventorySlots[id].UpdateSlot(itemScriptable, quantity);
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
            
        } 
        
        
        public void ResetAllItems()
        {
            foreach (InventorySlotUI inventorySlot in inventorySlots)
            {
                inventorySlot.ClearSlot();
            }
        }
        
        
        // EVENTS
        
        
        private void HandleShowItemsActions(InventorySlotUI obj)
        {
            
        }

        
        private void HandleEndDrag(InventorySlotUI obj)
        {
            ResetDraggedItem();
        }

        
        private void HandleSwap(InventorySlotUI obj)
        {
            int index = inventorySlots.IndexOf(obj);
            if (index == -1)
            {
                return;
            }

            OnSwapItems ?.Invoke(_currentlyDraggedItemIndex, index);
        }

        
        private void HandleBeginDrag(InventorySlotUI obj)
        {
            int index = inventorySlots.IndexOf(obj);
            if (index == -1) return;
            _currentlyDraggedItemIndex = index;
            OnStartDragging?.Invoke(index);
        }


        public void CreateDraggedItem(Items item, int quantity)
        {
            mouseFollower.Toggle(true);
            mouseFollower.UpdateMouseFollowerSlot(item, quantity);
        }


        private void ResetDraggedItem()
        {
            mouseFollower.Toggle(false);
            _currentlyDraggedItemIndex = -1;
        }

        #endregion

        #region Inventory Stats Management

        public void BtnStatsIncrement()
        {
            
        }


        public void BtnStatsDecrement()
        {
            
        }

        #endregion
    }
}
