using UnityEngine;

namespace _Scripts.Scriptables
{
    
    /**
     * <summary>
     * The Scriptable Object for weapons.
     * </summary>
     */
    [CreateAssetMenu(fileName = "New Weapon", menuName = "RPG/Weapon", order = 0)]
    public class Weapons : Items
    {
        #region Variables

        [Header("Weapon Properties")]
        [SerializeField] private int weaponDamage;
        [SerializeField] private float weaponAttackSpeed;
        [SerializeField] private float weaponDurability;

        #endregion

        #region Properties

        // Weapon Properties.
        public int WeaponDamage => weaponDamage;
        public float WeaponAttackSpeed => weaponAttackSpeed;
        public float WeaponDurability => weaponDurability;

        #endregion
    }
}
