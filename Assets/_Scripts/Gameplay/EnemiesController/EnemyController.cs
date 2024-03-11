using System.Collections;
using _Scripts.Scriptables;
using _Scripts.UI;
using UnityEngine;

namespace _Scripts.Gameplay.EnemiesController
{

    public class EnemyController : MonoBehaviour
    {
        #region Variables

        [Header("Scriptable Data")]
        [SerializeField] protected Enemies enemiesScriptable;

        [Header("Health Display")]
        [SerializeField] private HealthBar healthUI;
        [SerializeField] private GameObject healthBar;
        [SerializeField] private float healthDisplayTime = 15f;

        [Header("Patrol")]
        [SerializeField] private Transform modelManager;
        [SerializeField] private Transform[] patrolPoints;
        [SerializeField] private float speedPatrol = 0.5f;

        //Enemy Stats.
        private int _currentEnemyHP;

        //Health display.
        private bool _healthIsActive = false;

        //Patrol features.
        private int _targetPoint;
        private bool _inInteraction;

        #endregion

        #region Built-In Methods

        /**
        * <summary>
        * Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.
        * </summary>
        */
        void Start()
        {
            //Components.
            _currentEnemyHP = enemiesScriptable.EnemyMaxHealth;

            //Update the maximum size of gauges.
            healthUI.SetHealthBarMax(enemiesScriptable.EnemyMaxHealth);

            //Patrol set up.
            _targetPoint = 0;
        }

        /**
         * <summary>
         * Update is called once per frame.
         * </summary>
         */
        void Update()
        {
            _currentEnemyHP = Mathf.Clamp(_currentEnemyHP, 0, enemiesScriptable.EnemyMaxHealth);

            Patrol(false);
        }

        #endregion

        #region Health Method

        /**
         * <summary>
         * Remove HP from the NPC based on a quantity given.
         * </summary>
         * <param name="damage">The number of damage the player took.</param>
         */
        public void TakeDamage(int damage)
        {
            //Display health.
            healthBar.SetActive(true);
            _healthIsActive = true;

            if(_healthIsActive == true)
            {
                StartCoroutine(HealthDisplayTime());
            }

            //Take damage and update UI.
            _currentEnemyHP -= damage;
            healthUI.UpdateHealthBar(_currentEnemyHP);

            if (_currentEnemyHP == 0)
            {
                Destroy(this.gameObject);
            }
        }

        /**
         * <summary>
         * Coroutine for display health bar.
         * </summary>
         */
        private IEnumerator HealthDisplayTime()
        {
            yield return new WaitForSeconds(healthDisplayTime);
            healthBar.SetActive(false);
        }

        #endregion

        #region Patrol Method

        /**
         * <summary>
         * Make the character patrol between points.
         * </summary>
         */
        public void Patrol(bool _inInteraction)
        {
            if(!_inInteraction)
            {
                if(modelManager.transform.position == patrolPoints[_targetPoint].position)
                {
                    _targetPoint++;

                    if (_targetPoint >= patrolPoints.Length)
                    {
                        _targetPoint = 0;
                    }
                }

                modelManager.transform.position = Vector3.MoveTowards(modelManager.transform.position,
                    patrolPoints[_targetPoint].position, speedPatrol * Time.deltaTime);

                modelManager.LookAt(patrolPoints[_targetPoint]);
            }
            else
            {
                UnityEngine.Debug.Log("r");
                speedPatrol = 0;
            }

            _inInteraction = !_inInteraction;

        }

        #endregion
    }
}