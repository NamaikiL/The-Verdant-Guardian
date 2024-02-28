using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
    /**
     * <summary>
     * Manage the life bar on every character who has life.
     * </summary>
     */

    public class LifeBar : MonoBehaviour
    {
        #region Variables

        [Header("Life gauge UI")]
        [SerializeField] private Slider lifeGauge;

        [Header("Floating life gauge position")]
        [SerializeField] private Camera cam;
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 gaugePosition;

        #endregion

        #region Built-In Methods

        /**
         * <summary>
         * Update is called once per frame.
         * </summary>
         */
        private void Update()
        {
            LifeBarPosition();
        }

        #endregion

        #region Custom Methods

        /**
         * <summary>
         * Update life bar.
         * </summary>
         * <param name="currentLife">The actual life value. </param>
         * <param name="maxLife">The maximum life disponible. </param>
         */
        public void UpdateLifeBar(float currentLife, float maxLife)
        {
            lifeGauge.value = currentLife / maxLife;
        }

        /**
         * <summary>
         * Set up the rotation and position of floating life bar depending on the character and the camera.
         * </summary>
         */
        private void LifeBarPosition()
        {
            if (cam)
            {
                transform.rotation = cam.transform.rotation;
            }

            if (target)
            {
                transform.position = target.position + gaugePosition;
            }
        }

        #endregion
    }
}