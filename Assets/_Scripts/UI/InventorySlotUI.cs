using System;
using _Scripts.Scriptables;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Scripts.UI
{
    /**
     * <summary>
     * The custom Event Data.
     * </summary>
     */
    [Serializable]
    public class ItemEventData
    {
        #region Variables

        // Data variables.
        private PointerEventData _pointerData;
        private InventorySlotUI _inventorySlotUI;

        #endregion

        #region Properties

        public PointerEventData PointerData => _pointerData;
        public InventorySlotUI InventorySlotUI => _inventorySlotUI;

        #endregion

        #region Constructor Methods

        /**
         * <summary>
         * The custom event constructor.
         * </summary>
         */
        public ItemEventData(PointerEventData pointerData, InventorySlotUI inventorySlotUI)
        {
            _pointerData = pointerData;
            _inventorySlotUI = inventorySlotUI;
        }

        #endregion
    }
    
    /**
     * <summary>
     * The inventory slot on the UI.
     * </summary>
     */
    public class InventorySlotUI : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDropHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        #region Variables

        [Header("Item Case Properties")] 
        [SerializeField] private Image itemIcon;
        [SerializeField] private TMP_Text itemText;

        // Storing the data.
        private Items _currentItem;

        // Conditions.
        private bool _isEmpty = true;
        private IDragHandler _dragHandlerImplementation;

        // Events for the Inventory UI Interactions.
        public event Action<InventorySlotUI> OnItemDroppedOn, OnItemBeginDrag, OnItemEndDrag, OnItemHoverEnd;
        public delegate void ItemEvent(ItemEventData eventData);
        public event ItemEvent OnItemHoverBegin, OnItemRightClicked;

        #endregion

        #region Properties

        // Data Properties.
        public Items CurrentItem => _currentItem;
        
        #endregion

        #region Built-In Methods

        /**
         * <summary>
         * Unity calls Awake when an enabled script instance is being loaded.
         * </summary>
         */
        void Awake()
        {
            ClearSlot();
        }

        #endregion
        
        #region UI Interaction Methods

        /**
         * <summary>
         * Update the UI display when called.
         * </summary>
         * <param name="itemSO">The item data.</param>
         * <param name="itemQuantity">The item quantity.</param>
         */
        public void UpdateSlot(Items itemSO, int itemQuantity)
        {
            // Image and Text.
            itemIcon.gameObject.SetActive(true);
            itemIcon.sprite = itemSO.ItemImage;
            itemText.gameObject.SetActive(true);
            itemText.text = itemQuantity.ToString();

            // Storing and conditions.
            _currentItem = itemSO;
            _isEmpty = false;
        }


        /**
         * <summary>
         * Clear the slot on the UI when no item.
         * </summary>
         */
        public void ClearSlot()
        {
            // Image and Text.
            itemIcon.gameObject.SetActive(false);
            itemIcon.sprite = null;
            itemText.gameObject.SetActive(false);
            itemText.text = "";

            // Conditions.
            _isEmpty = true;
        }

        #endregion

        #region Events Handler Methods

        // Item Inventory Management Events.
        
        /**
         * <summary>
         * Called by a BaseInputModule before a drag is started.
         * </summary>
         * <param name="eventData">Current event data.</param>
         */
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_isEmpty) return;
            OnItemBeginDrag?.Invoke(this);
        }

        
        /**
         * <summary>
         * Called by the EventSystem once dragging ends.
         * </summary>
         * <param name="eventData">Current event data.</param>
         */
        public void OnEndDrag(PointerEventData eventData)
        {
            OnItemEndDrag?.Invoke(this);
        }
        
        
        /**
         * <summary>
         * Called by the EventSystem every time the pointer is moved during dragging.
         * </summary>
         * <param name="eventData">Current event data.</param>
         */
        public void OnDrag(PointerEventData eventData)
        {
            
        }
        
        
        /**
         * <summary>
         * Called by the EventSystem when an object accepts a drop.
         * </summary>
         * <param name="eventData">Current event data.</param>
         */
        public void OnDrop(PointerEventData eventData)
        {
            OnItemDroppedOn?.Invoke(this);
        }
        

        /**
         * <summary>
         * Registered IPointerClickHandler callback.
         * </summary>
         * <param name="eventData">Data passed in (Typically by the event system).</param>
         */
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right && !_isEmpty)
            {   // If the input received is a right click.
                ItemEventData itemEventData = new ItemEventData(eventData, this);   // Creating a custom event data.
                OnItemRightClicked?.Invoke(itemEventData);
            }
        }
        
        // Item Information Events.

        /**
         * <summary>
         * Evaluate current state and transition to appropriate state.
         * </summary>
         * <param name="eventData">The EventData usually sent by the EventSystem.</param>
         */
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_isEmpty) return;
            ItemEventData itemEventData = new ItemEventData(eventData, this);       // Custom event data.
            OnItemHoverBegin?.Invoke(itemEventData);
        }

        
        /**
         * <summary>
         * Evaluate current state and transition to normal state.
         * </summary>
         * <param name="eventData">The EventData usually sent by the EventSystem.</param>
         */
        public void OnPointerExit(PointerEventData eventData)
        {
            OnItemHoverEnd?.Invoke(this);
        }
        
        #endregion
    }
}
