using UnityEngine;

namespace _Scripts.Scriptables
{

    /**
     * <summary>
     * Enemies SO.
     * </summary>
     */
    [CreateAssetMenu(fileName = "New Enemy", menuName = "RPG/Enemy", order = 0)]
    public class Enemies : ScriptableObject
    {
        #region Variables

        [Header("NPC Features")]
        [SerializeField] private string enemyName;
        [SerializeField] private GameObject enemyModel;

        [Header("NPC Fight")]
        [SerializeField] private int enemyMaxHealth;
        [SerializeField] private int damage;
        [SerializeField] private int attackSpeed;

        #endregion

        #region Properties

        // Enemies Properties.
        public string EnemyName => enemyName;
        public GameObject EnemyModel => enemyModel;
        public int EnemyMaxHealth => enemyMaxHealth;
        public int Damage => damage;
        public int AttackSpeed => attackSpeed;

        #endregion
    }
}
