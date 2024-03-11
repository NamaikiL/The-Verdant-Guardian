using _Scripts.Scriptables;

namespace _Scripts.Gameplay.ItemsScripts
{
    /**
     * <summary>
     * The armor item type script.
     * </summary>
     */
    public class Armor : Item
    {
        #region Variables

        // Armor Stats
        private int _armorDefense;
        private float _armorWeight;
        private float _armorCost;
        private float _armorSellCost;
        private float _armorDurability;
        private ItemRarity _armorRarity;

        #endregion
        
        #region Properties
        
        // Armor Stats Properties.
        public int ArmorDefense => _armorDefense;
        
        public float ArmorWeight => _armorWeight;
        
        public float ArmorCost => _armorCost;
        
        public float ArmorSellCost => _armorSellCost;
        
        public float ArmorDurability => _armorDurability;
        
        public ItemRarity ArmorRarity => _armorRarity;

        #endregion

        #region Built-In Methods

        /**
         * <summary>
         * Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.
         * </summary>
         */
        protected override void Start()
        {
            base.Start();
            InitializeArmorStats(ItemScriptable);      // Initialize the weapon Stats
        }

        #endregion

        #region Initialize Methods

        /**
         * <summary>
         * Initialize the weapon stats by taking data from Scriptable Object.
         * </summary>
         */
        private void InitializeArmorStats(Items itemsScriptable)
        {
            if(itemsScriptable is Armors armorSO)
            {   // If the scriptable data is a weapon scriptable.
                _armorDefense = armorSO.ArmorDefense;
                _armorWeight = armorSO.ItemWeight;
                _armorCost = armorSO.ItemCost;
                _armorSellCost = armorSO.ItemSellCost;
                _armorDurability = armorSO.ArmorDurability;
                _armorRarity = armorSO.ItemRarity;
            }
            else
            {
                _armorDefense = 0;
                _armorWeight = 0f;
                _armorCost = 0f;
                _armorSellCost = 0f;
                _armorDurability = 0f;
                _armorRarity = 0f;
            }
        }

        #endregion
    }
}
