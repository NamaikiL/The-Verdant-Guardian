using System.Collections;
using _Scripts.Managers;
using _Scripts.Scriptables;
using UnityEngine;
using UnityEngine.AI;

namespace _Scripts.Gameplay
{
    public enum DragonAttackType
    {
        Charge,
        FireBreath,
        Stomp
    }
    
    public class DragonController : MonoBehaviour
    {
        #region Variables

        [Header("Dragon Properties")] 
        [SerializeField] private float minimumDistance = 30f;
        [SerializeField] private float maximumDistance = 50f;
        [SerializeField] private Enemies dragonSO;
        
        [Header("Attack Properties")]
        [SerializeField] private float attackInterval = 40f; // Time between attacks
        [SerializeField] private float fireBreathRange = 10f; // Adjust as needed
        [SerializeField] private float fireBreathDuration = 8f; 
        [SerializeField] private float stompAnimationDuration = 2f;
        
        // Dragon Stats.
        private int _maxDragonHealth;
        private int _currentDragonHealth;
        private int _dragonDamage;
        private int _dragonAttackSpeed;
        private bool _isDragonDead;
        
        // Dragon Attack.
        private bool _isAttacking;
        
        // Player Find.
        private Transform _playerTransform;
        
        // Components.
        private NavMeshAgent _navMeshAgent;
        private UIManager _uiManager;

        #endregion

        #region Built-In Methods

        void Start()
        {
            // Components.
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _uiManager = UIManager.Instance;
            
            // Initializing Methods.
            InitializeDragonStats();
            
            // Player Find.
            _playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
            
            // Show the UI.
            StartCoroutine(_uiManager.DragonGroupFade(true));
            
            // Start the dragon behavior.
            StartCoroutine(UpdatePathRoutine());
            StartCoroutine(AttackRoutine());
        }

        #endregion

        #region Initialize Methods

        private void InitializeDragonStats()
        {
            _maxDragonHealth = dragonSO.EnemyMaxHealth;
            _currentDragonHealth = _maxDragonHealth;
            _dragonDamage = dragonSO.Damage;
            _dragonAttackSpeed = dragonSO.AttackSpeed;
        }

        #endregion

        #region AI Behavior Movement Methods

        /**
         * <summary>
         * Find a new path after every end of one.
         * </summary>
         */
        private IEnumerator UpdatePathRoutine()
        {
            while (true)
            {
                Vector3 newDestination = FindNewDestinationWithinRange();   // Take a new path.
                
                if (newDestination != Vector3.zero)
                {
                    _navMeshAgent.SetDestination(newDestination);
                    
                    // Wait until the dragon is close to the current destination before finding a new one.
                    while (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance > 1f)
                    {
                        yield return null;
                    }
                }

                yield return null;
            }
        }

        
        /**
         * <summary>
         * Find a new destination for the dragon with given range from the player.
         * </summary>
         * <returns>The path to take.</returns>
         */
        private Vector3 FindNewDestinationWithinRange()
        {
            Vector3 bestDestination = Vector3.zero;
            float bestDistance = 0;
    
            // Attempt multiple times to find a good destination.
            for (int i = 0; i < 10; i++)
            {
                // Generate a direction and distance within the specified range.
                Vector3 randomDirection = Random.insideUnitSphere.normalized;
                float randomDistance = Random.Range(minimumDistance, maximumDistance);
                Vector3 potentialDestination = _playerTransform.position + randomDirection * randomDistance;

                // Ensure there's no obstacle.
                if (NavMesh.SamplePosition(potentialDestination, out NavMeshHit hit, randomDistance, NavMesh.AllAreas))
                {
                    float distanceToPlayer = Vector3.Distance(_playerTransform.position, hit.position);
                    
                    // Find the most longer destination.
                    if (distanceToPlayer > bestDistance)
                    {
                        bestDestination = hit.position;
                        bestDistance = distanceToPlayer;
                    }
                }
            }
            
            return bestDestination != Vector3.zero ? bestDestination : transform.position;  // Return the best destination.
        }

        #endregion

        #region Dragon Attack Methods

        /**
         * <summary>
         * Attack Coroutine Timer.
         * </summary>
         */
        private IEnumerator AttackRoutine()
        {
            while (true)
            {
                if(!_isAttacking)
                {
                    yield return new WaitForSeconds(attackInterval); // Wait for the attack interval

                    _isAttacking = true;
                    //DragonAttackType selectedAttack = (DragonAttackType)Random.Range(0, 3); // Choose an attack
                    DragonAttackType selectedAttack = DragonAttackType.FireBreath;
                    PerformAttack(selectedAttack);
                }

                yield return null;
            }
        }
        
        
        /**
         * <summary>
         * The Attack State Handler.
         * </summary>
         * <param name="attackType">The attack to perform.</param>
         */
        private void PerformAttack(DragonAttackType attackType)
        {
            switch (attackType)
            {
                case DragonAttackType.Charge:
                    ChargeAttack();
                    break;
                case DragonAttackType.FireBreath:
                    StartCoroutine(FireBreathAttack());
                    break;
                case DragonAttackType.Stomp:
                    // TailSwipeAttack();
                    break;
            }
        }
        

        /**
         * <summary>
         * The Charge attack.
         * </summary>
         */
        private void ChargeAttack()
        {
            // Calculate the direction from the dragon to the player, then calculate the charge point from it.
            Vector3 playerPos = _playerTransform.position;
            Vector3 directionToPlayer = (playerPos - transform.position).normalized;
            Vector3 chargePoint = playerPos + directionToPlayer * minimumDistance;

            // Get the original speed and apply the new one.
            float originalSpeed = _navMeshAgent.speed;
            _navMeshAgent.speed *= 2f;

            StartCoroutine(PerformCharge(chargePoint, originalSpeed));
        }
        
        
        /**
         * <summary>
         * Perform the charge attack when he has the point for it.
         * </summary>
         * <param name="chargePoint">The point he needs to charge to.</param>
         * <param name="originalSpeed">The original speed of the dragon.</param>
         */
        private IEnumerator PerformCharge(Vector3 chargePoint, float originalSpeed)
        {
            // Set the path.
            _navMeshAgent.SetDestination(chargePoint);
    
            // Run until close to the end of the charge.
            while (Vector3.Distance(transform.position, chargePoint) > 5f)
            {
                yield return null;
            }

            // Decrease its speed gradually.
            while (_navMeshAgent.speed > originalSpeed)
            {
                _navMeshAgent.speed -= Time.deltaTime * 2;
                yield return null;
            }

            // Get its speed back and do its movement.
            _navMeshAgent.speed = originalSpeed;
            _isAttacking = false;
        }
        
        
        /**
         * <summary>
         * The FireBreathAttack.
         * </summary>
         */
        private IEnumerator FireBreathAttack()
        {
            // Stop its movement.
            _navMeshAgent.isStopped = false;
            StopCoroutine(UpdatePathRoutine());
            
            // Approach the player so he can be close to breath its fire.
            while (Vector3.Distance(transform.position, _playerTransform.position) > fireBreathRange)
            {
                _navMeshAgent.SetDestination(_playerTransform.position);
                yield return null;
            }

            // Stop and perform the fire breath.
            _navMeshAgent.isStopped = true;
            // TODO: Trigger fire breath animation and effects here.

            yield return new WaitForSeconds(fireBreathDuration); // Wait for the duration of the fire breath.

            // Then resume its AI Behavior.
            _navMeshAgent.isStopped = false;
            _isAttacking = false; // Allow for new attacks
            StartCoroutine(UpdatePathRoutine());
        }
        
        
        /**
         * <summary>
         * The Stomping Attack.
         * </summary>
         */
        private IEnumerator StompAttack()
        {
            // Stop its movements.
            _navMeshAgent.isStopped = true;
            
            // Face the player
            transform.LookAt(new Vector3(_playerTransform.position.x, transform.position.y, _playerTransform.position.z));

            // TODO: Trigger stomp animation here.

            yield return new WaitForSeconds(stompAnimationDuration);

            // Make that he can move again.
            _navMeshAgent.isStopped = false;
            _isAttacking = false;
        }

        #endregion

        #region Dragon Health Management Methods

        /**
         * <summary>
         * Take damage method.
         * </summary>
         * <param name="damageQuantity">The damage the dragon has taken.</param>
         */
        private void TakeDamage(int damageQuantity)
        {
            _currentDragonHealth = Mathf.Clamp(_currentDragonHealth - damageQuantity, 0, _maxDragonHealth);

            _uiManager.DragonLifeBarUpdate(_currentDragonHealth, _maxDragonHealth);
            
            if (_currentDragonHealth == 0)
            {
                StartCoroutine(DeathToTheDragon());
            }
            else
            {
                // Damage handler(animation)
            }
        }


        private IEnumerator DeathToTheDragon()
        {
            StopCoroutine(UpdatePathRoutine());
            StopCoroutine(AttackRoutine());
            _navMeshAgent.isStopped = true;
            _uiManager.DragonGroupFade(false);

            // TODO: Play death Animation and change the seconds by the animation seconds.
            yield return new WaitForSeconds(1f);
            Destroy(gameObject);
        }

        #endregion
    }
}
