using _Scripts.Scriptables;


namespace _Scripts.Gameplay
{
    public class Consumable : Item
    {
        #region Variables

        private float _consumableRegen;
        private ConsumableType _consumableType;
        
        #endregion

        #region Properties

        public float ConsumableRegen => _consumableRegen;
        public ConsumableType CurrentConsumableType => _consumableType;

        #endregion

        #region Built-In Methods

        /**
         * <summary>
         * Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.
         * </summary>
         */
        void Start()
        {
            InitializeConsumable();
        }

        #endregion

        #region InitializeMethods

        /**
         * <summary>
         * Initialize the Consumable script values.
         * </summary>
         */
        private void InitializeConsumable()
        {
            if (itemScriptable is Consumables consumablesSO)
            {
                _consumableRegen = consumablesSO.ConsumableRegen;
                _consumableType = consumablesSO.CurrentConsumableType;
            }
        }

        #endregion
        
    }
}
