using UnityEngine;

namespace _Scripts.Scriptables
{
    [CreateAssetMenu(fileName = "New Collectable", menuName = "RPG/Collectable", order = 0)]
    public class Collectables : Items
    {
        #region Variables

        [Header("Collectable Properties")]
        [SerializeField] private string collectableName;

        #endregion

        #region Properties

        // Collectable Properties.
        public string CollectableName => collectableName;

        #endregion
    }
}
