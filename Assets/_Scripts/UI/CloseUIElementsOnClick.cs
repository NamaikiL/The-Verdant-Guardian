using UnityEngine;
using UnityEngine.EventSystems;

namespace _Scripts.UI
{
    /**
     * <summary>
     * Close elements given when the player click outside the element in question.
     * </summary>
     */
    public class CloseUIElementsOnClick : MonoBehaviour, IPointerClickHandler
    {
        #region Variables

        [Header("Elements to Close")] 
        [SerializeField] private ItemActionTooltip itemActionTooltip;

        #endregion

        #region Event Method

        /**
         * <summary>
         * Registered IPointerClickHandler callback.
         * </summary>
         * <param name="eventData">Data passed in (Typically by the event system).</param>
         */
        public void OnPointerClick(PointerEventData eventData)
        {
            if (!RectTransformUtility.RectangleContainsScreenPoint(
                    itemActionTooltip.transform.GetChild(0).GetComponent<RectTransform>(),
                    eventData.position,
                    eventData.pressEventCamera
                )
                && eventData.button == PointerEventData.InputButton.Left)
            {   // If the click is perform outside the Tooltip and is a left click.
                itemActionTooltip.gameObject.SetActive(false);  // Dis-activate the tooltip.
            }
        }

        #endregion
    }
}
