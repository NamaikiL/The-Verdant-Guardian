using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Managers;
using _Scripts.Scriptables;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace _Scripts.Gameplay
{
    /**
     * <summary>
     * The Player controller, all functions about his behavior are here.
     * </summary>
     */
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerInputs))]
    [RequireComponent(typeof(PlayerStats))]
    public class PlayerController : MonoBehaviour
    {
        #region Variables
        
        [Header("Movements")]
        [SerializeField] private float moveSpeed = 7f; 
        [SerializeField] private float turnSmoothSpeed = 0.05f;
        
        [Header("Sprint")]
        [SerializeField] private float moveSpeedSprint = 15f; 
        
        [Header("Crouch")]
        [SerializeField] private float moveSpeedCrouch = 3f;

        [Header("Dodging")] 
        [SerializeField] private float dodgeSpeed = 10f;
        [SerializeField] private float dodgeDuration = .2f;
        [SerializeField] private float timeBetweenDodge = 1f;
    
        [Header("Forces")]
        [SerializeField] private float jumpForce = 2f;
        [SerializeField] private float gravity = -9.81f;
    
        // Movements.
        private float _currentMoveSpeed;
        private float _currentVelocity;
        private float _verticalSpeed;
        private bool _canMove = true;
        private Vector3 _movement = Vector3.zero;
        private Vector3 _direction = Vector2.zero;
        private Vector3 _moveDirection = Vector3.zero;
        
        // Crouching
        private float _baseHeight;
        private bool _isCrouching;
        
        // Dodging.
        private bool _canDodge = true;
        private bool _isDodging;
        private Vector3 dodgeDirection;

        //Attack
        private bool _isAttacking;
        
        // NPC Interaction.
        private bool _isNpcHere;

        // Quests Handler(Public variables not permanent).
        // TO-DO: Move this to a quest handler.
        private List<Quest> _playerQuestsList = new List<Quest>();
        private Quest _playerActiveQuest;
        
        // EVENT.
        public static event Action<PlayerController> OnInteraction; 
        
        // Components.
        private Animator _animator;
        private Camera _camera;
        private CharacterController _characterController;
        private PlayerStats _playerStats;
        
        private PlayerInputs _playerInputs;
        private UIManager _uiManager;
        private AudioManager _audioManager;
        private AnimationManager _animationManager;

        //Singleton
        private static PlayerController _instance;

        #endregion

        #region Properties

        // Player Movements Values.
        public Vector3 MoveDirection => _moveDirection;
        
        // Player Coordinates Properties.
        public Vector3 CurrentPlayerPosition => gameObject.transform.localPosition;
        public Vector3 CurrentPlayerRotation => gameObject.transform.eulerAngles;
        
        // Conditions.
        public bool IsCrouching => _isCrouching;
        public bool IsDodging => _isDodging;
        
        // Quest Property.
        public List<Quest> PlayerQuestsList => _playerQuestsList;

        // Singleton Property.
        public static PlayerController Instance => _instance;

        #endregion

        #region Built_In Methods

        /**
         * <summary>
         * Unity calls Awake when an enabled script instance is being loaded.
         * </summary>
         */
        void Start()
        {
            // Singleton.
            if (_instance) Destroy(gameObject);
            _instance = this;
        }

        /**
         * <summary>
         * Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.
         * </summary>
         */
        void Start()
        {
            // Component by instance.
            _playerInputs = PlayerInputs.Instance;
            _uiManager = UIManager.Instance;
            _audioManager = AudioManager.Instance;
            _animationManager = AnimationManager.Instance;

            // Component in object.
            _characterController = GetComponent<CharacterController>();
            _animator = GetComponent<Animator>();
            _playerStats = GetComponent<PlayerStats>();

            _camera = Camera.main;  // Camera.

            _currentMoveSpeed = moveSpeed;  // Apply the moveSpeed value.
            _baseHeight = _characterController.height;
            
            _animationManager.InitializePlayerAnimator(GetComponent<Animator>());
        }


        /**
         * <summary>
         * FixedUpdate is called once per fixed frame.
         * </summary>   
         */
        void FixedUpdate()
        {
            // Dodge Behavior
            if (_playerInputs.Dodge && !_isDodging && _canDodge)
            {
                StartDodge();
            }
            if (_isDodging)
            {
                ContinueDodge();
            }
            // Movements & Gravity.
            else
            {
                // Player movements.
                Locomotion();
                CalculateVerticalMovement();
            }
            
            // Player animations.
            UpdateAnimation();
        }

        /**
         * <summary>
         * Update is called once per frame.
         * </summary>
         */
        void Update()
        {
            if(!_isDodging)
            {
                // EVENT
                if (_playerInputs.Interaction)
                {
                    OnInteraction?.Invoke(this);
                }

                MoveSpeedBehavior();
                Crouch();
            }
        }


        /**
         * <summary>
         * When a GameObject collides with another GameObject, Unity calls OnTriggerEnter.
         * </summary>
         * <param name="other">The other Collider involved in this collision.</param>
         */
        void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.layer == LayerMask.NameToLayer($"Dragon")) 
                _playerStats.TakeDamage(GameObject.FindWithTag("Dragon").GetComponent<DragonController>().DragonSO.Damage);
        }
        
        
        /**
         * <summary>
         * OnControllerColliderHit is called when the controller hits a collider while performing a Move.
         * </summary>
         * <param name="hit">The other Hit involved in this collision.</param>
         */
        void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (hit.gameObject.layer == LayerMask.NameToLayer($"Dragon"))
                _playerStats.TakeDamage(1f);
        }

        #endregion

        #region Initialize Methods

        /**
         * <summary>
         * Initialize the player position.
         * </summary>
         * <param name="playerPosition">The player spawn position.</param>
         * <param name="playerRotation">The player spawn rotation.</param>
         */
        public void InitializePlayerCoordinates(Vector3 playerPosition, Vector3 playerRotation)
        {
            _characterController.enabled = false;
            transform.position = playerPosition;
            transform.eulerAngles = playerRotation;
            _characterController.enabled = true;
        }

        #endregion
        
        #region Custom Methods

        /**
         * <summary>
         * Locomotion of the player.
         * </summary>
         */
        private void Locomotion()
        {
            if (!_playerInputs) return;

            if (_canMove)
            {
                _direction.Set(_playerInputs.Movement.x, 0, _playerInputs.Movement.y);

                if (_direction.normalized.magnitude >= 0.1f)
                {
                    
                    float targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg +
                                        _camera.transform.eulerAngles.y;    // Angle calculation.
                    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _currentVelocity,
                        turnSmoothSpeed);   // Smoothing the rotation.
                    transform.rotation = Quaternion.Euler(0, angle, 0); // Rotation of the player.
                    
                    _moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                }
                else
                {
                    _moveDirection = Vector3.zero;
                }       // In case the player don't move.
            }
            
            _movement = _moveDirection.normalized * (_currentMoveSpeed * Time.deltaTime);       // Movements of the player.
        }


        /**
         * <summary>
         * Apply a gravity to the player.
         * </summary>
         */
        private void CalculateVerticalMovement()
        {
            if (_characterController.isGrounded)
            {
                _verticalSpeed += gravity * Time.deltaTime;
                
                if (_playerInputs.Jumped && _canMove && !_isCrouching)
                {
                    _verticalSpeed = Mathf.Sqrt(jumpForce * -3f * gravity);     // Jump.
                }
            }
            else
            {
                _verticalSpeed += gravity * Time.deltaTime;
            }
            
            _movement += _verticalSpeed * Vector3.up;
            _characterController.Move(_movement);
        }


        /**
         * <summary>
         * When the speed needs a change.
         * </summary>
         */
        private void MoveSpeedBehavior()
        {
            if (_playerInputs.Sprint && !_isCrouching && _playerStats.CurrentPlayerStamina > 0f)
            {
                _currentMoveSpeed = moveSpeedSprint;
                _playerStats.UseStaminaSprint();
            }
            else if (_isCrouching)
            {
                _currentMoveSpeed = moveSpeedCrouch;
            }
            else
            {
                _currentMoveSpeed = moveSpeed;
            }
        }


        /**
         * <summary>
         * Handle the Crouching behavior.
         * </summary>
         */
        private void Crouch()
        {
            if (_playerInputs.Crouch)
                _isCrouching = !_isCrouching;
        }


        /**
         * <summary>
         * The start of the dodge.
         * </summary>
         */
        private void StartDodge()
        {
            _playerInputs.Dodge = false;
            if (_playerStats.CurrentPlayerStamina > _playerStats.RollStaminaCost
                && !_isCrouching)
            {
                _canDodge = false;
                _playerStats.UseStaminaRoll();

                _isDodging = true;
                _animationManager.UpdateDodgingAnimation(_isDodging, dodgeDuration);
                Vector3 inputDirection = new Vector3(_playerInputs.Movement.x, 0, _playerInputs.Movement.y).normalized;
                if (inputDirection == Vector3.zero)
                {
                    dodgeDirection = transform.forward;
                }
                else
                {
                    inputDirection = inputDirection.normalized;
                    dodgeDirection = _camera.transform.forward * inputDirection.z + _camera.transform.right * inputDirection.x;
                    dodgeDirection.y = 0;
                }
                
                dodgeDirection = dodgeDirection.normalized;
                Invoke("EndDodge", dodgeDuration);
            }
        }


        /**
         * <summary>
         * The action of the Dodge.
         * </summary>
         */
        private void ContinueDodge()
        {
            _characterController.Move(dodgeDirection * (dodgeSpeed * Time.deltaTime));
        }


        /**
         * <summary>
         * End the dodge.
         * </summary>
         */
        private void EndDodge()
        {
            _isDodging = false;
            StartCoroutine(CanDodge());
        }


        /**
         * <summary>
         * Make that the player can dodge again after precised time.
         * </summary>
         */
        private IEnumerator CanDodge()
        {
            yield return new WaitForSeconds(timeBetweenDodge);
            _canDodge = true;
        }

        
        /**
         * <summary>
         * Update the animations.
         * </summary>
         */
        private void UpdateAnimation()
        {
            if (!_characterController.isGrounded)
            {
                _animationManager.UpdateJumpingAnimation(_verticalSpeed, _characterController.isGrounded);
            }
            else
            {
                _animationManager.UpdateJumpingAnimation(_verticalSpeed, _characterController.isGrounded);
                
                bool isSprinting = _playerInputs.Sprint && _playerStats.CurrentPlayerStamina > 0f;
                if (moveSpeed != 0f) // In case the player doesn't have a move speed.
                    _animationManager.UpdateLocomotionAnimation(_moveDirection.normalized.magnitude, isSprinting);
                
                _animationManager.UpdateCrouchingAnimation(_isCrouching);
            }
                
        }

        
        /**
         * <summary>
         * Change the move state of the player.
         * </summary>
         * <param name="canMove">Tells if the player can move or not.</param>
         */
        private void CanMove(bool canMove)
        {
            if (canMove)
            {
                _canMove = true;
            }
            else
            {
                _canMove = false;
                _moveDirection = Vector3.zero;
            }
        }

        #endregion

        #region Attack Methods

        /**
         * <summary>
         * Attack behaviour of the player.
         * </summary>
         */
        private void Attack()
        {
            if (_playerInputs.Attack)
            {
                if (!_isAttacking)
                {
                    Cursor.lockState = CursorLockMode.None;
                    _audioManager.PlayerAttackSFX.Play();
                    Debug.Log("ee");
                }
                else
                {

                    Cursor.lockState = CursorLockMode.Locked;
                    Debug.Log("elllle");
                }

                _isAttacking = !_isAttacking;
            }
        }

        #endregion

        #region Quest Management

        /**
         * <summary>
         * Add the player a new quest.
         * </summary>
         * <param name="quest">The quest to add.</param>
         */
        public void ReceiveNewQuest(Quest quest)
        {
            _playerQuestsList.Add(quest);      // Add the quest to the list.
            
            // Change active quest.
            quest.IsActive = true;
            _playerActiveQuest = quest;
            
            // EVENT.
            quest.OnQuestComplete += RemoveCompletedQuest;
            
            // UI.
            _uiManager.AddNewQuest(quest.QuestTitle, quest.QuestDescription);
        }

        
        /**
         * <summary>
         * Remove the quest from the player.
         * </summary>
         * <param name="quest">The quest to remove.</param>
         */
        private void RemoveCompletedQuest(Quest quest)
        {
            // EVENT.
            quest.OnQuestComplete -= RemoveCompletedQuest;
            _playerQuestsList.Remove(quest);

            if (_playerQuestsList.Count > 0)
            {
                _playerActiveQuest = _playerQuestsList[0];
                _uiManager.AddNewQuest(_playerQuestsList[0].QuestTitle, _playerQuestsList[0].QuestDescription);
            }
            _uiManager.RemoveQuest();
            
            Debug.Log(quest.QuestTitle + "Quête terminé");
        }

        #endregion

    }
}
