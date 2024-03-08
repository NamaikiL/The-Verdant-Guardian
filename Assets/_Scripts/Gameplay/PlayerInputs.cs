using UnityEngine;

namespace _Scripts.Gameplay
{
    
    /**
     * <summary>
     * Inputs management.
     * </summary>
     */
    public class PlayerInputs : MonoBehaviour
    {
        #region Variables

        [Header("Player Movements")] 
        [SerializeField] private KeyCode horizontalLeft = KeyCode.A;
        [SerializeField] private KeyCode horizontalRight = KeyCode.D;
        [SerializeField] private KeyCode verticalUp = KeyCode.W;
        [SerializeField] private KeyCode verticalDown = KeyCode.S;
        [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
        [SerializeField] private KeyCode crouchKey = KeyCode.C;
        [SerializeField] private KeyCode dodgeKey = KeyCode.LeftControl;
        
        [Header("Player Interactions")]
        [SerializeField] private KeyCode jump = KeyCode.Space;
        [SerializeField] private KeyCode attack = KeyCode.Mouse0;
        [SerializeField] private KeyCode interaction = KeyCode.E;
        [SerializeField] private KeyCode inventory = KeyCode.I;
        [SerializeField] private KeyCode map = KeyCode.Semicolon;
        [SerializeField] private KeyCode pause = KeyCode.Escape;
        [SerializeField] private KeyCode questUI = KeyCode.Tab;

        // Conditions.
        private bool _jumped;
        private bool _attack;
        private bool _interaction;
        private bool _sprint;
        private bool _crouch;
        private bool _dodge;
        private bool _inventory;
        private bool _map;
        private bool _pause;
        private bool _questUI;

        // Player movements.
        private float _horizontal;
        private float _vertical;
        private Vector2 _movement = Vector2.zero;
        
        // Singleton.
        private static PlayerInputs _instance;
        
        #endregion

        #region Properties
        
        // Conditions.
        public bool Attack => _attack;
        public bool Jumped => _jumped;
        public bool Interaction => _interaction;
        public bool Sprint
        {
            get => _sprint;
            set => _sprint = value;
        }
        public bool Crouch => _crouch;
        public bool Dodge
        {
            get => _dodge;
            set => _dodge = value;
        }

        public bool Inventory => _inventory;
        public bool Map => _map;
        public bool Pause => _pause;
        public bool QuestUI => _questUI;

        // Player movements.
        public Vector2 Movement => _movement;

        // Singleton Property.
        public static PlayerInputs Instance => _instance;
        
        #endregion

        #region Builtin Methods

        /**
         * <summary>
         * Unity calls Awake when an enabled script instance is being loaded.
         * </summary>
         */
        void Awake()
        {
            // Singleton.
            if(_instance) Destroy(gameObject);
            _instance = this;
        }


        /**
         * <summary>
         * Update is called once per frame.
         * </summary>
         */
        void Update()
        {
            _movement.Set(HorizontalMovementsCalculation(), VerticalMovementsCalculation());
            
            _jumped = Input.GetKey(jump);
            _attack = Input.GetKey(attack);
            _interaction = Input.GetKeyDown(interaction);
            _inventory = Input.GetKeyDown(inventory);
            _map = Input.GetKeyDown(map);
            _pause = Input.GetKeyDown(pause);
            _questUI = Input.GetKeyDown(questUI);
            _crouch = Input.GetKeyDown(crouchKey);

            // Sprint.
            if (Input.GetKeyDown(sprintKey)) _sprint = true;
            else if(Input.GetKeyUp(sprintKey)) _sprint = false;
            
            // Dodge.
            if(Input.GetKeyDown(dodgeKey)) _dodge = true;
            
        }

        #endregion

        #region Movements Methods
        
        /**
         * <summary>
         * Calculate the Horizontal Movements.
         * </summary>
         */
        private float HorizontalMovementsCalculation() {
            if (Input.GetKey(horizontalRight)) return _horizontal = Mathf.Clamp(_horizontal + Time.deltaTime, 0f, 1f);
            if (Input.GetKey(horizontalLeft)) return _horizontal = Mathf.Clamp(_horizontal - Time.deltaTime, -1f, 0f);
            return _horizontal = 0f;
        }
        
        
        /**
         * <summary>
         * Calculate the Vertical Movements.
         * </summary>
         */
        private float VerticalMovementsCalculation() {
            if (Input.GetKey(verticalUp)) return _vertical = Mathf.Clamp(_vertical + Time.deltaTime, 0f, 1f);
            if (Input.GetKey(verticalDown)) return _vertical = Mathf.Clamp(_vertical - Time.deltaTime, -1f, 0f);
            return _vertical = 0f;
        }

        #endregion
    }
}
