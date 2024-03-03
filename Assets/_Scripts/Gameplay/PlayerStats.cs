using System.Collections;
using _Scripts.Managers;
using _Scripts.UI;
using UnityEngine;

namespace _Scripts.Gameplay
{
    public class PlayerStats : MonoBehaviour
    {
        #region Variables

        [Header("Scripts")]
        [SerializeField] private UIManager uiManager;
        [SerializeField] private HealthBar healthBar;

        [Header("Player Health")]
        [SerializeField] private float maxPlayerHP = 100f;
        [SerializeField] private float healthRegenRate = 5f;
        [SerializeField] private float healthRegenDelay = 5f;
        [SerializeField] private float durationHealVFX = 2f;
        [SerializeField] private GameObject healVFX;

        [Header("Player Stamina")]
        [SerializeField] private float maxPlayerStamina = 100f;
        [SerializeField] private float staminaRegenRate = 5f;
        [SerializeField] private float staminaRegenDelay = 5f;
        [SerializeField] private float sprintStaminaCost = 5f;
        [SerializeField] private float rollStaminaCost = 20f;
        
        // Player Stats.
        private float _currentPlayerHP;
        private float _currentPlayerStamina;
        
        // Stamina Conditions.
        private IEnumerator _regenStamina;

        #endregion

        #region Properties

        public float CurrentPlayerStamina => _currentPlayerStamina;
        public float RollStaminaCost => rollStaminaCost;

        #endregion

        #region Built-In Methods

        /**
         * <summary>
         * Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.
         * </summary>
         */
        void Start()
        {
            _currentPlayerHP = maxPlayerHP;
            _currentPlayerStamina = maxPlayerStamina;

            //Update the maximum size of gauges
            healthBar.SetHealthBarMax(maxPlayerHP);
            uiManager.SetStaminaBarMax(maxPlayerStamina);
        }

        /**
         * <summary>
         * Update is called once per frame.
         * </summary>
         */
        void Update()
        {
            _currentPlayerHP = Mathf.Clamp(_currentPlayerHP, 0f, maxPlayerHP);
        }

        #endregion

        #region Health Management

        /**
         * <summary>
         * Remove HP from the player based on a quantity given.
         * </summary>
         * <param name="damage">The number of damage the player took.</param>
         */
        public void TakeDamage(float damage)
        {
            _currentPlayerHP -= damage;
            healthBar.UpdateHealthBar(_currentPlayerHP);

            StartCoroutine(RegenerateHealth());
        }

        /**
         * <summary>
         * Regen the player's HP based on a quantity given.
         * </summary>
         * <param name="quantity">The quantity to regen.</param>
         */
        public void RegenHealth(float quantity)
        {
            _currentPlayerHP += quantity;
            healthBar.UpdateHealthBar(_currentPlayerHP);
            StartCoroutine(HealEffectDuration());
        }

        /**
         * <summary>
         * Regen all the player HP.
         * </summary>
         */
        public void RegenAllHealth()
        {
            _currentPlayerHP += maxPlayerHP;
            healthBar.UpdateHealthBar(_currentPlayerHP);
        }

        /**
         * <summary>
         * Active VFX when the player get healed for a certain duration.
         * </summary>
         */
        private IEnumerator HealEffectDuration()
        {
            healVFX.SetActive(true);
            yield return new WaitForSeconds(durationHealVFX);
            healVFX.SetActive(false);
        }

        /**
         * <summary>
         * Coroutine to regenerate the life after a decided time.
         * </summary>
         */
        private IEnumerator RegenerateHealth()
        {
            yield return new WaitForSeconds(healthRegenDelay);

            while (_currentPlayerHP < maxPlayerHP)
            {
                _currentPlayerHP = Mathf.Clamp(_currentPlayerHP + healthRegenRate * Time.deltaTime, 0f, maxPlayerHP);
                healthBar.UpdateHealthBar(_currentPlayerHP);
                yield return null;
            }
        }

        #endregion

        #region Stamina Management

        /**
         * <summary>
         * The stamina used when sprint.
         * </summary>
         */
        public void UseStaminaSprint()
        {
            if (_currentPlayerStamina > 0)
            {
                if (_regenStamina != null)
                {
                    StopCoroutine(_regenStamina);
                    _regenStamina = null;
                }
                _currentPlayerStamina = Mathf.Clamp(_currentPlayerStamina - sprintStaminaCost * Time.deltaTime, 0f, maxPlayerStamina);
                uiManager.UpdateStaminaBar(_currentPlayerStamina);
            }
            
            if (_currentPlayerStamina == 0)
            {
                GetComponent<PlayerInputs>().Sprint = false;
            }

            _regenStamina = RegenerateStamina();
            StartCoroutine(_regenStamina);
        }


        /**
         * <summary>
         * The stamina used when the player roll.
         * </summary>
         */
        public void UseStaminaRoll()
        {
            if (_regenStamina != null)
            {
                StopCoroutine(_regenStamina);
                _regenStamina = null;
            }
            
            _currentPlayerStamina = Mathf.Clamp(_currentPlayerStamina - rollStaminaCost, 0f, maxPlayerStamina);
            uiManager.UpdateStaminaBar(_currentPlayerStamina);
            _regenStamina = RegenerateStamina();
            StartCoroutine(_regenStamina);
        }
        
        
        /**
         * <summary>
         * Use the stamina with a precised amount.
         * </summary>
         * <param name="amount">The amount to remove.</param>
         */
        public void UseStamina(float amount)
        {
            if (_regenStamina != null)
            {
                StopCoroutine(_regenStamina);
                _regenStamina = null;
            }
            
            _currentPlayerStamina = Mathf.Clamp(_currentPlayerStamina - amount, 0f, maxPlayerStamina);
            uiManager.UpdateStaminaBar(_currentPlayerStamina);
            _regenStamina = RegenerateStamina();
            StartCoroutine(_regenStamina);
        }


        /**
         * <summary>
         * Coroutine to regenerate the stamina after a decided time.
         * </summary>
         */
        private IEnumerator RegenerateStamina()
        {
            yield return new WaitForSeconds(staminaRegenDelay);
            
            while (_currentPlayerStamina < maxPlayerStamina)
            {
                _currentPlayerStamina = Mathf.Clamp(_currentPlayerStamina + staminaRegenRate * Time.deltaTime, 0f, maxPlayerStamina);
                uiManager.UpdateStaminaBar(_currentPlayerStamina);
                yield return null;
            }
        }

        #endregion
    }
}
