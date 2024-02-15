using UnityEngine;

namespace _Scripts.Scriptables
{
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

        public int Damage => damage;
        public float AttackSpeed => attackSpeed;
        public float Durability => durability;

        #endregion
    }
}
