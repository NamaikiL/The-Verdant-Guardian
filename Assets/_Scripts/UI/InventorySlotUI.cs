using System;
using _Scripts.Scriptables;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Scripts.UI
{
    [Serializable]
    public class ItemEventData
    {
        #region Variables

        private PointerEventData pointerData;
        private InventorySlotUI inventorySlotUI;

        #endregion

        #region Properties

        public PointerEventData PointerData => pointerData;
        public InventorySlotUI InventorySlotUI => inventorySlotUI;

        #endregion

        #region Constructor

        public ItemEventData(PointerEventData pointerData, InventorySlotUI inventorySlotUI)
        {
            this.pointerData = pointerData;
            this.inventorySlotUI = inventorySlotUI;
        }

        #endregion
    }
    
    public class InventorySlotUI : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDropHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        #region Variables

        [Header("Item Case Properties")] 
        [SerializeField] private Image itemIcon;
        [SerializeField] private TMP_Text itemText;

        private Items _currentItem;
        private int _currentQuantity;

        // Conditions.
        private bool _isEmpty = true;
        private IDragHandler _dragHandlerImplementation;

        // Events for the Inventory UI Interactions.
        public event Action<InventorySlotUI> OnItemDroppedOn, OnItemBeginDrag, OnItemEndDrag, OnItemHoverEnd;

        public delegate void ItemEvent(ItemEventData eventData);
        public event ItemEvent OnItemHoverBegin, OnItemRightClicked;

        #endregion

        #region Properties

        public Items CurrentItem => _currentItem;
        public int CurrentQuantity => _currentQuantity;
        
        #endregion

        #region Built-In Methods

        void Awake()
        {
            ClearSlot();
        }

        #endregion
        
        #region Custom Methods

        public void UpdateSlot(Items item, int quantity)
        {
            itemIcon.gameObject.SetActive(true);
            itemIcon.sprite = item.ItemImage;
            itemText.gameObject.SetActive(true);
            itemText.text = quantity.ToString();

            _currentQuantity = quantity;
            _currentItem = item;
            _isEmpty = false;
        }


        public void ClearSlot()
        {
            itemIcon.gameObject.SetActive(false);
            itemIcon.sprite = null;
            itemText.gameObject.SetActive(false);
            itemText.text = "";

            _isEmpty = true;
        }

        #endregion

        #region Events Handler

        // Item Inventory Management Events.
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_isEmpty) return;
            OnItemBeginDrag?.Invoke(this);
        }

        public void OnDrop(PointerEventData eventData)
        {
            OnItemDroppedOn?.Invoke(this);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            OnItemEndDrag?.Invoke(this);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right && !_isEmpty)
            {
                ItemEventData itemEventData = new ItemEventData(eventData, this);
                OnItemRightClicked?.Invoke(itemEventData);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            
        }
        
        // Item Information Events.

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_isEmpty) return;
            ItemEventData itemEventData = new ItemEventData(eventData, this);
            OnItemHoverBegin?.Invoke(itemEventData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnItemHoverEnd?.Invoke(this);
        }
        
        #endregion
    }
}
