using UnityEngine;

namespace _Scripts.Scriptables
{
    
    /**
     * <summary>
     * Consumable Type.
     * </summary>
     */
    public enum ConsumableType
    {
        Health,
        Stamina
    }
    
    
    /**
     * <summary>
     * Consumables SO.
     * </summary>
     */
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
