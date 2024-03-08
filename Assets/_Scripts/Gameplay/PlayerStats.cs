using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Managers;
using _Scripts.Scriptables;
using _Scripts.UI;
using UnityEngine;

namespace _Scripts.Gameplay
{
    
    /**
     * <summary>
     * Enum used to upgrade or degrade the skills.
     * </summary>
     */
    public enum SkillUpdate
    {
        Increment,
        Decrement
    }

    
    /**
     * <summary>
     * Handle the player level and experiences.
     * </summary>
     */
    [Serializable]
    public class PlayerLevel
    {
        #region Variables

        [Header("Player Levels Database")]
        [SerializeField] private int level;
        [SerializeField] private int expRequired;

        #endregion

        #region Properties

        public int Level => level;
        public int ExpRequired => expRequired;

        #endregion
    }
    
    
    /**
     * <summary>
     * Handle the players stats and skills.
     * </summary>
     */
    public class PlayerStats : MonoBehaviour
    {
        #region Variables

        [Header("Player Level")] 
        [SerializeField] private List<PlayerLevel> playerLevels;
        
        [Header("Player Health")]
        [SerializeField] private float maxPlayerHP = 100f;
        [SerializeField] private float healthRegenRate = 5f;
        [SerializeField] private float healthRegenDelay = 5f;
        [SerializeField] private float healthAlert = 15f;
        [SerializeField] private float durationHealVFX = 2f;
        [SerializeField] private GameObject healVFX;
        [SerializeField] private HealthBar healthBar;

        [Header("Player Stamina")]
        [SerializeField] private float maxPlayerStamina = 100f;
        [SerializeField] private float staminaRegenRate = 5f;
        [SerializeField] private float staminaRegenDelay = 5f;
        [SerializeField] private float sprintStaminaCost = 5f;
        [SerializeField] private float rollStaminaCost = 20f;

        [Header("Player Skills")] 
        [SerializeField] private float maxPlayerAttack = 10f;
        [SerializeField] private float maxPlayerCritAttack = 2.5f;
        [SerializeField] private float maxPlayerSpeed = 4f;
        [SerializeField] private float maxPlayerLootRate = 10f;
        
        // Player Level.
        private int _currentPlayerLevel = 1;
        private int _currentPlayerExp;
        
        // Player Stats.
        private float _currentPlayerHP;
        private float _currentPlayerStamina;
        private float _currentPlayerAttack;
        private float _currentPlayerCritAttack;
        private float _currentPlayerSpeed;
        private float _currentPlayerLootRate;
        
        // Player Skill Management.
        private int _skillPoints;
        private int _constitutionPoints;
        private int _strengthPoints;
        private int _vigorPoints;
        private int _dexterityPoints;
        private int _luckPoints;
        
        // Player Equipment Stats.
        private int _actualWeaponAtk;
        private float _actualWeaponAtkSpeed;
        private int _actualArmorDefense;
        
        // Stamina Conditions.
        private IEnumerator _regenStamina;

        // EVENT.
        public static event Action<PlayerStats> OnInteraction;

        // Components.
        private UIManager _uiManager;
        private AudioManager _audioManager;
        private PlayerInputs _playerInputs;
        private PlayerController _playerController;

        #endregion

        #region Properties

        // Player Level Properties.
        public int CurrentPlayerLevel => _currentPlayerLevel;
        public int CurrentPlayerExperience => _currentPlayerExp;
        
        // Player Skills Properties.
        public int CurrentPlayerSkillPoints => _skillPoints;
        public int CurrentPlayerConstitutionPoints => _constitutionPoints;
        public int CurrentPlayerStrengthPoints => _strengthPoints;
        public int CurrentPlayerVigorPoints => _vigorPoints;
        public int CurrentPlayerDexterityPoints => _dexterityPoints;
        public int CurrentPlayerLuckPoints => _luckPoints;
        
        // Stamina Properties.
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
            // Components.
            _uiManager = UIManager.Instance;
            _audioManager = AudioManager.Instance;
            _playerInputs = PlayerInputs.Instance;
            _playerController = PlayerController.Instance;

            // Player stats.
            _currentPlayerHP = maxPlayerHP;
            _currentPlayerStamina = maxPlayerStamina;

            //Update the maximum size of gauges
            healthBar.SetHealthBarMax(maxPlayerHP);
            _uiManager.SetStaminaBarMax(maxPlayerStamina);
        }

        /**
         * <summary>
         * Update is called once per frame.
         * </summary>
         */
        void Update()
        {
            _currentPlayerHP = Mathf.Clamp(_currentPlayerHP, 0f, maxPlayerHP);

            // Event.
            if (_playerInputs.Interaction)
            {
                OnInteraction?.Invoke(this);
                RegenAllHealth();
            }
        }

        #endregion

        #region Initialize Methods

        /**
         * <summary>
         * Initialize the player level from the save file.
         * </summary>
         * <param name="levelPlayer">The level to initialize.</param>
         * <param name="expPlayer">The experience to initialize.</param>
         */
        public void InitializeLevel(int levelPlayer, int expPlayer)
        {
            _currentPlayerLevel = levelPlayer;
            _currentPlayerExp = expPlayer;
            
            int expReq = playerLevels[playerLevels.FindIndex(level => level.Level == _currentPlayerLevel + 1)]
                .ExpRequired;
            
            _uiManager.UpdatePlayerLevelAndExperienceUI(_currentPlayerLevel, _currentPlayerExp, expReq, _skillPoints);
        }
        
        
        /**
         * <summary>
         * Initialize the player skills and stats from the save file.
         * </summary>
         * <param name="skillPoints">The skill points to initialize.</param>
         * <param name="consPoints">The constitution points to initialize.</param>
         * <param name="strPoints">The strength points to initialize.</param>
         * <param name="vigPoints">The vigor points to initialize.</param>
         * <param name="dexPoints">The dexterity points to initialize.</param>
         * <param name="luckPoints">The luck points to initialize.</param>
         */
        public void InitializePlayerSkills(int skillPoints, int consPoints, int strPoints, int vigPoints, int dexPoints, int luckPoints)
        {
            _skillPoints = skillPoints;
            
            _constitutionPoints = consPoints;
            _strengthPoints = strPoints;
            _vigorPoints = vigPoints;
            _dexterityPoints = dexPoints;
            _luckPoints = luckPoints;
            
            LoadSkillsAndStatsValues();
        }


        /**
         * <summary>
         * Initialize the Player Equipment.
         * </summary>
         * <param name="itemSO">The item data.</param>
         */
        public void InitializePlayerEquipment(Items itemSO)
        {
            if (itemSO is Weapons weaponSO)
            {
                _actualWeaponAtk = weaponSO.WeaponDamage;
                _actualWeaponAtkSpeed = weaponSO.WeaponAttackSpeed;
            }
            else if (itemSO is Armors armorSO)
            {
                _actualArmorDefense = armorSO.ArmorDefense;
            }
        }

        #endregion

        #region Health Methods

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

            //Active the death behaviour.
            if(_currentPlayerHP == 0)
            {
                PlayerDeath();
            }

            //SFX to alert the low life of the player.
            if (_currentPlayerHP < healthAlert)
            {
                _audioManager.PlayerDyingSFX.Play();
            }
        }

        /**
         * <summary>
         * Player death behaviour.
         * </summary>
         */
        private void PlayerDeath()
        {
            StopCoroutine(RegenerateHealth());
            _audioManager.PlayerDeathSFX.Play();
            //To-Do: Function to reload game
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
        private void RegenAllHealth()
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
            _audioManager.PlayerHealedSFX.Play();
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

        #region Stamina Methods

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
                _uiManager.UpdateStaminaBar(_currentPlayerStamina);
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
            _uiManager.UpdateStaminaBar(_currentPlayerStamina);
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
            _uiManager.UpdateStaminaBar(_currentPlayerStamina);
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
                _uiManager.UpdateStaminaBar(_currentPlayerStamina);
                yield return null;
            }
        }

        #endregion

        #region Player Level Methods

        /**
         * <summary>
         * Check the player Experience and see if he can level up.
         * </summary>
         * <param name="expQuantity"></param>
         */
        public void AddPlayerExpAndCheckLevel(int expQuantity)
        {
            if (playerLevels[playerLevels.FindIndex(level => level.Level == _currentPlayerLevel + 1)] == null) return;

            int expReq = playerLevels[playerLevels.FindIndex(level => level.Level == _currentPlayerLevel + 1)]
                .ExpRequired;
            _currentPlayerExp += expQuantity;
            
            if (expReq <= _currentPlayerExp)
            {
                 _currentPlayerExp -= expReq;     // Getting the excess exp.
                 LevelUp();
            }
            else
            {
                _uiManager.UpdatePlayerLevelAndExperienceUI(_currentPlayerLevel, _currentPlayerExp, expReq, _skillPoints);
            }
        }


        /**
         * <summary>
         * Player Level Up Method.
         * </summary>
         */
        private void LevelUp()
        {
            _currentPlayerLevel++;
            
            // Get the next experience required.
            int expReq = playerLevels[playerLevels.FindIndex(level => level.Level == _currentPlayerLevel + 1)]
                .ExpRequired;
            
            _skillPoints++;
            _uiManager.UpdatePlayerLevelAndExperienceUI(_currentPlayerLevel, _currentPlayerExp, expReq, _skillPoints);
        }

        #endregion

        #region Player Skills & Stats Methods

        /**
         * Load the skills and stats values from the save.
         */
        private void LoadSkillsAndStatsValues()
        {
            // Health.
            maxPlayerHP *= Mathf.Pow(1f + 0.025f, _constitutionPoints);
            
            // Attack.
            maxPlayerAttack *= Mathf.Pow(1f + 0.025f, _strengthPoints);
            
            // Stamina.
            maxPlayerStamina *= Mathf.Pow(1f + 0.025f, _vigorPoints);
            
            // Speed.
            maxPlayerSpeed *= Mathf.Pow(1f + 0.025f, _dexterityPoints);
            
            // Luck.
            maxPlayerCritAttack *= Mathf.Pow(1f + 0.025f, _luckPoints);
            maxPlayerLootRate *= Mathf.Pow(1f + 0.025f, _luckPoints);
            
            // Update UI.
            UpdateSkillsAndStats();
        }
        
        
        /**
         * <summary>
         * Link to the UIManager to update the skills and stats on the UI.
         * </summary>
         */
        private void UpdateSkillsAndStats()
        {
            _uiManager.UpdatePlayerStats(
                _currentPlayerHP, 
                maxPlayerHP, 
                _currentPlayerStamina, 
                maxPlayerStamina
                );

            _uiManager.UpdatePlayerSkillPoints(
                _skillPoints,
                _constitutionPoints, 
                _strengthPoints, 
                _vigorPoints, 
                _dexterityPoints,
                _luckPoints
                );
        }


        /**
         * <summary>
         * Upgrade the skills points and update the skills.
         * </summary>
         * <param name="skill">The actual skill point to upgrade</param>
         * <param name="skillUpdate">To know if we add a point or remove one.</param>
         */
        public void UpgradeSkill(String skill, SkillUpdate skillUpdate)
        {
            if(_skillPoints > 0 && skillUpdate == SkillUpdate.Increment)
            {
                switch (skill)
                {
                    case "Constitution": 
                        _constitutionPoints++; 
                        maxPlayerHP *= 1.025f; 
                        break;
                    case "Strength": 
                        _strengthPoints++; 
                        maxPlayerAttack *= 1.025f; 
                        break;
                    case "Vigor": 
                        _vigorPoints++; 
                        maxPlayerStamina *= 1.025f; 
                        break;
                    case "Dexterity": 
                        _dexterityPoints++; 
                        maxPlayerSpeed *= 1.025f; 
                        break;
                    case "Luck": 
                        _luckPoints++; 
                        maxPlayerCritAttack *= 1.025f; 
                        maxPlayerLootRate *= 1.025f; 
                        break;
                }
                _skillPoints--;
            }
            else if(skillUpdate == SkillUpdate.Decrement)
            {
                if (skill == "Constitution" && _constitutionPoints > 0)
                {
                    _constitutionPoints--;
                    maxPlayerHP /= 1.025f;
                    _skillPoints++;
                }
                if (skill == "Strength" && _strengthPoints > 0)
                {
                    _strengthPoints--;
                    maxPlayerAttack /= 1.025f;
                    _skillPoints++;
                }
                if (skill == "Vigor" && _vigorPoints > 0)
                {
                    _vigorPoints--;
                    maxPlayerStamina /= 1.025f;
                    _skillPoints++;
                }
                if (skill == "Dexterity" && _dexterityPoints > 0)
                {
                    _dexterityPoints--;
                    maxPlayerSpeed /= 1.025f;
                    _skillPoints++;
                }
                if (skill == "Luck" && _luckPoints > 0)
                {
                    _luckPoints--;
                    maxPlayerCritAttack /= 1.025f;
                    maxPlayerLootRate /= 1.025f;
                    _skillPoints++;
                }
            }
            UpdateSkillsAndStats();
        }

        #endregion
    }
}
