using _Scripts.Scriptables;

namespace _Scripts.Gameplay
{
    public class Weapon : Item
    {
        #region Variables
        
        // Weapon Stats.
        private int _damage;
        private float _attackSpeed;
        private float _weight;
        private float _cost;
        private float _sellCost;
        private float _durability;
        private ItemRarity _rarity;

        #endregion

        #region Properties
        
        // Weapon Stats Properties.
        public int Damage => _damage;

        public float AttackSpeed => _attackSpeed;
        
        public float Weight => _weight;
        
        public float Cost => _cost;
        
        public float SellCost => _sellCost;
        
        public float Durability => _durability;
        
        public ItemRarity Rarity => _rarity;

        #endregion

        #region Built-In Methods

        /**
         * <summary>
         * This function is called when the object becomes enabled and active.
         * </summary>
         */
        void OnEnable()
        {
            InitializeWeaponStats(scriptable);
        }

        #endregion

        #region Initialize Methods

        /**
         * <summary>
         * Initialize the weapon stats by taking data from Scriptable Object.
         * </summary>
         */
        private void InitializeWeaponStats(Items itemsScriptable)
        {
            if(itemsScriptable is Weapons weaponScriptable)
            {
                _damage = weaponScriptable.Damage;
                _attackSpeed = weaponScriptable.AttackSpeed;
                _weight = weaponScriptable.ItemWeight;
                _cost = weaponScriptable.ItemCost;
                _sellCost = weaponScriptable.ItemSellCost;
                _rarity = weaponScriptable.ItemRarity;
            }
            else
            {
                _damage = 0;
                _attackSpeed = 0f;
                _weight = 0f;
                _cost = 0f;
                _sellCost = 0f;
                _rarity = 0f;
            }
        }

        #endregion
        
    }
}
