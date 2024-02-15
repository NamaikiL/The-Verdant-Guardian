using _Scripts.Scriptables;
using UnityEngine;

namespace _Scripts.Gameplay
{
    public class Collectable : Item
    {
        #region Variables

        [SerializeField] private Collectables collectablesScriptable;

        #endregion

        #region Properties

        public Collectables CollectablesScriptable => collectablesScriptable;

        #endregion
        
        #region Built-In Methods

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        #endregion
        
    }
}
