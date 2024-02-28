using System.Collections;
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
        [SerializeField] private Slider lifeSlider;
        [SerializeField] private Slider damageSlider;

        [Header("Floating life gauge position")]
        [SerializeField] private Camera cam;
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 gaugePosition;

        private float timeBeforeUpdateUI = 0.3f;
        #endregion

        #region Built-In Methods

        /**
         * <summary>
         * Update is called once per frame.
         * </summary>
         */
        void Update()
        {
            LifeBarPosition();
        }

        #endregion

        #region Custom Methods

        /**
         * <summary>
         * Update the size of the gauge based on the maximum life of the character.
         * </summary>
         * <param name="maxLife">The maximum life disponible. </param>
         */
        public void SetLifeBarMax(int maxLife)
        {
            lifeSlider.maxValue = maxLife;
            damageSlider.maxValue = maxLife;
            UpdateLifeBar(maxLife);
        }

        /**
         * <summary>
         * Update life bar.
         * </summary>
         * <param name="currentLife">The actual life value. </param>
         */
        public void UpdateLifeBar(int currentLife)
        {
            lifeSlider.value = currentLife;
            StartCoroutine(EffectLifeDamage(currentLife));
        }

        /**
         * <summary>
         * Visual effect when a character loose life.
         * </summary>
         * <param name="currentDamage">The actual damage value. </param>
         */
        public IEnumerator EffectLifeDamage(int currentDamage)
        {
            yield return new WaitForSeconds(timeBeforeUpdateUI);
            damageSlider.value = currentDamage;
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