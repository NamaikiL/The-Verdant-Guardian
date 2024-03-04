using System;
using System.Collections.Generic;
using _Scripts.Gameplay;
using _Scripts.Scriptables;
using _Scripts.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Screen = UnityEngine.Device.Screen;

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
        
        [Header("Quest UI")]
        [SerializeField] private Transform panQuestHolder;
        [SerializeField] private GameObject panQuest;
        
        [Header("Inventory")]
        // Panels.
        [SerializeField] private GameObject panInventory;
        // Items UI Assets.
        [SerializeField] private List<InventorySlotUI> inventorySlots;
        // Mouse Controller.
        [SerializeField] private MouseFollower mouseFollower;
        // ToolTip Controller.
        [SerializeField] private ItemTooltip itemTooltip;
        [SerializeField] private ItemActionTooltip itemActionTooltip;

        [Header("Inventory Stats & Skills")] 
        // Player Stats.
        [SerializeField] private TMP_Text txtPlayerHpUI;
        [SerializeField] private TMP_Text txtPlayerEnduranceUI;
        // Player Level.
        [SerializeField] private TMP_Text txtPlayerLevelUI;
        [SerializeField] private TMP_Text txtPlayerExpUI;
        [SerializeField] private TMP_Text txtPlayerSkillPoints;
        // Player Skill Points.
        [SerializeField] private TMP_Text txtConsPoints;
        [SerializeField] private TMP_Text txtStrPoints;
        [SerializeField] private TMP_Text txtVigPoints;
        [SerializeField] private TMP_Text txtDexPoints;
        [SerializeField] private TMP_Text txtLuckPoints;

        // Inventory Variables.
        private int _currentlyDraggedItemIndex = -1;
        private bool _inventoryShowed;
        private RectTransform _itemTooltipTransform;
        private RectTransform _backgroundTooltipTransform;
        
        
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

            _itemTooltipTransform = itemTooltip.GetComponent<RectTransform>();
            _backgroundTooltipTransform = itemTooltip.transform.GetChild(0).GetComponent<RectTransform>();
        }
        
        
        void OnEnable()
        {
            foreach (InventorySlotUI inventorySlotUI in inventorySlots)
            {
                inventorySlotUI.OnItemBeginDrag += HandleBeginDrag;
                inventorySlotUI.OnItemDroppedOn += HandleSwap;
                inventorySlotUI.OnItemEndDrag += HandleEndDrag;
                inventorySlotUI.OnItemRightClicked += HandleShowItemsActions;
                inventorySlotUI.OnItemHoverBegin += HandleHoverItemBegin;
                inventorySlotUI.OnItemHoverEnd += HandleHoverItemEnd;
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


        void Update()
        {
            if (itemTooltip.isActiveAndEnabled)
            {
                Vector2 mousePosition = Input.mousePosition;    // Getting the mouse position.

                // Calculation of the offsets based on the RectTransform of the tooltip.
                float pivotOffsetX = _backgroundTooltipTransform.rect.width * _backgroundTooltipTransform.pivot.x;
                float pivotOffsetY =
                    _backgroundTooltipTransform.rect.height * (1f - _backgroundTooltipTransform.pivot.y);

                // Blocking the Tooltip on screen by Calculating the anchored position
                // by adjusting the position of the mouse with the pivot offsets.
                Vector2 anchoredPosition = new Vector2(
                    Mathf.Clamp(
                        mousePosition.x - pivotOffsetX, 
                        0,
                        Screen.width - _backgroundTooltipTransform.rect.width
                        ),
                    Mathf.Clamp(
                        mousePosition.y + pivotOffsetY, 
                        _backgroundTooltipTransform.rect.height, 
                        Screen.height
                        )
                );

                // Apply the position wanted.
                _itemTooltipTransform.anchoredPosition =
                    anchoredPosition - new Vector2(Screen.width / 2f, Screen.height / 2f);
            }
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
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                panInventory.SetActive(false);
                CloseTooltip();
                CloseActionTooltip();
                Cursor.lockState = CursorLockMode.Locked;
            }

            _inventoryShowed = !_inventoryShowed;
        }

        
        /**
         * <summary>
         * Update the information of the Item on its Slot
         * </summary>
         * <param name="itemId">The current instance ID of the Item.</param>
         * <param name="itemSO">The ItemSO.</param>
         * <param name="itemQuantity">The quantity of the item.</param>
         */
        public void UpdateInventorySlotUI(int itemId, Items itemSO, int itemQuantity)
        {
            if (inventorySlots.Count > itemId)
            {
                inventorySlots[itemId].UpdateSlot(itemSO, itemQuantity);
            }
        }
        
        
        public void ResetAllItems()
        {
            foreach (InventorySlotUI inventorySlot in inventorySlots)
            {
                inventorySlot.ClearSlot();
            }
        }

        private void CloseTooltip()
        {
            itemTooltip.ResetToolTip();
            itemTooltip.gameObject.SetActive(false);
        }

        private void CloseActionTooltip()
        {
            itemActionTooltip.gameObject.SetActive(false);
        }
        
        // EVENTS
        
        
        private void HandleShowItemsActions(ItemEventData eventData)
        {
            int index = inventorySlots.IndexOf(eventData.InventorySlotUI);
            itemActionTooltip.gameObject.SetActive(true);
            itemActionTooltip.InitializeButtons(index, eventData.InventorySlotUI.CurrentItem);
            itemActionTooltip.transform.position = eventData.PointerData.position;
        }

        
        private void HandleEndDrag(InventorySlotUI inventorySlotUI)
        {
            ResetDraggedItem();
        }

        
        private void HandleSwap(InventorySlotUI inventorySlotUI)
        {
            int index = inventorySlots.IndexOf(inventorySlotUI);
            if (index == -1)
            {
                return;
            }

            OnSwapItems ?.Invoke(_currentlyDraggedItemIndex, index);
        }

        
        private void HandleBeginDrag(InventorySlotUI inventorySlotUI)
        {
            int index = inventorySlots.IndexOf(inventorySlotUI);
            if (index == -1) return;
            _currentlyDraggedItemIndex = index;
            OnStartDragging?.Invoke(index);
        }


        public void CreateDraggedItem(Items itemSO, int itemQuantity)
        {
            mouseFollower.Toggle(true);
            mouseFollower.UpdateMouseFollowerSlot(itemSO, itemQuantity);
        }


        private void ResetDraggedItem()
        {
            mouseFollower.Toggle(false);
            _currentlyDraggedItemIndex = -1;
        }
        
        private void HandleHoverItemBegin(ItemEventData eventData)
        {
            itemTooltip.gameObject.SetActive(true);
            itemTooltip.transform.position = eventData.PointerData.position;
            itemTooltip.InitializeTooltip(eventData.InventorySlotUI.CurrentItem);
        }
        
        private void HandleHoverItemEnd(InventorySlotUI inventorySlotUI)
        {
            CloseTooltip();
        }
        
        #endregion

        #region Inventory Stats Management

        public void UpdatePlayerStats(int playerHp, int maxPlayerHp, float playerStamina, float maxPlayerStamina)
        {
            txtPlayerHpUI.text = $"Health {playerHp}/{maxPlayerHp}";
            txtPlayerEnduranceUI.text = $"Stamina {playerStamina}/{maxPlayerStamina}";
        }


        public void UpdatePlayerSkillPoints(int skillPoints, int consPoint, int strPoint, int vigPoint, int dexPoint, int luckPoint)
        {
            txtPlayerSkillPoints.text = $"Skill points: {skillPoints}";
            
            txtConsPoints.text = consPoint.ToString();
            txtStrPoints.text = strPoint.ToString();
            txtVigPoints.text = vigPoint.ToString();
            txtDexPoints.text = dexPoint.ToString();
            txtLuckPoints.text = luckPoint.ToString();
        }
        
        
        public void UpdatePlayerLevelAndExperienceUI(int playerLevel, int playerExperience, int experienceRequired, int playerSkillPoints)
        {
            txtPlayerLevelUI.text = $"Level {playerLevel}";
            txtPlayerExpUI.text = $"{playerExperience}/{experienceRequired}";
            txtPlayerSkillPoints.text = $"Skill points: {playerSkillPoints}";
        }
        
        
        public void BtnStatsIncrement(String skill)
        {
            if (GameObject.FindWithTag("Player").TryGetComponent(out PlayerStats playerStats))
            {
                playerStats.UpgradeSkill(skill, SkillUpdate.Increment);
            }
        }


        public void BtnStatsDecrement(String skill)
        {
            if (GameObject.FindWithTag("Player").TryGetComponent(out PlayerStats playerStats))
            {
                playerStats.UpgradeSkill(skill, SkillUpdate.Decrement);
            }
        }

        #endregion
    }
}
