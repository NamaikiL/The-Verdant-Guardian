using System;
using System.Collections.Generic;
using _Scripts.Gameplay;
using _Scripts.Scriptables;
using _Scripts.UI;
using TMPro;
using UnityEngine;
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

        [Header("Stamina gauge UI.")]
        [SerializeField] private Slider staminaSlider;

        [Header("Quest UI.")]
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
        public event Action<int> OnStartDragging; 
        public event Action<int, int> OnSwapItems;
        
        // Singleton.
        private static UIManager _instance;

        #endregion

        #region Properties

        // Inventory Management.
        public bool InventoryShowed => _inventoryShowed;
        
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
        
        
        /**
         * <summary>
         * This function is called when the object becomes enabled and active.
         * </summary>
         */
        void OnEnable()
        {
            foreach (InventorySlotUI inventorySlotUI in inventorySlots)
            {   // Initialize the events.
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
         * Update is called every frame, if the MonoBehaviour is enabled.
         * </summary>
         */
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

        #region Stamina Management

        /**
         * <summary>
         * Update the size of the gauge based on the maximum stamina.
         * </summary>
         * <param name="maxStamina">The maximum stamina disponible. </param>
         */
        public void SetStaminaBarMax(float maxStamina)
        {
            staminaSlider.maxValue = maxStamina;
            UpdateStaminaBar(maxStamina);
        }

        /**
         * <summary>
         * Update stamina bar.
         * </summary>
         * <param name="currentStamina">The actual stamina value. </param>
         */
        public void UpdateStaminaBar(float currentStamina)
        {
            staminaSlider.value = currentStamina;
        }

        #endregion

        #region Quest Management

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

        #region Inventory Item Methods

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
         * <param name="itemSO">The Item data.</param>
         * <param name="itemQuantity">The quantity of the item.</param>
         */
        public void UpdateInventorySlotUI(int itemId, Items itemSO, int itemQuantity)
        {
            if (inventorySlots.Count > itemId)
            {
                inventorySlots[itemId].UpdateSlot(itemSO, itemQuantity);
            }
        }
        
        
        /**
         * <summary>
         * Reset all the inventory slots UI content.
         * </summary>
         */
        public void ResetAllItems()
        {
            foreach (InventorySlotUI inventorySlot in inventorySlots)
            {
                inventorySlot.ClearSlot();
            }
        }

        
        /**
         * <summary>
         * Close the tooltip and reset its values.
         * </summary>
         */
        private void CloseTooltip()
        {
            itemTooltip.ResetToolTip();
            itemTooltip.gameObject.SetActive(false);
        }

        
        /**
         * <summary>
         * Close the action tooltip.
         * </summary>
         */
        private void CloseActionTooltip()
        {
            itemActionTooltip.gameObject.SetActive(false);
        }
        
        
        /**
         * <summary>
         * Create a dragged item.
         * </summary>
         * <param name="itemSO">The item data.</param>
         * <param name="itemQuantity">The quantity of the item.</param>
         */
        public void CreateDraggedItem(Items itemSO, int itemQuantity)
        {
            mouseFollower.Toggle(true);
            mouseFollower.UpdateMouseFollowerSlot(itemSO, itemQuantity);
        }


        /**
         * <summary>
         * Reset the dragged item.
         * </summary>
         */
        private void ResetDraggedItem()
        {
            mouseFollower.Toggle(false);
            _currentlyDraggedItemIndex = -1;
        }
        
        #endregion

        #region Inventory Item Event Methods

        // EVENTS
        
        /**
         * <summary>
         * Handle the items actions when the player right click on an Inventory Slot.
         * </summary>
         * <param name="eventData">Custom event class storing the inventorySlot and the PointerEventData.</param>
         */
        private void HandleShowItemsActions(ItemEventData eventData)
        {
            int indexInventorySlot = inventorySlots.IndexOf(eventData.InventorySlotUI); // Get the Inventory Slot ID.
            
            // Initialize the Action tooltip.
            itemActionTooltip.InitializeActionTooltip(indexInventorySlot, eventData.InventorySlotUI.CurrentItem);
            itemActionTooltip.transform.position = eventData.PointerData.position;
            itemActionTooltip.gameObject.SetActive(true);
        }

        
        /**
         * <summary>
         * When the player start dragging an object on its inventory.
         * </summary>
         * <param name="inventorySlotUI">The actual inventory slot.</param>
         */
        private void HandleBeginDrag(InventorySlotUI inventorySlotUI)
        {
            int indexInventorySlot = inventorySlots.IndexOf(inventorySlotUI);   // Storing the inventory slot.
            if (indexInventorySlot == -1) return;    // If there's no inventory slot.
            _currentlyDraggedItemIndex = indexInventorySlot;
            
            OnStartDragging?.Invoke(indexInventorySlot);    // Start Dragging if there's an inventory slot and an item.
        }
        
        
        /**
         * <summary>
         * When the player release the drag when the mouse isn't on another slot.
         * </summary>
         * <param name="inventorySlotUI">The actual inventory slot.</param>
         */
        private void HandleEndDrag(InventorySlotUI inventorySlotUI)
        {
            ResetDraggedItem();
        }

        
        /**
         * <summary>
         * Handle the swap movement.
         * </summary>
         * <param name="inventorySlotUI">The current inventory slot.</param>
         */
        private void HandleSwap(InventorySlotUI inventorySlotUI)
        {
            int index = inventorySlots.IndexOf(inventorySlotUI);
            if (index == -1) return;

            OnSwapItems ?.Invoke(_currentlyDraggedItemIndex, index);
        }
        
        
        /**
         * <summary>
         * Show the tooltip of the item when mouse over the slot.
         * </summary>
         * <param name="eventData">The custom event data.</param>
         */
        private void HandleHoverItemBegin(ItemEventData eventData)
        {
            itemTooltip.gameObject.SetActive(true);
            itemTooltip.transform.position = eventData.PointerData.position;
            itemTooltip.InitializeTooltip(eventData.InventorySlotUI.CurrentItem);
        }
        
        
        /**
         * <summary>
         * Hide the tooltip of the item when mouse goes outside its slot.
         * </summary>
         */
        private void HandleHoverItemEnd(InventorySlotUI inventorySlotUI)
        {
            CloseTooltip();
        }

        #endregion

        #region Inventory Stats Methods

        /**
         * <summary>
         * Update the player stats on the Inventory UI.
         * </summary>
         * <param name="playerHp">The current player hp.</param>
         * <param name="maxPlayerHp">The max Hp the player can have.</param>
         * <param name="playerStamina">The current player stamina.</param>
         * <param name="maxPlayerStamina">The max stamina the player can have.</param>
         */
        public void UpdatePlayerStats(int playerHp, int maxPlayerHp, float playerStamina, float maxPlayerStamina)
        {
            txtPlayerHpUI.text = $"Health {playerHp}/{maxPlayerHp}";
            txtPlayerEnduranceUI.text = $"Stamina {playerStamina}/{maxPlayerStamina}";
        }


        /**
         * <summary>
         * Update the player skills on the UI.
         * </summary>
         * <param name="skillPoints">The current number of skill point the player have.</param>
         * <param name="consPoint">The current number of constitution point the player have.</param>
         * <param name="strPoint">The current number of strength point the player have.</param>
         * <param name="vigPoint">The current number of vigor point the player have.</param>
         * <param name="dexPoint">The current number of dexterity point the player have.</param>
         * <param name="luckPoint">The current number of luck point the player have.</param>
         */
        public void UpdatePlayerSkillPoints(int skillPoints, int consPoint, int strPoint, int vigPoint, int dexPoint, int luckPoint)
        {
            txtPlayerSkillPoints.text = $"Skill points: {skillPoints}";
            
            txtConsPoints.text = consPoint.ToString();
            txtStrPoints.text = strPoint.ToString();
            txtVigPoints.text = vigPoint.ToString();
            txtDexPoints.text = dexPoint.ToString();
            txtLuckPoints.text = luckPoint.ToString();
        }
        
        
        /**
         * <summary>
         * Update the player level and experience on the UI.
         * </summary>
         * <param name="playerLevel">The current player level.</param>
         * <param name="playerExperience">The current player experience.</param>
         * <param name="experienceRequired">The experience required for the next level.</param>
         * <param name="playerSkillPoints">The current player skill points.</param>
         */
        public void UpdatePlayerLevelAndExperienceUI(int playerLevel, int playerExperience, int experienceRequired, int playerSkillPoints)
        {
            txtPlayerLevelUI.text = $"Level {playerLevel}";
            txtPlayerExpUI.text = $"{playerExperience}/{experienceRequired}";
            txtPlayerSkillPoints.text = $"Skill points: {playerSkillPoints}";
        }
        
        
        /**
         * <summary>
         * Button that increase the player skill.
         * </summary>
         * <param name="skill">The skill to upgrade.</param>
         */
        public void BtnStatsIncrement(String skill)
        {
            if (GameObject.FindWithTag("Player").TryGetComponent(out PlayerStats playerStats))
            {
                playerStats.UpgradeSkill(skill, SkillUpdate.Increment);
            }
        }


        /**
         * <summary>
         * Button that decrease the player skill.
         * </summary>
         * <param name="skill">The skill to upgrade.</param>
         */
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
