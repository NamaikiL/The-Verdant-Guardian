using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Scriptables
{
    
    /**
     * <summary>
     * The Scriptable Object of quests.
     * </summary>
     */
    [CreateAssetMenu(fileName = "New Quest", menuName = "RPG/Quest", order = 0)]
    public class Quest : ScriptableObject
    {
        #region Variables
        
        [Header("Quest Conditions")]
        [SerializeField] private bool isActive;
        [SerializeField] private bool isComplete;
        
        [Header("Quest Information")]
        [SerializeField] private string questTitle;
        [TextArea][SerializeField] private string questDescription;
        [SerializeField] private List<Objectives> questObjectives = new List<Objectives>();

        // Event.
        public event Action<Quest> OnQuestComplete; 
        
        #endregion

        #region Properties

        // Conditions.
        public bool IsActive
        {
            set => isActive = value;
        }

        // Information.
        public string QuestTitle => questTitle;
        public string QuestDescription => questDescription;
        public List<Objectives> QuestObjectives => questObjectives;

        #endregion

        #region Built-In Methods

        /**
         * <summary>
         * This function is called when the object becomes enabled and active.
         * </summary>
         */
        private void OnEnable()
        {
            foreach (Objectives objective in questObjectives)
            {
                objective.parentQuest = this;
            }
        }

        #endregion
        
        #region Quest Handler

        /**
         * <summary>
         * Quest ending handler.
         * </summary>
         */
        public void TryToEndQuest()
        {
            foreach (Objectives objective in questObjectives)
            {
                if (!objective.IsComplete && objective.IsRequired) return;      // If one the objectives isn't complete.
                
                isComplete = true;
                isActive = false;
                OnQuestComplete?.Invoke(this);      // Invoke all subscribed functions from Event.
            }
        }

        #endregion"
    }
}
