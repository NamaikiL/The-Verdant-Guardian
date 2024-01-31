using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Gameplay
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        #region Variables
        [Header("Movements")]
        [SerializeField] private float moveSpeed = 7f; 
        [SerializeField] private float turnSmoothSpeed = 0.05f;
    
        [Header("Forces")]
        [SerializeField] private float jumpForce = 10f;
        [SerializeField] private float gravity = 6f;
    
        private float _currentVelocity;
        private float _verticalSpeed;
        private bool _canMove = true;
        private bool _isNpcHere = false;

        public List<Quest> questsList = new List<Quest>();
        public Quest activeQuest;

        public static event Action<PlayerController> OnInteraction; 

        private Vector3 _movement = Vector3.zero;
        private Vector3 _direction = Vector2.zero;
        private Vector3 _moveDirection = Vector3.zero;
        
        private PlayerInputs _inputs;
        private CharacterController _characterController;
        private Camera _camera;
        private Animator _animator;
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
            _inputs = PlayerInputs.Instance;
            _uiManager = UIManager.Instance;

            _characterController = GetComponent<CharacterController>();
            _animator = GetComponent<Animator>();

            _camera = Camera.main;
        }


        /**
         * <summary>
         * Update is called once per frame.
         * </summary>   
         */
        void FixedUpdate()
        {
            Locomotion();
            CalculateVerticalMovement();
            UpdateAnimation();
            
            // EVENT
            if (_inputs.Interaction)
            {
                OnInteraction?.Invoke(this);
            }
        }

        #endregion

        #region Custom Methods

        /**
         * <summary>
         * Locomotion for the player.
         * </summary>
         */
        private void Locomotion()
        {
            if (!_inputs) return;

            if (_canMove)
            {
                _direction.Set(_inputs.Movement.x, 0, _inputs.Movement.y);

                if (_direction.normalized.magnitude >= 0.1f)
                {
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
                }
            }
            _movement = _moveDirection.normalized * (moveSpeed * Time.deltaTime);
        }


        private void CalculateVerticalMovement()
        {

            if (_characterController.isGrounded)
            {
                //Debug.Log("Grounded");
                _verticalSpeed = -gravity * 0.3f * Time.deltaTime;

                if (_inputs.Jumped && _canMove)
                {
                    //Debug.Log("Je vole");
                    _verticalSpeed = jumpForce;
                }

           
            }
            else
            {
                _verticalSpeed -= gravity * Time.deltaTime;
                //Debug.Log("isNotGrounded");
            }

            _movement += _verticalSpeed * Vector3.up;
            _characterController.Move(_movement);

        }


        private void UpdateAnimation()
        {
        
            if(moveSpeed != 0f)
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


        public void ReceiveNewQuest(Quest quest)
        {
            questsList.Add(quest);
            quest.IsActive = true;
            Quest.OnQuestComplete += RemoveCompletedQuest;
            activeQuest = quest;
            _uiManager.AddNewQuest(quest.Title, quest.Description);
        }

        
        public void RemoveCompletedQuest(Quest quest)
        {
            Quest.OnQuestComplete -= RemoveCompletedQuest;
        }

        
        public void AddItemToInventory(ItemType itemType)
        {
            Debug.Log("AddItemToInventory of type:" + itemType.ToString());

            foreach (Quest quest in questsList)
            {
                foreach (Objectives objective in quest.Objectives)
                {
                    Debug.Log("Test");
                    if (!objective.isComplete && objective.objectiveType == ObjectiveType.Collect && objective.itemType == itemType)
                    {
                        Debug.Log("Test");
                        objective.nbCollected++;
                        Debug.Log(objective.nbCollected + " / " + objective.nbToCollect);
                        if (objective.nbCollected == objective.nbToCollect)
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
