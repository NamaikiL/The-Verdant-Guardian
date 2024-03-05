using _Scripts.Scriptables;
using _Scripts.UI;
using UnityEngine;

namespace _Scripts.Gameplay
{

    public class EnemyController : MonoBehaviour
    {
        #region Variables

        [Header("Scriptable Data")]
        [SerializeField] protected Enemies enemiesScriptable;

        [Header("Scripts")]
        [SerializeField] private HealthBar healthBar;

        //Enemy Stats
        private int _currentEnemyHP;

        #endregion

        #region Built-In Methods

        /**
        * <summary>
        * Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.
        * </summary>
        */
        void Start()
        {
            _currentEnemyHP = enemiesScriptable.EnemyMaxHealth;

            //Update the maximum size of gauges
            healthBar.SetHealthBarMax(enemiesScriptable.EnemyMaxHealth);
        }
        
        /**
         * <summary>
         * Update is called once per frame.
         * </summary>
         */
        void Update()
        {
            _currentEnemyHP = Mathf.Clamp(_currentEnemyHP, 0, enemiesScriptable.EnemyMaxHealth);
        }

        #endregion

        #region Health Management

        /**
         * <summary>
         * Remove HP from the NPC based on a quantity given.
         * </summary>
         * <param name="damage">The number of damage the player took.</param>
         */
        public void TakeDamage(int damage)
        {
            _currentEnemyHP -= damage;
            healthBar.UpdateHealthBar(_currentEnemyHP);

            if (_currentEnemyHP == 0)
            {
                NpcDeath();
            }
        }

        /**
         * <summary>
         * Give the behaviour to the object when he dies.
         * </summary>
         */
        private void NpcDeath()
        {
            Destroy(this.gameObject);
        }

        #endregion
    }
}