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

        [Header("Objective Conditions")]
        [HideInInspector] public Quest parentQuest;
        [SerializeField] private bool isRequired;
        [SerializeField] private bool isComplete;
        
        [Header("Objective Information")]
        [SerializeField] private string objectiveName;
        [SerializeField] private string description;
        [SerializeField] private ObjectiveType objectiveType;

        [Header("Collect Type")] 
        [SerializeField] private int nbToCollect;
        [SerializeField] private int nbCollected;
        [SerializeField] private ItemType itemType;

        [Header("Killing Type")] 
        [SerializeField] private int nbToFight;

        [Header("Escort Type")] 
        [SerializeField] private Transform escortPoint;

        [Header("Talk Type")] 
        [SerializeField] private GameObject npcToTalk;
        
        #endregion

        #region Properties

        // Objective Conditions.
        public bool IsRequired => isRequired;
        public bool IsComplete => isComplete;
        
        // Objective Information.
        public string ObjectiveName => objectiveName;
        public string Description => description;
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
        
        // Escort Objective Type.
        public Transform EscortPoint => escortPoint;
        
        // Talk Objective Type.
        public GameObject NpcToTalk => npcToTalk;
        
        #endregion
        
        #region Methods

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
