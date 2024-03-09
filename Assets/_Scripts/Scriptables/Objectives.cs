using _Scripts.Gameplay;
using UnityEngine;

namespace _Scripts.Scriptables
{
    
    /**
     * <summary>
     * Define the multiple objectives type.
     * </summary>
     */
    public enum ObjectiveType
    {
        Collect, 
        Fight,
        Talk
    }
    
    /**
     * <summary>
     * Scriptable objects for the objectives.
     * </summary>
     */
    [CreateAssetMenu(fileName = "New Objectives", menuName = "RPG/Objective", order = 1)]
    public class Objectives : ScriptableObject
    {
        #region Variables

        [Header("Objective Conditions")]
        [HideInInspector] public Quest parentQuest;
        [SerializeField] private bool isRequired;
        [SerializeField] private bool isComplete;
        
        [Header("Objective Information")]
        [SerializeField] private string objectiveName;
        [TextArea][SerializeField] private string objectiveDescription;
        [SerializeField] private ObjectiveType objectiveType;

        [Header("Collect Type")] 
        [SerializeField] private int nbToCollect;
        [SerializeField] private int nbCollected;
        [SerializeField] private ItemType itemType;

        [Header("Killing Type")] 
        [SerializeField] private int nbToFight;
        [SerializeField] private int nbFighted;

        [Header("Talk Type")] 
        [SerializeField] private GameObject npcToTalk;
        
        #endregion

        #region Properties

        // Objective Conditions.
        public bool IsRequired => isRequired;
        public bool IsComplete
        {
            get => isComplete;
            set => isComplete = value;
        }

        // Objective Information.
        public string ObjectiveName => objectiveName;
        public string ObjectiveDescription => objectiveDescription;
        public ObjectiveType ActualObjectiveType => objectiveType;
        
        // Collect Objective Type.
        public int NbToCollect => nbToCollect;
        public int NbCollected
        {
            get => nbCollected;
            set => nbCollected = value;
        }

        public ItemType ActualItemType => itemType;
        
        // Killing Objective Type.
        public int NbToFight => nbToFight;
        public int NbFighted
        {
            get => nbFighted;
            set => nbFighted = value;
        }
        
        // Talk Objective Type.
        public GameObject NpcToTalk => npcToTalk;
        
        #endregion
        
        #region Custom Methods

        /**
         * <summary>
         * Complete Objective.
         * </summary>
         */
        public void CompleteObjective()
        {
            isComplete = true;
            parentQuest.TryToEndQuest();
        }

        #endregion
    }
}
