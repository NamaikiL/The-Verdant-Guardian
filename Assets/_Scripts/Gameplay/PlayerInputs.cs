using UnityEngine;

namespace _Scripts.Gameplay
{
    public class PlayerInputs : MonoBehaviour
    {
        #region Variables

        [Header("Player Movements")] 
        [SerializeField] private KeyCode horizontalLeft = KeyCode.A;
        [SerializeField] private KeyCode horizontalRight = KeyCode.D;
        [SerializeField] private KeyCode verticalUp = KeyCode.W;
        [SerializeField] private KeyCode verticalDown = KeyCode.S;
        
        [Header("Player Interactions")]
        [SerializeField] private KeyCode jump = KeyCode.Space;
        [SerializeField] private KeyCode attack = KeyCode.Mouse0;
        [SerializeField] private KeyCode interaction = KeyCode.E;
        
        // Conditions.
        private bool _jumped;
        private bool _attack;
        private bool _interaction;

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

        // Player movements.
        public Vector2 Movement => _movement;

        // Singleton.
        public static PlayerInputs Instance => _instance;
        
        #endregion

        #region Builtin Methods

        /**
         * <summary>
         * Start is called before the first frame update.
         * </summary>
         */
        void Awake()
        {
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
        }

        #endregion

        #region Movements

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
