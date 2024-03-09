using _Scripts.Scriptables;
using UnityEngine;

namespace _Scripts.Gameplay
{
    /**
     * <summary>
     * The management of a weapon.
     * </summary>
     */
    public class Weapon : Item
    {
        #region Variables
        
        // Weapon Stats.
        private int _weaponDamage;
        private float _weaponAttackSpeed;
        private float _weaponWeight;
        private float _weaponCost;
        private float _weaponSellCost;
        private float _weaponDurability;
        private ItemRarity _weaponRarity;

        #endregion

        #region Properties

        // Weapon Stats Properties.
        public int WeaponDamage => _weaponDamage;

        public float WeaponAttackSpeed => _weaponAttackSpeed;
        
        public float WeaponWeight => _weaponWeight;
        
        public float WeaponCost => _weaponCost;
        
        public float WeaponSellCost => _weaponSellCost;
        
        public float WeaponDurability => _weaponDurability;
        
        public ItemRarity WeaponRarity => _weaponRarity;

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
            InitializeWeaponStats(ItemScriptable);      // Initialize the weapon Stats
        }

        #endregion

        #region Initialize Methods

        /**
         * <summary>
         * Initialize the weapon stats by taking data from Scriptable Object.
         * </summary>
         * <param name="itemSO">The item data.</param>
         */
        private void InitializeWeaponStats(Items itemSO)
        {
            if(itemSO is Weapons weaponScriptable)
            {   // If the scriptable data is a weapon scriptable.
                _weaponDamage = weaponScriptable.WeaponDamage;
                _weaponAttackSpeed = weaponScriptable.WeaponAttackSpeed;
                _weaponWeight = weaponScriptable.ItemWeight;
                _weaponCost = weaponScriptable.ItemCost;
                _weaponSellCost = weaponScriptable.ItemSellCost;
                _weaponDurability = weaponScriptable.WeaponDurability;
                _weaponRarity = weaponScriptable.ItemRarity;
            }
            else
            {
                _weaponDamage = 0;
                _weaponAttackSpeed = 0f;
                _weaponWeight = 0f;
                _weaponCost = 0f;
                _weaponSellCost = 0f;
                _weaponDurability = 0f;
                _weaponRarity = 0f;
            }
        }

        #endregion

    }
}
