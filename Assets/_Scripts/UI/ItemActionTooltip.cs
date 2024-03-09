using _Scripts.Gameplay;
using _Scripts.Managers;
using _Scripts.Scriptables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
    /**
     * <summary>
     * Tooltip for the actions on item.
     * </summary>
     */
    public class ItemActionTooltip : MonoBehaviour
    {
        #region Variables

        [Header("Tooltip Parameters")] 
        [SerializeField] private GameObject panelButtons;
        [SerializeField] private GameObject btnExample;

        [Header("Equipment Slots")] 
        [SerializeField] private EquipmentSlotUI weaponSlotUI;
        [SerializeField] private EquipmentSlotUI armorSlotUI;
        
        // Data.
        private int _currentInventorySlotIndex;
        private Items _currentItem;
        
        // Components.
        private InventoryManager _inventoryManager;

        #endregion

        #region Built-In Methods

        /**
         * <summary>
         * Unity calls Awake when an enabled script instance is being loaded.
         * </summary>
         */
        void Start()
        {
            // Components.
            _inventoryManager = InventoryManager.Instance;
        }

        #endregion
        
        #region Initialize Methods

        /**
         * <summary>
         * Initialize the Action Tooltip.
         * </summary>
         * <param name="indexInventorySlot">The inventory slot.</param>
         * <param name="itemSO">The item data.</param>
         */
        public void InitializeActionTooltip(int indexInventorySlot, Items itemSO)
        {
            // Storing the data.
            _currentInventorySlotIndex = indexInventorySlot;
            _currentItem = itemSO;
            
            // Destroy each child everytime the player active the action Tooltip
            foreach (Transform childObject in panelButtons.transform)
            {
                Destroy(childObject.gameObject);
            }
            
            // Initialize the buttons.
            InitializeDropButton();
            if(_currentItem is Weapons || _currentItem is Armors)
                InitializeEquipButton();
            if (_currentItem is Consumables)
                InitializeUseConsumableButton();
        }

        
        /**
         * <summary>
         * Initialize the Drop Button.
         * </summary>
         */
        private void InitializeDropButton()
        {
            GameObject btnDrop = Instantiate(btnExample, panelButtons.transform);
            btnDrop.transform.GetChild(0).GetComponent<TMP_Text>().text = "Drop";
            btnDrop.GetComponent<Button>().onClick.AddListener(DropItems);
        }

        
        /**
         * <summary>
         * Initialize the Equip button.
         * </summary>
         */
        private void InitializeEquipButton()
        {
            GameObject btnEquip = Instantiate(btnExample, panelButtons.transform);
            btnEquip.transform.GetChild(0).GetComponent<TMP_Text>().text = "Equip";
            btnEquip.GetComponent<Button>().onClick.AddListener(EquipItem);
        }


        /**
         * <summary>
         * Initialize the Use Consumable button.
         * </summary>
         */
        private void InitializeUseConsumableButton()
        {
            GameObject btnUseConsumable = Instantiate(btnExample, panelButtons.transform);
            btnUseConsumable.transform.GetChild(0).GetComponent<TMP_Text>().text = "Use Consumable";
            btnUseConsumable.GetComponent<Button>().onClick.AddListener(UseConsumable);
        }

        #endregion

        #region Listening Methods

        /**
         * <summary>
         * Drop the items on the current slot.
         * </summary>
         */
        private void DropItems()
        {
            // Inventory Management.
            _inventoryManager.InventoryScriptable.RemoveItemFromInventory(_currentInventorySlotIndex);
            
            gameObject.SetActive(false);    // Dis-activate the tooltip.
        }

        
        /**
         * <summary>
         * Equip the item on the slot.
         * </summary>
         */
        private void EquipItem()
        {
            // Inventory Management.
            _inventoryManager.InventoryScriptable.RemoveItemFromInventory(_currentInventorySlotIndex);
            if (_currentItem is Weapons)
                weaponSlotUI.EquipItem(_currentItem);
            else if (_currentItem is Armors)
                armorSlotUI.EquipItem(_currentItem);
            
            
            gameObject.SetActive(false);    // Dis-activate the tooltip.
        }


        /**
         * <summary>
         * Use Consumable of the item given.
         * </summary>
         */
        private void UseConsumable()
        {
            // Inventory Management.
            _inventoryManager.InventoryScriptable.RemoveItemWithAmount(_currentInventorySlotIndex, 1);
            if (_currentItem is Consumables consumablesSO
                && GameObject.FindWithTag("Player").TryGetComponent(out PlayerStats playerStats))
            {
                switch (consumablesSO.CurrentConsumableType)
                {
                    case ConsumableType.health:
                        playerStats.RegenHealth(consumablesSO.ConsumableRegen);
                        break;
                    case ConsumableType.stamina:
                        playerStats.RegenerateStaminaWithAmount(consumablesSO.ConsumableRegen);
                        break;
                }
            }

            gameObject.SetActive(false);
        }

        #endregion

    }
}
