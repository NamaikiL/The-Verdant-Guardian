using UnityEngine;

namespace _Scripts.Scriptables
{
    
    /**
     * <summary>
     * The Sciptable Object for weapons.
     * </summary>
     */
    [CreateAssetMenu(fileName = "New Weapon", menuName = "RPG/Weapon", order = 0)]
    public class Weapons : Items
    {
        #region Variables

        [Header("Weapon Properties")]
        [SerializeField] private int damage;
        [SerializeField] private float attackSpeed;
        [SerializeField] private float durability;

        #endregion

        #region Properties

        // Weapon Properties.
        //public int Damage => damage;
        public int Damage
        {
            get => damage;
        }
        public float AttackSpeed => attackSpeed;
        public float Durability => durability;

        #endregion
    }
}
