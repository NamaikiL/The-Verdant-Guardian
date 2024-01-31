using UnityEngine;

namespace _Scripts.Gameplay
{
    public class PlayerInputs : MonoBehaviour
    {
        #region Variables
        private bool _jumped = false;
        private bool _attack = false;
        private bool _interaction = false;

        private Vector2 _movement = Vector2.zero;

        private static PlayerInputs _instance;
        #endregion

        #region Properties
        public bool Attack => _attack;
        public bool Jumped => _jumped;
        public bool Interaction => _interaction;

        public Vector2 Movement => _movement;

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
            if(_instance != null){
                Destroy(gameObject);
                return;
            }
            _instance = this;
        }


        /**
         * <summary>
         * Update is called once per frame.
         * </summary>
         */
        void Update()
        {
            _movement.Set(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            _jumped = Input.GetButton("Jump");
            _attack = Input.GetButton("Fire1");
            _interaction = Input.GetButtonDown("Fire2");
        }

        #endregion
    }
}
