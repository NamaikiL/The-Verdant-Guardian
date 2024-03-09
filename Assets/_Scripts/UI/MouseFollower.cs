using _Scripts.Scriptables;
using UnityEngine;

namespace _Scripts.UI
{
    /**
     * <summary>
     * The drag handler.
     * </summary>
     */
    public class MouseFollower : MonoBehaviour
    {
        #region Variables

        [Header("UI Properties")]
        [SerializeField] private Canvas canvas;
        [SerializeField] private InventorySlotUI inventorySlotUI;

        #endregion

        #region Built-In Methods
        
        /**
         * <summary>
         * Update is called every frame.
         * </summary>
         */
        public void Update()
        {
            Vector2 mousePosition;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                (RectTransform)canvas.transform,
                Input.mousePosition,
                canvas.worldCamera,
                out mousePosition
                );

            transform.position = canvas.transform.TransformPoint(mousePosition);
        }

        #endregion

        #region Custom Methods

        /**
         * <summary>
         * Update the mouse follower Inventory Slot.
         * </summary>
         * <param name="itemSO">The item data.</param>
         * <param name="itemQuantity">The item quantity.</param>
         */
        public void UpdateMouseFollowerSlot(Items itemSO, int itemQuantity)
        {
            inventorySlotUI.UpdateSlot(itemSO, itemQuantity);
        }


        /**
         * <summary>
         * Activate the Mouse Follower based on the value given.
         * </summary>
         * <param name="isDragged">If the item is being dragged.</param>
         */
        public void Toggle(bool isDragged)
        {
            gameObject.SetActive(isDragged);
        }

        #endregion
    }
}
