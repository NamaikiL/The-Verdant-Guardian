using _Scripts.Managers;
using _Scripts.Scriptables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
    public class ItemActionTooltip : MonoBehaviour
    {
        #region Variables

        [Header("Tooltip Parameters")] 
        [SerializeField] private GameObject panelButtons;
        [SerializeField] private GameObject btnExample;

        [Header("Equipement Slots")] 
        [SerializeField] private EquipableSlotUI weaponSlotUI;
        
        // Data.
        private int _currentInventorySlotIndex;
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
        
        #region Initialize Methods

        public void InitializeButtons(int indexInventorySlot, Items itemSO)
        {
            _currentInventorySlotIndex = indexInventorySlot;
            _currentItem = itemSO;
            
            foreach (Transform childObject in panelButtons.transform)
            {
                Destroy(childObject.gameObject);
            }
            
            InitializeDropButton();

            if (itemSO is Weapons weaponSO
                /*|| itemSO is Armors armorSO*/)
            {
                InitializeEquipButton();
            }
        }

        private void InitializeDropButton()
        {
            GameObject btnDrop = Instantiate(btnExample, panelButtons.transform);
            btnDrop.transform.GetChild(0).GetComponent<TMP_Text>().text = "Drop";
            btnDrop.GetComponent<Button>().onClick.AddListener(() => DropItems());
        }

        private void InitializeEquipButton()
        {
            GameObject btnEquip = Instantiate(btnExample, panelButtons.transform);
            btnEquip.transform.GetChild(0).GetComponent<TMP_Text>().text = "Equip";
            btnEquip.GetComponent<Button>().onClick.AddListener(() => EquipItem());
        }

        #endregion

        #region Listening Methods

        private void DropItems()
        {
            _inventoryManager.InventoryScriptable.RemoveItemFromInventory(_currentInventorySlotIndex);
            gameObject.SetActive(false);
        }

        private void EquipItem()
        {
            _inventoryManager.InventoryScriptable.RemoveItemFromInventory(_currentInventorySlotIndex);
            weaponSlotUI.EquipItem(_currentItem);
            gameObject.SetActive(false);
        }

        #endregion

    }
}
