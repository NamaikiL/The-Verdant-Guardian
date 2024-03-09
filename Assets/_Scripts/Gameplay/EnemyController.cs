using System.Collections;
using _Scripts.Managers;
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
        private bool _isSfxPlaying;

        //Component.
        private AudioManager _audioManager;


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
            _audioManager = AudioManager.Instance;
            _currentEnemyHP = enemiesScriptable.EnemyMaxHealth;

            //Update the maximum size of gauges.
            healthUI.SetHealthBarMax(enemiesScriptable.EnemyMaxHealth);

            //Patrol set up.
            _targetPoint = 0;

            IdleSFX();
        }

        /**
         * <summary>
         * Update is called once per frame.
         * </summary>
         */
        void Update()
        {
            _currentEnemyHP = Mathf.Clamp(_currentEnemyHP, 0, enemiesScriptable.EnemyMaxHealth);

            Patrol();
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
                StartCoroutine(DelayEnemyDeath());
            }
        }

        /**
         * <summary>
         * Coroutine for giving the behaviour to the object when he dies.
         * </summary>
         */
        private IEnumerator DelayEnemyDeath()
        {
            _audioManager.EnemyDeathSFX.Play();
            yield return new WaitForSeconds(0.5f);
            Destroy(this.gameObject);
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
        private void Patrol()
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

        }

        /**
         * <summary>
         * Play idle SFX.
         * </summary>
         */
        private void IdleSFX()
        {
            StartCoroutine(Delay());
        }

        /**
         * <summary>
         * Coroutine to play random SFX at random time.
         * </summary>
         */
        private IEnumerator Delay()
        {
            _isSfxPlaying = true;

            if(_isSfxPlaying == true)
            {
                //Choose random SFX.
                AudioSource idleSFX = _audioManager.EnemyIdleSFX[Random.Range(0, _audioManager.EnemyIdleSFX.Length)];
                idleSFX.Play();
                _isSfxPlaying = false;

                if(_isSfxPlaying == false)
                {
                    //Play SFX at random time.
                    float randomTime = Random.Range(_audioManager.MinRadomTimeIdleSFX, _audioManager.MaxRadomTimeIdleSFX);
                    yield return new WaitForSeconds(randomTime);

                    IdleSFX();
                }   //Allows the SFX to be play more than one time.
            }
        }

        #endregion
    }
}