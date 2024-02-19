using _Scripts.Scriptables;
using UnityEngine;

namespace _Scripts.Gameplay
{
    public class Collectable : Item
    {
        #region Variables
        
        [Header("Scriptable Data")]
        [SerializeField] private Collectables collectablesScriptable;

        #endregion

        #region Properties

        // Scriptable Data.
        public Collectables CollectablesScriptable => collectablesScriptable;

        #endregion
        
    }
}
