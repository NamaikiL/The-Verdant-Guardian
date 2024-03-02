using _Scripts.Scriptables;
using UnityEngine;

namespace _Scripts.UI
{
    public class MouseFollower : MonoBehaviour
    {
        #region Variables

        [Header("UI Properties")]
        [SerializeField] private Canvas canvas;
        [SerializeField] private InventorySlotUI inventorySlotUI;

        #endregion

        #region Built-In Methods
        
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

        public void UpdateMouseFollowerSlot(Items item, int quantity)
        {
            inventorySlotUI.UpdateSlot(item, quantity);
        }


        public void Toggle(bool val)
        {
            Debug.Log($"Item toggled {val}");
            gameObject.SetActive(val);
        }

        #endregion
    }
}
