using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
    /**
     * <summary>
     * Manager of the health bar in general.
     * </summary>
     */
    public class HealthBar : MonoBehaviour
    {
        #region Variables

        [Header("Life gauge UI.")]
        [SerializeField] private Slider currentHealthSlider;
        [SerializeField] private Slider damageSlider;
        [SerializeField] private float cooldownDamageEffect = 0.3f;

        [Header("Floating life gauge position")]
        [SerializeField] private Transform cam;
        [SerializeField] private Transform target;

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

        #region Health Management

        /**
         * <summary>
         * Update the size of the gauge based on the maximum life of the character.
         * </summary>
         * <param name="maxHealth">The maximum life disponible. </param>
         */
        public void SetHealthBarMax(float maxHealth)
        {
            currentHealthSlider.maxValue = maxHealth;
            damageSlider.maxValue = maxHealth;
            UpdateHealthBar(maxHealth);
        }

        /**
         * <summary>
         * Update life bar.
         * </summary>
         * <param name="currentHealth">The actual life value. </param>
         */
        public void UpdateHealthBar(float currentHealth)
        {
            currentHealthSlider.value = currentHealth;
            StartCoroutine(EffectHealthDamage(currentHealth));
        }

        /**
         * <summary>
         * Coroutine for the visual effect on the gauge when a character loose life.
         * </summary>
         * <param name="currentDamage">The actual damage value. </param>
         */
        private IEnumerator EffectHealthDamage(float currentDamage)
        {
            yield return new WaitForSeconds(cooldownDamageEffect);
            damageSlider.value = currentDamage;
        }

        #endregion

        #region Floating Life Bar

        /**
         * <summary>
         * Set up the rotation of floating life bar depending on the camera.
         * </summary>
         */
        private void LifeBarPosition()
        {   
            if (cam)
            {
                target.LookAt(cam);
            }
        }

        #endregion

    }
}