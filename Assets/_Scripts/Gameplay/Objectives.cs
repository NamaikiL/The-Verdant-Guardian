using UnityEngine;

namespace _Scripts.Gameplay
{
    public enum ObjectiveType
    {
        Collect, 
        Fight, 
        Escort,
        Talk
    }
    
    [CreateAssetMenu(fileName = "New Objectives", menuName = "Objective", order = 1)]
    public class Objectives : ScriptableObject
    {
        #region Variables

        [HideInInspector] public Quest parentQuest;
        public bool isRequired;
        public bool isComplete = false;
        
        public string name;
        public string description;
        public ObjectiveType objectiveType;

        [Header("Collect")] 
        public int nbToCollect;
        public int nbCollected;
        public ItemType itemType;

        [Header("Kill")] 
        [SerializeField] private int nbToFight;
        
        #endregion

        #region Methods

        public void CompleteObjective()
        {
            isComplete = true;
            parentQuest.TryToEndQuest();
        }

        #endregion
    }
}
