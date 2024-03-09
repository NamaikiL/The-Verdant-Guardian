using UnityEngine;

namespace _Scripts.Scriptables
{
    public enum ConsumableType
    {
        health,
        stamina
    }
    
    
    [CreateAssetMenu(fileName = "New Consumable", menuName = "RPG/Consumable", order = 0)]
    public class Consumables : Items
    {
        #region Variables

        [Header("Consumables Properties")]
        [SerializeField] private float consumableRegen;
        [SerializeField] private ConsumableType consumableType;

        #endregion

        #region Properties

        // Consumables Properties.
        public float ConsumableRegen => consumableRegen;
        public ConsumableType CurrentConsumableType => consumableType;

        #endregion


    }
}
