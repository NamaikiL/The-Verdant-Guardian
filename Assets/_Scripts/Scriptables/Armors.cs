using UnityEngine;

namespace _Scripts.Scriptables
{
    
    /**
     * <summary>
     * Armors SO.
     * </summary>
     */
    [CreateAssetMenu(fileName = "New Armor", menuName = "RPG/Armor", order = 0)]
    public class Armors : Items
    {
        #region Variables

        [Header("Armor Stats")]
        [SerializeField] private int armorDefense;
        [SerializeField] private int armorDurability;

        #endregion

        #region Properties

        // Armor Stats Properties.
        public int ArmorDefense => armorDefense;
        public int ArmorDurability => armorDurability;

        #endregion
    }
}
