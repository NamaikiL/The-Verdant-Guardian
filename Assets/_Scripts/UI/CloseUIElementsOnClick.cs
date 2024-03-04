using UnityEngine;
using UnityEngine.EventSystems;

namespace _Scripts.UI
{
    public class CloseUIElementsOnClick : MonoBehaviour, IPointerClickHandler
    {
        #region Variables

        [Header("Elements to Close")] 
        [SerializeField] private ItemActionTooltip itemActionTooltip;

        #endregion

        #region Event

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("Test");
            if (!RectTransformUtility.RectangleContainsScreenPoint(
                    itemActionTooltip.transform.GetChild(0).GetComponent<RectTransform>(),
                    eventData.position,
                    eventData.pressEventCamera
                )
                && eventData.button == PointerEventData.InputButton.Left)
            {
                Debug.Log("Test");
                itemActionTooltip.gameObject.SetActive(false);
            }
        }

        #endregion
    }
}
