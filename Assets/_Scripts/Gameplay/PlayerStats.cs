using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Managers;
using UnityEngine;

namespace _Scripts.Gameplay
{
    
    public enum SkillUpdate
    {
        Increment,
        Decrement
    }

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
    
    public class PlayerStats : MonoBehaviour
    {
        #region Variables

        [Header("Player Level")] 
        [SerializeField] private List<PlayerLevel> playerLevels;
        
        [Header("Player Health")] 
        [SerializeField] private int maxPlayerHP = 100;
        
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
        private int _currentPlayerHp;
        private float _currentPlayerStamina;
        private float _currentPlayerAttack;
        private float _currentPlayerCritAttack;
        private float _currentPlayerSpeed;
        private float _currentPlayerLootRate;
        
        // Player Skill Management;
        private int _skillPoints = 10;
        private int _constitutionPoints;
        private int _strengthPoints;
        private int _vigorPoints;
        private int _dexterityPoints;
        private int _luckPoints;
        
        // Stamina Conditions.
        private IEnumerator _regenStamina;
        
        // Components.
        private UIManager _uiManager;

        #endregion

        #region Properties

        public float CurrentPlayerStamina => _currentPlayerStamina;
        public float RollStaminaCost => rollStaminaCost;

        #endregion

        #region Built-In Methods

        void Start()
        {
            _uiManager = UIManager.Instance;
            
            _currentPlayerHp = maxPlayerHP;
            _currentPlayerStamina = maxPlayerStamina;
        }

        #endregion

        #region Initialize Methods

        /**
         * <summary>
         * Initialize the player level from the save file.
         * </summary>
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

        #endregion
        
        #region Health Management

        /**
         * <summary>
         * Remove HP from the player based on a quantity given.
         * </summary>
         * <param name="damage">The number of damage the player took.</param>
         */
        public void TakeDamage(int damage)
        {
            _currentPlayerHp -= damage;
            // TO-DO: Show it on the UI.
        }


        /**
         * <summary>
         * Regen the player's HP based on a quantity given.
         * </summary>
         * <param name="quantity">The quantity to regen.</param>
         */
        public void RegenHealth(int quantity)
        {
            _currentPlayerHp += quantity;
            // TO-DO: Show it on the UI.
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
                // TO-DO: UI Update.
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
            // TO-DO: UI Update.
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
            // TO-DO: UI Update.
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
                // TO-DO: UI Update.
                yield return null;
            }
        }

        #endregion

        #region Player Level Management

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

        #region Player Skills & Stats management

        /**
         * Load the skills and stats values from the save.
         */
        private void LoadSkillsAndStatsValues()
        {
            // Health.
            float playerHealthUpdate = maxPlayerHP * Mathf.Pow(1f + 0.025f, _constitutionPoints);
            maxPlayerHP = (int)playerHealthUpdate;
            
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
                _currentPlayerHp, 
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
         * <param name="playerSkills">The actual skill point to upgrade</param>
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
                        float healthFloatValue = maxPlayerHP * 1.025f; 
                        maxPlayerHP = Mathf.RoundToInt(healthFloatValue); 
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
                    float healthFloatValue = maxPlayerHP / 1.025f;
                    maxPlayerHP = Mathf.RoundToInt(healthFloatValue);
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
