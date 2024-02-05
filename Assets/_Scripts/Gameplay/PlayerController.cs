using System;
using System.Collections.Generic;
using _Scripts.Managers;
using UnityEngine;

namespace _Scripts.Gameplay
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(Animator))]
    public class PlayerController : MonoBehaviour
    {
        #region Variables
        
        [Header("Movements")]
        [SerializeField] private float moveSpeed = 7f; 
        [SerializeField] private float turnSmoothSpeed = 0.05f;
    
        [Header("Forces")]
        [SerializeField] private float jumpForce = 10f;
        [SerializeField] private float gravity = 6f;
    
        // Movements.
        private float _currentVelocity;
        private float _verticalSpeed;
        private bool _canMove = true;
        private Vector3 _movement = Vector3.zero;
        private Vector3 _direction = Vector2.zero;
        private Vector3 _moveDirection = Vector3.zero;
        
        // NPC Interaction.
        private bool _isNpcHere;

        // Quests Handler(Public variables not permanent).
        public List<Quest> questsList = new List<Quest>();
        public Quest activeQuest;

        // EVENT.
        public static event Action<PlayerController> OnInteraction; 
        
        // Components.
        private Animator _animator;
        private Camera _camera;
        private CharacterController _characterController;
        
        private PlayerInputs _playerInputs;
        private UIManager _uiManager;
        
        #endregion

        #region Builtin Methods

        /**
         * <summary>
         * Start is called before the first frame update.
         * </summary>
         */
        void Start()
        {
            // Component by instance.
            _playerInputs = PlayerInputs.Instance;
            _uiManager = UIManager.Instance;

            // Component in object.
            _characterController = GetComponent<CharacterController>();
            _animator = GetComponent<Animator>();

            _camera = Camera.main;  // Camera.
        }


        /**
         * <summary>
         * FixedUpdate is called once per fixed frame.
         * </summary>   
         */
        void FixedUpdate()
        {
            // Player movements.
            Locomotion();
            CalculateVerticalMovement();
            
            // Player animations.
            UpdateAnimation();
            
            // EVENT
            if (_playerInputs.Interaction)
            {
                OnInteraction?.Invoke(this);
            }
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
                    // Angle calculation.
                    float targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg +
                                        _camera.transform.eulerAngles.y;
                    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _currentVelocity,
                        turnSmoothSpeed);

                    transform.rotation = Quaternion.Euler(0, angle, 0);

                    _moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                }
                else
                {
                    _moveDirection = Vector3.zero;
                }       // In case the player don't move.
            }
            
            _movement = _moveDirection.normalized * (moveSpeed * Time.deltaTime);       // Movements of the player.
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
                _verticalSpeed = -gravity * 0.3f * Time.deltaTime;

                if (_playerInputs.Jumped && _canMove)
                {
                    _verticalSpeed = jumpForce;     // Jump.
                }
            }
            else
            {
                _verticalSpeed -= gravity * Time.deltaTime;
            }
            _movement += _verticalSpeed * Vector3.up;
            _characterController.Move(_movement);
        }


        /**
         * <summary>
         * Update the animations.
         * </summary>
         */
        private void UpdateAnimation()
        {
            if(moveSpeed != 0f)     // In case the player doesn't have a move speed.
                _animator.SetFloat($"Locomotion", _moveDirection.normalized.magnitude);

            if (!_characterController.isGrounded)
            {
                _animator.SetBool($"IsGrounded", false);
                //_animator.SetFloat("VerticalSpeed", _verticalSpeed);
            }
            else
            {
                _animator.SetBool($"IsGrounded", true);
                /*if (_inputs.Attack)
                {
                    _animator.SetTrigger("Attack");
                }
                else
                {
                    _animator.ResetTrigger("Attack");
                }*/
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


        /**
         * <summary>
         * Add the player a new quest.
         * </summary>
         * <param name="quest">The quest to add.</param>
         */
        public void ReceiveNewQuest(Quest quest)
        {
            questsList.Add(quest);      // Add the quest to the list.
            
            // Change active quest.
            quest.IsActive = true;
            activeQuest = quest;
            
            // EVENT.
            Quest.OnQuestComplete += RemoveCompletedQuest;
            
            // UI.
            _uiManager.AddNewQuest(quest.Title, quest.Description);
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
            Quest.OnQuestComplete -= RemoveCompletedQuest;
        }

        
        /**
         * <summary>
         * Update the quests with gathering type.
         * </summary>
         * <param name="itemType">The item type.</param>
         */
        public void AddItemToInventory(ItemType itemType)
        {
            foreach (Quest quest in questsList)
            {
                foreach (Objectives objective in quest.Objectives)
                {
                    if (!objective.IsComplete && objective.ActualObjectiveType == ObjectiveType.Collect && objective.ActualItemType == itemType)
                    {
                        objective.NbCollected++;
                        if (objective.NbCollected == objective.NbToCollect)
                        {
                            objective.CompleteObjective();
                        }
                        return;
                    }
                }
            }
        }

        #endregion

    }
}
