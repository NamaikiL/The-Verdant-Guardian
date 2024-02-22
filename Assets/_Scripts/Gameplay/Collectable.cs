using _Scripts.Scriptables;
using UnityEngine;

namespace _Scripts.Gameplay
{
    /**
     * <summary>
     * The collectable item type script.
     * </summary>
     */
    public class Collectable : Item
    {
        #region Variables
        
        [Header("Scriptable Data")]
        [SerializeField] private Collectables collectableScriptable;

        #endregion

        #region Properties

        // Scriptable Data.
        public Collectables CollectableScriptable => collectableScriptable;

        #endregion
        
    }
}
